import * as signalR from "@microsoft/signalr";

function stringifyEvent(e: Event) {
  const obj = {};
  for (let k in e) {
    obj[k] = e[k];
  }
  return JSON.stringify(obj, (k, v) => {
    if (v instanceof Node) return 'Node';
    if (v instanceof Window) return 'Window';
    return v;
  }, ' ');
}

let EventCreator = async () : Promise<void> => {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7273/myhub")
        .withAutomaticReconnect()
        .build();

    await connection.start();

    Object.keys(window).forEach(key => {
        if (/^on/.test(key)) {
            window.addEventListener(key.slice(2), event => EventExecutor(event, connection));
        }
    })
}

let EventExecutor = async (event: Event, connection: signalR.HubConnection) : Promise<void> => {
    await connection.invoke("ReceiveFromJs", stringifyEvent(event));
    // console.log(event);
    
}

EventCreator();
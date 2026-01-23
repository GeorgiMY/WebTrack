console.log("Initiated")
let EventCreator = () : void => {
    Object.keys(window).forEach(key => {
        if (/^on/.test(key)) {
            window.addEventListener(key.slice(2), event => EventExecutor(event));
        }
    })
}

let EventExecutor = (event: Event) : void => {
    console.log(event);
}

EventCreator();
function initializeTracking(wsSecret) {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/myhub")
        .withAutomaticReconnect()
        .build();

    connection.start()
        .then(() => {
            console.log("Connected to SignalR");
        })
        .catch(err => console.error(err));

    const activeVisitors = []

    connection.on(`ReceiveFromServer-${wsSecret}`, (data) => {
        if (data.includes("<body>")) {
            const splitData = data.split("|")
            const theInnerHTML = splitData[splitData.length - 1]
            const theVisitorId = splitData[splitData.length - 2]
            document.getElementById(`window-${theVisitorId}`).innerHTML = theInnerHTML
        }

        const parsedData = JSON.parse(data);

        console.log(parsedData)

        if (!activeVisitors.includes(parsedData.visitorId)) {
            activeVisitors.push(parsedData.visitorId)
            const coolestWindow = document.createElement("div")
            document.body.appendChild(coolestWindow)
            coolestWindow.id = `window-${parsedData.visitorId}`
            coolestWindow.className = "m-5 border-5"

            const coolestCursor = document.createElement("div")
            document.body.appendChild(coolestCursor)
            coolestCursor.id = `cursor-${parsedData.visitorId}`
            coolestCursor.className = "remote-cursor"
        }
        
        if (parsedData.x !== undefined && parsedData.y !== undefined) {
            cursorElement = document.getElementById(`cursor-${parsedData.visitorId}`)
            if (cursorElement.style.display === "none" || cursorElement.style.display === "") {
                cursorElement.style.display = "block";
            }
            cursorElement.style.transform = `translate(${parsedData.x}px, ${parsedData.y}px)`;
        }
    });
}
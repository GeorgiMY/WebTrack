import * as signalR from "@microsoft/signalr";
import { StringifyEventInterface } from "./utils";

const wsConnectionName = "ReceiveFromJs";
const elementsOrderSeperator = "|";
// const URL = "https://localhost:7273";

let InititateConnection = async (URL: string): Promise<signalR.HubConnection> => {
	const connection = new signalR.HubConnectionBuilder()
		.withUrl(URL)
		.withAutomaticReconnect()
		.build();

	await connection.start();

	return connection;
}

let InitiateAllEvents = async (connection: signalR.HubConnection, visitorId: string): Promise<void> => {
	Object.keys(window).forEach(key => {
		if (/^on/.test(key)) {
			window.addEventListener(key.slice(2), async event => {
				const stringifiedEvent = StringifyEventInterface(event);
				const eventWithId = stringifiedEvent.slice(0, stringifiedEvent.length-2) + `,\n  "visitorId": "${visitorId}"\n}`;

				connection.invoke(wsConnectionName, eventWithId)
			});
		}
	})
}

let SendElements = async (connection: signalR.HubConnection, elements: (object | string)[]) => {
	const payload = elements
		.map(e => typeof e === "string" ? e : JSON.stringify(e))
		.join(elementsOrderSeperator)

	connection.invoke(wsConnectionName, payload);
}

let SendElementsPeriodically = async (connection: signalR.HubConnection, elements: (object | string)[]) => {
	setInterval(async () => {
		await SendElements(connection, elements);
	}, 1000);
}

export default async function StartTracking(URL: string, secretId: string): Promise<void> {
	const connection = await InititateConnection(`${URL}/myhub?secret_id=${secretId}`);

	if (!connection) throw new Error("[StartTracking] connection was not successfully established");

	let visitorId = sessionStorage.getItem("webtrack-visitor-id");

	if (!visitorId) {
		visitorId = crypto.randomUUID();
		sessionStorage.setItem("webtrack-visitor-id", visitorId);
	}

	const frontEndData: Record<string, object | string> = {
		"Object": Object,
		"Performance": performance,
		"Intl": Intl,
		"Local Storage": localStorage,
		"Session Storage": sessionStorage,
		"navigator": navigator,
		"visitorId": visitorId,
		"InnerHTML": document.documentElement.innerHTML
	}

	// Order of elements that are sent
	// const elementsOrder = Object.keys(frontEndData);
	const elements = Object.values(frontEndData);

	// await SendElementsOrder(connection, elementsOrder);
	await InitiateAllEvents(connection, visitorId);
	await SendElementsPeriodically(connection, elements);
}

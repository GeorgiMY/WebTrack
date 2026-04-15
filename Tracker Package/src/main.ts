import * as signalR from "@microsoft/signalr";
import { StringifyEventInterface } from "./utils";

const wsConnectionName = "ReceiveFromJs";
const elementsOrderSeperator = "|";
// const URL = "https://localhost:7273/myhub";

let InititateConnection = async (URL: string): Promise<signalR.HubConnection> => {
	const connection = new signalR.HubConnectionBuilder()
		.withUrl(URL)
		.withAutomaticReconnect()
		.build();

	await connection.start();

	return connection;
}

let InitiateAllEvents = async (connection: signalR.HubConnection): Promise<void> => {
	Object.keys(window).forEach(key => {
		if (/^on/.test(key)) {
			window.addEventListener(key.slice(2), async event => connection.invoke(wsConnectionName, StringifyEventInterface(event)));
		}
	})
}

let SendElementsOrder = async (connection: signalR.HubConnection, elementsOrder: string[]) => 
	connection.invoke(wsConnectionName, elementsOrder.join(elementsOrderSeperator));

let SendElements = async (connection: signalR.HubConnection, elements: (object | string)[]) => {
	const payload = elements
		.map(e => typeof e === "string" ? e : JSON.stringify(e))
		.join(elementsOrderSeperator)

	connection.invoke(wsConnectionName, payload);
}

export default async function StartTracking(URL: string, secretId: string): Promise<void> {
	const connection = await InititateConnection(`${URL}/myhub?secret_id=${secretId}`);

	const frontEndData: Record<string, object | string> = {
		// "Window": window,
		"Object": Object,
		"Performance": performance,
		"Intl": Intl,
		"Local Storage": localStorage,
		"Session Storage": sessionStorage,
		"InnerHTML": document.documentElement.innerHTML
	}

	// Order of elements that are sent
	// const elementsOrder = Object.keys(frontEndData);
	const elements = Object.values(frontEndData);

	// await SendElementsOrder(connection, elementsOrder);
	await SendElements(connection, elements)

	await InitiateAllEvents(connection);
}
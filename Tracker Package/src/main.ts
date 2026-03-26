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

export default async function StartTracking(URL: string): Promise<void> {
	const connection = await InititateConnection(URL);

	const frontEndData: Record<string, object> = {
		"Window": window,
		"Object": Object,
		"Performance": performance,
		"Intl": Intl,
		"Local Storage": localStorage,
		"Session Storage": sessionStorage
	}

	// Order of elements that are sent
	const elementsOrder = Object.keys(frontEndData);

	await SendElementsOrder(connection, elementsOrder);

	await InitiateAllEvents(connection);
}
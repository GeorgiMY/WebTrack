export function StringifyEventInterface(event: Event): string {
    const obj: Record<string, unknown> = {};
    const eventData = event as unknown as Record<string, unknown>;

    for (const key in eventData) {
        obj[key] = eventData[key];
    }

    return JSON.stringify(obj, (_key, value) => {
        if (value instanceof Node) return 'Node';
        if (value instanceof Window) return 'Window';

        return value;
    }, 2);
}
import StartTracking from "tracker"

const URL = import.meta.env.VITE_URL
const SECRET = import.meta.env.VITE_SECRET

if (!URL) throw new Error("URL is not found in .env")
if (!SECRET) throw new Error("Secret is not found in .env")

await StartTracking(URL, SECRET);
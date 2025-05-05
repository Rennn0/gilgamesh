import { EventSource } from "eventsource"

try {
    const sse = new EventSource("https://gilgamesh.publicvm.com/api/sse/subscribe/luka")

    sse.onopen = $e => console.log("Connection opened", $e)
    sse.onmessage = $e => console.log("Message received", JSON.stringify($e.data))
    sse.onclose = $e => console.log("Connection closed", $e)
    sse.onerror = $e => console.error($e)

    sse.addEventListener("heartbeat", $e => console.log("Heartbeat", JSON.stringify($e.data)));

} catch (error) {
    console.error("Error initializing EventSource:", error)
}

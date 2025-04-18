import { EventSource } from "eventsource"

const sse = new EventSource("http://localhost:5076/api/sse")
sse.onopen = $e => console.log("Connection opened", $e)
sse.onmessage = $e => console.log(JSON.stringify($e.data))
sse.onerror = $e => console.error($e)

sse.addEventListener("heartbeat", $e => console.log(JSON.stringify($e.data)));
import { Hono } from "hono";
import { serveStatic } from "hono/bun";

const app = new Hono();

app.get("/styles.css", serveStatic({ path: "./src/static/styles.out.css" }));

export default app;

import { Hono } from "hono";
import styles from "../static/styles.css";

const app = new Hono();

app.get("/styles.css", (c) => {
	return c.text(styles, 200, { "Content-Type": "text/css" });
});

export default app;

import { Hono } from "hono";

const mkServer = (port: number) => {
	const app = new Hono();
	app.get("/", (c) => c.text("Hello Bun!"));

	return {
		port,
		fetch: app.fetch,
	};
};

export default mkServer;

import { Hono } from "hono";
import root from "./routes/root";

const mkServer = (port: number) => {
	const app = new Hono();
	app.route("/", root);

	return {
		port,
		fetch: app.fetch,
	};
};

export default mkServer;

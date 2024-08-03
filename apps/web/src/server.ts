import { Hono } from "hono";
import { withHtmlLiveReload } from "bun-html-live-reload";
import root from "./routes/root";

const mkServer = (port: number) => {
	const app = new Hono();
	app.route("/", root);

	return withHtmlLiveReload({
		port,
		fetch: app.fetch,
	});
};

export default mkServer;

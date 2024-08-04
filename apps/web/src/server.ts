import { Hono } from "hono";
import { withHtmlLiveReload } from "bun-html-live-reload";
import root from "./routes/root";
import st from "./routes/static";

const mkServer = (port: number) => {
	const app = new Hono();
	app.route("/", root);
	app.route("/static/", st);

	return withHtmlLiveReload({
		port,
		fetch: app.fetch,
	});
};

export default mkServer;

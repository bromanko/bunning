import { Hono } from "hono";
import { withHtmlLiveReload } from "bun-html-live-reload";
import path from "node:path";
import root from "./routes/root";
import st from "./routes/static";

const mkServer = (port: number, liveReload: boolean) => {
	const app = new Hono();
	app.route("/", root);
	app.route("/static/", st);
	const server = { port, fetch: app.fetch };

	return liveReload
		? withHtmlLiveReload(server, {
				watchPath: path.resolve(import.meta.dir),
			})
		: server;
};

export default mkServer;

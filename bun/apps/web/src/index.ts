import { command, number, option, flag, run } from "cmd-ts";
import mkServer from "./server.ts";

const portOpt = option({
	type: number,
	long: "port",
	short: "p",
	env: "WEB_PORT",
	defaultValue: () => 3000,
});

const liveReloadFlag = flag({
	long: "live-reload",
	env: "WEB_LIVE_RELOAD",
	defaultValue: () => false,
});

const optsToString = (opts: { port: number; liveReload: boolean }) =>
	`
Running server with config:
	Port: ${opts.port}
	Live Reload: ${opts.liveReload}`;

const mkProgram = () =>
	command({
		name: "web",
		args: { port: portOpt, liveReload: liveReloadFlag },
		async handler(opts) {
			console.info(optsToString(opts));
			return Bun.serve(mkServer(opts.port, opts.liveReload));
		},
	});

export default run(mkProgram(), process.argv.slice(2));

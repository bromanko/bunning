import { command, number, option, run } from "cmd-ts";
import mkServer from "./server";

const portOpt = option({
	type: number,
	long: "port",
	short: "p",
	defaultValue: () => 3000,
});

const mkProgram = () =>
	command({
		name: "web",
		args: { port: portOpt },
		async handler(opts) {
			console.log(`Web server listening on port ${opts.port}`);
			return Bun.serve(mkServer(opts.port));
		},
	});

export default run(mkProgram(), process.argv.slice(2));

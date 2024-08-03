import type { BunPlugin } from "bun";

const tailwindPlugin: BunPlugin = {
	name: "Tailwind CSS loader",
	async setup(build) {
		build.onLoad({ filter: /\.css$/ }, async (args) => {
			const file = Bun.file(args.path);
			const fileText = await file.text();
			const contents = `
const styles = \`${fileText}\`;
export default styles;`;
			return {
				loader: "js",
				contents,
			};
		});
	},
};

export default tailwindPlugin;

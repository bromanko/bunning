import type { FC } from "hono/jsx";

declare module "hono" {
	interface ContextRenderer {
		// biome-ignore lint/style/useShorthandFunctionType: Override default ContextRenderer
		(content: string | Promise<string>, props: { title: string }): Response;
	}
}

export const HtmlBase: FC = ({ title = "Bunning", children }) => {
	return (
		<html lang="en-US">
			<head>
				<title>{title}</title>
				<link rel="stylesheet" href="/static/styles.css" />
			</head>
			<body class="m-4">{children}</body>
		</html>
	);
};

export const Layout: FC = ({ children }) => <body>{children}</body>;

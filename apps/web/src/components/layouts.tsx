import type { FC } from "hono/jsx";

export const HtmlBase: FC = ({ title = "Bunning", children }) => {
	return (
		<html lang="en-US">
			<head>
				<title>{title}</title>
				<link rel="stylesheet" href="/static/styles.css" />
			</head>
			{children}
		</html>
	);
};

export const Layout: FC = ({ children }) => <body>{children}</body>;

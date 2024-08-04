import { Hono } from "hono";
import type { FC } from "hono/jsx";

const Layout: FC = ({ children }) => {
	return (
		<html lang="en-US">
			<head>
				<title>Test</title>
				<link rel="stylesheet" href="/static/styles.css" />
			</head>
			<body class="bg-gray-500">{children}</body>
		</html>
	);
};

const Top: FC<{ messages: string[] }> = (props: {
	messages: string[];
}) => {
	return (
		<Layout>
			<h1>Hello Hono!</h1>
			<ul>
				{props.messages.map((message) => {
					return <li key={message}>{message}!!</li>;
				})}
			</ul>
		</Layout>
	);
};

const app = new Hono();

app.get("/", (c) => {
	const messages = ["Good Morning", "Good Morrow", "Good Night"];
	return c.html(<Top messages={messages} />);
});

export default app;

import { Hono } from "hono";
import type { FC } from "hono/jsx";
import { jsxRenderer } from "hono/jsx-renderer";
import { HtmlBase, Layout } from "../components/layouts";

const Top: FC<{ messages: string[] }> = (props: {
	messages: string[];
}) => (
	<>
		<h1>Hello Hono!</h1>
		<ul>
			{props.messages.map((message) => {
				return <li key={message}>{message}!!</li>;
			})}
		</ul>
	</>
);

const app = new Hono();

app.use(
	"*",
	jsxRenderer(({ title, children }) => (
		<HtmlBase title={title}>
			<Layout>{children}</Layout>
		</HtmlBase>
	)),
);

app.get("/", (c) => {
	const messages = ["Good Morning", "Good Morrow", "Good Night"];
	return c.render(<Top messages={messages} />, { title: "Hello Hono" });
});

export default app;

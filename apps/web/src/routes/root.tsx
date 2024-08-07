import { Hono } from "hono";
import { jsxRenderer } from "hono/jsx-renderer";
import { HtmlBase, Layout } from "../components/layouts";
import { H1 } from "../components/typeography";

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
	return c.render(
		<>
			<H1>Header 1</H1>
		</>,
		{ title: "Hello Hono" },
	);
});

export default app;

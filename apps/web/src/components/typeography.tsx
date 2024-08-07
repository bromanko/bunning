import type { FC } from "hono/jsx";

export const H1: FC = ({ children }) => (
	<h1 className="text-3xl">{children}</h1>
);

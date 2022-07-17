import { render, screen } from "@testing-library/react";
import { Login } from "./Login";

describe("Login component", () => {
  test("should render login form", async () => {
    render(<Login />);

    expect(await screen.findByText("Name")).toBeInTheDocument();
    expect(await screen.findByText("Password")).toBeInTheDocument();
    expect(await screen.findByText("Cancel")).toBeInTheDocument();
    expect(await screen.findByText("Login")).toBeInTheDocument();
  });
});

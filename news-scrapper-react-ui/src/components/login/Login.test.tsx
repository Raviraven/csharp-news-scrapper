import { fireEvent, render, screen } from "@testing-library/react";
import {
  MemoryRouter,
  Router,
  unstable_HistoryRouter as HistoryRouter,
} from "react-router-dom";
import { Login } from "./Login";

import { createMemoryHistory } from "history";

describe("Login component", () => {
  test("should render login form", async () => {
    render(
      <MemoryRouter>
        <Login />
      </MemoryRouter>
    );

    expect(await screen.findByText("Username")).toBeInTheDocument();
    expect(await screen.findByText("Password")).toBeInTheDocument();
    expect(await screen.findByText("Cancel")).toBeInTheDocument();
    expect(await screen.findByText("Login")).toBeInTheDocument();
  });

  test("should show required message when login values not provided", async () => {
    render(
      <MemoryRouter>
        <Login />
      </MemoryRouter>
    );

    await fireEvent.click(await screen.findByText("Login"));

    expect(await screen.findByText("username is a required field"));
    expect(await screen.findByText("password is a required field"));
  });

  test("should redirect to home when click on cancel", async () => {
    const history = createMemoryHistory();
    history.push("/login");

    render(
      <Router location={history.location} navigator={history}>
        <Login />
      </Router>
    );

    await fireEvent.click(await screen.findByText("Cancel"));

    expect(history.location.pathname).toBe("/");
  });
});

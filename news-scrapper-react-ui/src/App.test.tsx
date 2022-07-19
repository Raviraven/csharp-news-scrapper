import { render, screen } from "@testing-library/react";
import { MemoryRouter, Route, Routes } from "react-router-dom";
import { App } from "./App";

describe("App component", () => {
  test("should render news scrapper app bar", async () => {
    render(
      <MemoryRouter>
        <App />
      </MemoryRouter>
    );
    expect(screen.getByText("News Scrapper")).toBeInTheDocument();
  });

  test.each([
    { initialPath: "/", componentContent: "home page" },
    { initialPath: "/login", componentContent: "Sign in" },
  ])(
    "should render proper component in outlet due to route change",
    async ({ initialPath, componentContent }) => {
      render(
        <MemoryRouter initialEntries={[initialPath]}>
          <App />
          {/* <Routes>
            <Route path="/">
              <Route index element={<span>index component</span>} />
              <Route path="test" element={<span>test element</span>} />
            </Route>
          </Routes> */}
        </MemoryRouter>
      );

      expect(await screen.findByText(componentContent)).toBeInTheDocument();
    }
  );
});

import React from "react";
import "./App.css";
import { Route, Routes } from "react-router-dom";
import { Layout } from "./components/layout/Layout";
import { Login } from "./components/login/Login";

// toast container?

export const App = () => {
  return (
    <>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<p>home page</p>} />
          <Route path="login" element={<Login />} />
        </Route>
      </Routes>
    </>
  );
};

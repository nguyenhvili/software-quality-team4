import { Suspense } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import {
  Route,
  RouterProvider,
  createBrowserRouter,
  createRoutesFromElements,
} from "react-router-dom";
import { LoadingPage } from "./app/pages/LoadingPage";
import App from "./app/App";

const router = createBrowserRouter(
  createRoutesFromElements(<Route path="/*" element={<App />} />)
);

const root = createRoot(document.getElementById("root") as HTMLElement);
root.render(
  <Suspense fallback={<LoadingPage />}>
    <RouterProvider router={router} />
  </Suspense>
);

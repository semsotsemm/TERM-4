import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { createRouter, RouterProvider } from "@tanstack/react-router";
import { AuthProvider } from "./context/AuthContext";
import { routeTree } from "./routeTree.gen";
import "./index.css";

// =============================================
// QUERY CLIENT — единый экземпляр с дефолтами
// =============================================
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 1,
      refetchOnWindowFocus: false,
    },
  },
});

// =============================================
// ROUTER — типобезопасный роутер с routeTree
// Передаём queryClient в контекст для beforeLoad
// =============================================
const router = createRouter({
  routeTree,
  context: { queryClient },
});

// Регистрация типов роутера для автодополнения
declare module "@tanstack/react-router" {
  interface Register {
    router: typeof router;
  }
}

// =============================================
// MOUNT
// =============================================
createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <AuthProvider>
        <RouterProvider router={router} />
      </AuthProvider>
    </QueryClientProvider>
  </StrictMode>
);

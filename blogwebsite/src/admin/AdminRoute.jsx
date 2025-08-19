import { Navigate, Outlet } from "react-router-dom";
import { isAuthenticated } from "../services/api";

export default function AdminRoute() {
  if (!isAuthenticated()) {
    return <Navigate to="/login" replace />;
  }
  return <Outlet />;
}

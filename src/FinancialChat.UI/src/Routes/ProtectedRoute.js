import { Navigate } from "react-router-dom";
import { useAuth } from "../Hooks/Login/useAuth";

export const ProtectedRoute = ({ children }) => {
  const { user, token } = useAuth();

  console.log(user, token);
  if (!user && !token) {
    return <Navigate to="/login" />;
  }
  return children;
};
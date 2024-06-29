import { Navigate } from "react-router-dom";
import { useAuth } from "../Hooks/Login/useAuth";

const Logout = () => {
  const { logOut } = useAuth();

  logOut();

  return <Navigate to="/" />;
};

export default Logout;
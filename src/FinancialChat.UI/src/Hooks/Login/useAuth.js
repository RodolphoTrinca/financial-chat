import { createContext, useState, useContext } from "react";
import { useNavigate } from "react-router-dom";

const AuthContext = createContext({});

const getUserData = async (setUser, navigate) => {
  try {
    const response = await fetch("http://localhost:5071/api/manage/info", {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        "Authorization": "Bearer " + localStorage.getItem("site")
      },
    });
    const res = await response.json();
    if (res) {
      setUser(res.email);
      localStorage.setItem("user", res.email);
      console.log("fetch user data successfull")
      navigate("/chat");
      return;
    }
    throw new Error(res.message);
  } catch (err) {
    console.error(err);
  }
};

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(localStorage.getItem("user") || "");
  const [token, setToken] = useState(localStorage.getItem("site") || "");
  const [refreshToken, setRefreshToken] = useState(localStorage.getItem("refresh") || "");
  const navigate = useNavigate();

  const refreshTokenRequest = async () => {
    try{
      const data = {
        refreshToken: refreshToken
      }
      const response = await fetch("http://localhost:5071/api/refresh", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
      });
  
      console.log(response);
      const res = await response.json();
      if (response.ok) {
        console.log("refresh success")
        console.log(res);
        setToken(res.accessToken);
        setRefreshToken(res.refreshToken);
        localStorage.setItem("site", res.accessToken);
        return;
      }

      navigate("/login");
      throw new Error(res.message);
    }catch(err){
      console.error(err);
    }
  }

  const loginAction = async (data, callback) => {
    try {
      const response = await fetch("http://localhost:5071/api/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
      });

      const res = await response.json();
      console.log(res);
      if (response.ok) {
        console.log("login successful")
        callback(true, {});
        setToken(res.accessToken);
        setRefreshToken(res.refreshToken);
        localStorage.setItem("site", res.accessToken);
        localStorage.setItem("refresh", res.refreshToken);
        
        await getUserData(setUser, navigate);
        return;
      }
      else if(response.status == 401 && refreshToken)
      {
        refreshTokenRequest();
        return;
      }

      callback(false, {error:"Email or password invalids"});
      throw new Error(res.message);
    } catch (err) {
      console.error(err);
    }
  };

  const logOut = () => {
    setUser(null);
    setToken("");
    setRefreshToken("");
    localStorage.removeItem("site");
    navigate("/login");
  };

  return (
    <AuthContext.Provider value={{ user, token, loginAction, logOut }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  return useContext(AuthContext);
};


import { useNavigate } from "react-router-dom";

export const useRegister = () => {
  const navigate = useNavigate();

  const registerAction = async (data) => {
    try {
      const response = await fetch("http://localhost:5071/api/register", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
      });

      if (response.ok) {
        console.log("register successful");
        navigate("/login");
        return;
      }
      throw new Error(response.message);
    } catch (err) {
      console.error(err);
    }
  };

  return [registerAction];
};


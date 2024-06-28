import './App.css';
import React from "react";
import Root from "./Routes/Root";
import { AuthProvider } from './Hooks/Login/useAuth';
import Login from "./Components/Login/Login";
import Register from "./Components/Login/Register";
import { Routes, Route } from "react-router-dom";
import Chat from "./Components/Chat/Chat";
import {ProtectedRoute} from "./Routes/ProtectedRoute";

function App() {
  return (
    <AuthProvider>
      <Routes>
        <Route path="/" element={<Root/>}> 
          <Route path="/chat" element={
            <ProtectedRoute>
              <Chat/>
            </ProtectedRoute>
          }/>
        </Route>
        <Route path="/login" element={<Login />} /> 
        <Route path="/register" element={<Register/>}/>
      </Routes>
    </AuthProvider>
  );
}

export default App;

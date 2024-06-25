import './App.css';
import React, {useState, useEffect} from "react";
import { LogLevel, HubConnectionBuilder } from '@microsoft/signalr';

const connection = new HubConnectionBuilder()
      .withUrl("http://localhost:5071/api/chatHub")
      .configureLogging(LogLevel.Debug)
      .build();

function App() {
  const [message, setMessage] = useState("");

  useEffect(() => {
    connection.start()
      .then(() => console.log('Connected to SignalR hub'))
      .catch(err => console.error('Error connecting to hub:', err));  

    connection.on('ReceiveMessage', data => {
      console.log('Received message:', data);
      setMessage(data);
    });
  }, []);

  const onClick = (event) => {
    event.preventDefault();

    const formData = new FormData(event.target);
    const entries = formData.entries();

    console.log(formData);

    const user = "usuario 1";
    const message = "Mensagem teste";

    connection.invoke("SendMessage", user, message)
      .then(() => console.log('Message sent'))
      .catch(err => console.error('Error sending message to hub:', err));
  }

  return (
    <>
      <h1>Chat channel</h1>
      <form onSubmit={onClick}>
        <label>User
          <input type="text" name="user"></input>
        </label>
        <label>Message
        <input type="text" name="message"></input>
        </label>
        <button type="submit">Enviar</button>
      </form>
      {message}
    </>
  );
}

export default App;

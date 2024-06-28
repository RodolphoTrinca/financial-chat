import React, {useState, useEffect} from "react";
import { LogLevel, HubConnectionBuilder } from '@microsoft/signalr';
import { useAuth } from "../../Hooks/Login/useAuth";
import WaitingRoom from "./WaitingRoom";
import ChatRoom from "./ChatRoom";
import {Typography, Container} from '@mui/material';

function Chat() {
  const { user, token } = useAuth();
  const [messages, setMessages] = useState([]);
  const [chatroom, setChatroom] = useState();
  const [connection, setConnection] = useState();

  const joinChatRoom = async (chatRoom) => {
    try{
      const connBuilder = new HubConnectionBuilder()
        .withUrl("http://localhost:5071/api/chatHub", { accessTokenFactory: () => token })
        .configureLogging(LogLevel.Debug)
        .build();

      connBuilder.on("JoinSpecificChatRoom", (username, message) => {
        console.log('Received message JoinSpecificChatRoom from:' + username + " message: " + message);
        setMessages(messages => [...messages, {username, message}]);
      });

      connBuilder.on("ReceiveSpecificMessage", (username, message) => {
        console.log("Received a new message from username: " + username + " message: " + message);
        setMessages(messages => [...messages, {username, message}]);
      });

      await connBuilder.start()
        .then(() => console.log('Connected to SignalR hub'))
        .catch(err => console.error('Error connecting to hub:', err));  

      await connBuilder.invoke("JoinSpecificChatRoom", {username: user, chatRoom})
        .then(() => {
          console.log('Connected to chat room: ' + chatRoom)
          setChatroom(chatRoom);
        })
        .catch(err => console.error('Error connecting to hub:', err)); 

      setConnection(connBuilder);
    }catch(e){
      console.log(e);
    }
  }

  const sendMessage = async (message) => {
    try {
      await connection.invoke("SendMessage", message);
    } catch (e) {
      console.log(e);
    }
  }

  return (
    <Container>
      <Typography>Welcome to the Financial chat</Typography>
      {!connection 
        ? <WaitingRoom joinChatRoom={joinChatRoom}/>
        : <ChatRoom messages={messages} chatroom={chatroom} sendMessage={sendMessage}/>
      }
    </Container>
  );
}

export default Chat;

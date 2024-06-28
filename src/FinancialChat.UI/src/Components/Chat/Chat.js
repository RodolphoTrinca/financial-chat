import React, {useState} from "react";
import { LogLevel, HubConnectionBuilder } from '@microsoft/signalr';
import { useAuth } from "../../Hooks/Login/useAuth";
import WaitingRoom from "./WaitingRoom";
import ChatRoom from "./ChatRoom";
import {Typography, Container} from '@mui/material';
import BotMessageContainer from "../ChatBot/BotMessageContainer";

function Chat() {
  const { user, token } = useAuth();
  const [messages, setMessages] = useState([]);
  const [botMessages, setBotMessages] = useState([]);
  const [chatroom, setChatroom] = useState();
  const [connection, setConnection] = useState();

  const sendBotMessage = async(stockTicker) => {
    try {
      const response = await fetch("http://localhost:5071/api/Stock?stockTicker="+stockTicker, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          "Authorization": "Bearer " + token
        }
      });

      const res = await response.json();
      if (res.ok) {
        console.log("stock request success")
        return;
      }
      
      throw new Error(res.message);
    } catch (err) {
      console.error(err);
    }
  };

  const joinChatRoom = async (chatRoom) => {
    try{
      const connBuilder = new HubConnectionBuilder()
        .withUrl("http://localhost:5071/api/chatHub", { accessTokenFactory: () => token })
        .configureLogging(LogLevel.Debug)
        .build();

      connBuilder.on("JoinSpecificChatRoom", (username, message, date) => {
        console.log('Received message JoinSpecificChatRoom from:' + username + " message: " + message);
        setMessages(messages => [...messages, {username, message, date}]);
      });

      connBuilder.on("ReceiveSpecificMessage", (username, message, date) => {
        console.log("Received a new message from username: " + username + " message: " + message);
        setMessages(messages => [...messages, {username, message, date}]);
      });

      connBuilder.on("StockBotMessage", (username, message, date) => {
        console.log("Received a new bot stock message: " + username + " message: " + message);
        setBotMessages(botMessages => [...botMessages, {username, message, date}]);
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
      const regex = /^\/.[a-zA-Z]*/g;
      if(!message.match(regex)){
        await connection.invoke("SendMessage", message);
        return;
      }

      var stockTicker = message.replace(regex, "")
        .trim()
        .toUpperCase();

      if(!stockTicker){
        console.error("The stocker ticker cannot be null or empty");
        return;
      }

      sendBotMessage(stockTicker);
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
      {connection && 
      <Container>
          <BotMessageContainer messages={botMessages}/>
      </Container>}
    </Container>
  );
}

export default Chat;

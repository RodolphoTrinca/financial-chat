import { Container, Typography } from "@mui/material";
import MessageContainer from "./MessageContainer";
import SendMessageForm from "./SendMessageForm";

const ChatRoom = ({messages, chatroom, sendMessage}) => {
    return(
        <Container>
            <Typography>ChatRoom {chatroom}</Typography>
            <MessageContainer messages={messages}/>
            <SendMessageForm sendMessage={sendMessage}/>
        </Container>
    );
}

export default ChatRoom;
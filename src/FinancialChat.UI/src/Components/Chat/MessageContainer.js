import { Container, Typography } from "@mui/material";

const MessageContainer = ({messages}) => {
    return(
        <Container>
            {!messages ? <Typography>There are no messages into this chat room</Typography>
                : messages.map((message, index) => {
                    return <Typography id={index} key={index}>{message.message} - {message.username} {message.date && "Sent: " + message.date}</Typography>
                })
            }
        </Container>
    )
}

export default MessageContainer;
import { Container, Typography } from "@mui/material";

const BotMessageContainer = ({messages}) => {
    return(
        <Container>
            { messages.map((message, index) => {
                return <Typography id={"BotMessage-"+index} key={"BotMessage-"+index}>{message.message} - {message.username}</Typography>
                })
            }
        </Container>
    )
}

export default BotMessageContainer;
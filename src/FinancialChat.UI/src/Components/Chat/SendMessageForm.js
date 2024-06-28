import {useState} from "react";
import {Box, TextField, Button} from "@mui/material";

const SendMessageForm = ({sendMessage}) => {
    const [message, setMessage] = useState();

    const onSubmit = (event) => {
        event.preventDefault();
        sendMessage(message);
        setMessage("");
      }

    return(
        <Box component="form" onSubmit={onSubmit} noValidate sx={{ mt: 1 }}>
            <TextField
              margin="normal"
              required
              fullWidth
              id="message"
              label="Message"
              name="message"
              autoFocus
              value={message}
              onChange={e => setMessage(e.target.value)}
              placeholder="Type your message"
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2 }}
            >
              Send
            </Button>
        </Box>
    );
}

export default SendMessageForm;
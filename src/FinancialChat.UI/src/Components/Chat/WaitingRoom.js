import React, {useState} from "react";
import TextField from '@mui/material/TextField';
import {Box, Button} from '@mui/material';

const WaitingRoom = ({joinChatRoom}) => {
    const [chatRoom, setChatRoom] = useState();

    const handleSubmit = (event) => {
        event.preventDefault();
        joinChatRoom(chatRoom);
      };

    return (
        <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 1 }}>
            <TextField
              margin="normal"
              required
              fullWidth
              id="chatRoom"
              label="Chat Room"
              name="chatRoom"
              autoComplete="chatRoom"
              autoFocus
              onChange={e => setChatRoom(e.target.value)}
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2 }}
            >
              Join
            </Button>
        </Box>
    );
}

export default WaitingRoom;
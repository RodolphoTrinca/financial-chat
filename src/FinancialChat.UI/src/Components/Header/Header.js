import React from "react";
import { Box, AppBar, Toolbar, Typography, Grid } from "@mui/material";
import Button from '@mui/material/Button';
import { useAuth } from "../../Hooks/Login/useAuth";
import { Link } from "react-router-dom";

const Header = () => {
  const [open, setOpen] = React.useState(false);
  const { user, token } = useAuth();

  const handleDrawerOpen = () => {
    setOpen(true);
  };

  const handleDrawerClose = () => {
    setOpen(false);
  };

  console.log(user);
  console.log(token);
  return (
    <div>
      <header>
        <Box sx={{ flexGrow: 1 }}>
          <Grid container spacing={2}>
            <AppBar position="static">
              <Toolbar>
                <Grid xs={8}>
                  <Typography variant="h6" noWrap component="div">
                    Financial Chat
                  </Typography>
                </Grid>
                <Grid xs={4}>
                  {!user ? (
                    <Button variant="contained" color="success" component={Link} to="/login">Login</Button>
                  ) : (
                    <Typography variant="h6" noWrap component="div">
                      Welcome {user}
                    </Typography>
                  )}
                </Grid>
              </Toolbar>
            </AppBar>
          </Grid>
        </Box>
      </header>
    </div>
  );
};

export default Header;

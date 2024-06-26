import React from "react";
import { Box, AppBar, Toolbar, Typography, IconButton } from "@mui/material";
import MenuIcon from "@mui/icons-material/Menu";
import Drawer from "../Drawer/Drawer";
import Button from '@mui/material/Button';
import { useAuth } from "../../Hooks/Login/useAuth";
import { Link } from "react-router-dom";

const Header = () => {
  const [open, setOpen] = React.useState(false);
  const { user } = useAuth();

  const handleDrawerOpen = () => {
    setOpen(true);
  };

  const handleDrawerClose = () => {
    setOpen(false);
  };

  return (
    <div>
      <header>
        <Box sx={{ flexGrow: 1 }}>
          <AppBar position="static">
            <Toolbar>
              <IconButton
                color="inherit"
                aria-label="open drawer"
                onClick={handleDrawerOpen}
                edge="start"
                sx={{ mr: 2, ...(open && { display: "none" }) }}
              >
                <MenuIcon />
              </IconButton>
              <Typography variant="h6" noWrap component="div">
                Financial Chat
              </Typography>
              {!user ? (
                <Button color="inherit" component={Link} to="/login">Login</Button>
              ) : (
                <Typography variant="h6" noWrap component="div">
                  You're logged
                </Typography>
              )}
            </Toolbar>
          </AppBar>
          <Drawer open={open} closeDrawer={handleDrawerClose} />
        </Box>
      </header>
    </div>
  );
};

export default Header;

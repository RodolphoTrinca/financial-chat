import React from "react";
import { IconButton, List } from "@mui/material";
import MuiDrawer from "@mui/material/Drawer";
import AssignmentIcon from "@mui/icons-material/Assignment";
import ChevronLeftIcon from "@mui/icons-material/ChevronLeft";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import { styled, useTheme } from "@mui/material/styles";
import Item from "./Item";
import ListItem from "./ListItem";

const drawerWidth = 240;

const DrawerHeader = styled("div")(({ theme }) => ({
  display: "flex",
  alignItems: "center",
  padding: theme.spacing(0, 1),
  // necessary for content to be below app bar
  ...theme.mixins.toolbar,
  justifyContent: "flex-end",
}));

const Drawer = (props) => {
  const theme = useTheme();

  const handleDrawerClose = () => {
    props.closeDrawer(false);
  };

  const expensesItems = [
    { name: "List", icon: <AssignmentIcon />, link: "expenses" },
    { name: "Category", icon: <AssignmentIcon /> },
    { name: "Payment", icon: <AssignmentIcon /> },
    { name: "Type", icon: <AssignmentIcon />, link: "expenses/type"},
  ];

  const incomesItems = [
    { name: "List", icon: <AssignmentIcon />, link: "income" },
    { name: "Type", icon: <AssignmentIcon />, link: "income/type" },
  ];

  return (
    <MuiDrawer
      sx={{
        width: drawerWidth,
        flexShrink: 0,
        "& .MuiDrawer-paper": {
          width: drawerWidth,
          boxSizing: "border-box",
        },
      }}
      variant="persistent"
      anchor="left"
      open={props.open}
    >
      <DrawerHeader>
        <IconButton onClick={handleDrawerClose}>
          {theme.direction === "ltr" ? (
            <ChevronLeftIcon />
          ) : (
            <ChevronRightIcon />
          )}
        </IconButton>
      </DrawerHeader>
      <List>
        <Item
          key="Dashboard"
          name="Dashboard"
          icon={<AssignmentIcon />}
          link="/"
        />
        <ListItem key="Expenses" name="Expenses" items={expensesItems} />
        <ListItem key="Incomes" name="Incomes" items={incomesItems} />
        <Item
          key="Cash basis"
          name="Cash basis"
          icon={<AssignmentIcon />}
          link="cashBasis"
        />
      </List>
    </MuiDrawer>
  );
};

export default Drawer;

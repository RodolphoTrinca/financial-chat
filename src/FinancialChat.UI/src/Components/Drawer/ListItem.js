import React from "react";
import {
  List,
  ListItemIcon,
  ListItemText,
  Collapse,
  ListItemButton,
} from "@mui/material";
import MuiListItem from "@mui/material/ListItem";
import { ExpandMore, ExpandLess } from "@mui/icons-material";
import AssignmentIcon from "@mui/icons-material/Assignment";
import DrawerListItem from "./Item";
import { Link } from "react-router-dom";

const ListItem = (props) => {
  const [openList, setOpenList] = React.useState(false);

  return (
    <>
      <MuiListItem
        button
        key={props.name}
        onClick={() => {
          setOpenList(!openList);
        }}
      >
        <ListItemIcon>
          <AssignmentIcon />
        </ListItemIcon>
        <ListItemText primary={props.name} />
        {openList ? <ExpandLess /> : <ExpandMore />}
      </MuiListItem>
      <Collapse in={openList} timeout="auto" unmountOnExit>
        <List disablePadding>
          {props.items.map((item, index) => {
            return (
              <ListItemButton key={"Button_To_" + props.name + "_" + item.name} component={Link} to={item.link}>
                <DrawerListItem
                  key={props.name + "_" + item.name+ "_" + index}
                  icon={item.icon}
                  name={item.name}
                />
              </ListItemButton>
            );
          })}
        </List>
      </Collapse>
    </>
  );
};

export default ListItem;

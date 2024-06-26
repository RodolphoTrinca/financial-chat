import { ListItemIcon, ListItemText } from "@mui/material";
import MuiListItem from "@mui/material/ListItem";

const Item = (props) => {
  return (
    <MuiListItem key={props.name}>
      <ListItemIcon>{props.icon}</ListItemIcon>
      <ListItemText primary={props.name} />
    </MuiListItem>
  );
};

export default Item;

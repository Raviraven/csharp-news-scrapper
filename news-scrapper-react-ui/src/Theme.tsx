import { createTheme } from "@mui/material";
import { red } from "@mui/material/colors";

export const theme = createTheme({
  palette: {
    secondary: {
      main: "#F45B68",
    },
    error: {
      main: red.A400,
    },
  },
});

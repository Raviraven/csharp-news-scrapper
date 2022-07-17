import { Box, Button, Toolbar } from "@mui/material";
import { Form, Formik } from "formik";
import { TextField } from "../material/TextField";

export const Login = () => {
  return (
    <Box sx={{ display: "flex", justifyContent: "center" }}>
      <Formik
        initialValues={{
          name: "",
          password: "",
        }}
        onSubmit={() => {}}
      >
        <Form>
          <TextField label="Name" name="name" />
          <TextField label="Password" name="password" type="password" />
          <Toolbar
            variant="dense"
            sx={{ display: "flex", justifyContent: "center" }}
          >
            <Button type="button">Cancel</Button>
            <Button type="submit">Login</Button>
          </Toolbar>
        </Form>
      </Formik>
    </Box>
  );
};
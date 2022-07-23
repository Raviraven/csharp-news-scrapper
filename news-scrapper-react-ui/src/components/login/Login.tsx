import { Box, Button, Toolbar, Typography } from "@mui/material";
import { Form, Formik } from "formik";
import { UserLoginSchema, UserLogin } from "../../schemas/user-schema";
import { TextField } from "../material/TextField";
import { useNavigate } from "react-router-dom";
import { useCallback } from "react";
import { login } from "../../api/services/usersService";

export const Login = () => {
  const navigate = useNavigate();

  const handleCancel = useCallback(() => {
    navigate("/");
  }, [navigate]);

  const handleSubmit = useCallback(async (values: UserLogin) => {
    await login(values);
  }, []);

  return (
    <Box
      sx={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        flexDirection: "column",
        pt: "1rem",
      }}
    >
      <Typography variant="h4">Sign in</Typography>
      <Box>
        <Formik<UserLogin>
          initialValues={{
            username: "",
            password: "",
          }}
          // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
          validationSchema={UserLoginSchema}
          onSubmit={handleSubmit}
        >
          <Form>
            <TextField label="Username" name="username" />
            <TextField label="Password" name="password" type="password" />
            <Toolbar
              variant="dense"
              sx={{ display: "flex", justifyContent: "center" }}
            >
              <Button type="button" onClick={handleCancel}>
                Cancel
              </Button>
              <Button type="submit">Login</Button>
            </Toolbar>
          </Form>
        </Formik>
      </Box>
    </Box>
  );
};

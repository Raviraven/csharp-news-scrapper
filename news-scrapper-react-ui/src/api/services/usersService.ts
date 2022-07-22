import axios from "../interceptors/auth";
import { UserLogin } from "../../schemas/user-schema";
import { AuthenticateResponse } from "../models/user";

export const login = async (
  model: UserLogin
): Promise<AuthenticateResponse> => {
  console.log("hello");
  var result = await axios.post("/users/authenticate", model);
  console.log(result);

  localStorage.setItem("authToken", result.data.JwtToken);

  return result.data;
};

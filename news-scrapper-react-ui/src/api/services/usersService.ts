import axios from "../interceptors/auth";
import { UserLogin } from "../../schemas/user-schema";
import { AuthenticateResponse } from "../models/user";

export const login = async (
  model: UserLogin
): Promise<AuthenticateResponse> => {
  const result = await axios.post<AuthenticateResponse>(
    "/users/authenticate",
    model
  );
  localStorage.setItem("authToken", result.data.jwtToken);

  return result.data;
};

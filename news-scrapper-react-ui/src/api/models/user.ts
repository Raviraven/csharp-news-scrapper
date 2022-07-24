export interface UserLoginToDelete {
  username: string;
  password: string;
}

export interface AuthenticateResponse {
  id: number;
  firstName: string;
  lastName: string;
  username: string;
  jwtToken: string;
  expiresInDays: number;
}

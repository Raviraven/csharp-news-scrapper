export interface UserLoginToDelete {
  username: string;
  password: string;
}

export interface AuthenticateResponse {
  id: number;
  FirstName: string;
  LastName: string;
  Username: string;
  JwtToken: string;
  ExpiresInDays: number;
}

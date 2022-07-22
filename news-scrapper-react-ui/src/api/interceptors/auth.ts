import axios from "axios";

axios.interceptors.request.use((request) => {
  const token = localStorage.getItem("authToken");
  if (token !== null) {
    request.headers = { Authorization: `Bearer ${token}` };
  }

  return request;
});

export default axios;

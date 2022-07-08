import axios from "axios";

axios.interceptors.request.use(
  (config) => {},
  (error) => {}
);

export default axios;

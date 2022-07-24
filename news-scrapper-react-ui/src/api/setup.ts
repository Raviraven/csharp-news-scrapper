import axios from "axios";

const getBaseUrl = () => {
  let url;
  switch (process.env.NODE_ENV) {
    case "production":
      url = "";
      break;
    case "development":
    default:
      url = "";
  }

  return url;
};

export default axios.create({
  baseURL: getBaseUrl(),
});

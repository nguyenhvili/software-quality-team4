import axios from "axios";

export const base = axios.create({
  baseURL: `http://localhost:5025/`, //add BE url
});

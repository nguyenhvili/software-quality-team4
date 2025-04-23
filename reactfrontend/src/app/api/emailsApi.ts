import axios from "axios";
import { Email, EmailCreate } from "../types/email";

const axiosInstance = axios.create({
  baseURL: `http://localhost:5025/`, //add BE url
});

async function create(payload: EmailCreate) {
  const resp = await axiosInstance.post<Email>("emails", payload);
  return resp.data;
}

async function deleteEmail(id: number) {
  const resp = await axiosInstance.delete<{}>(`emails/${id}`);
  return resp.data;
}

async function getAll() {
  const resp = await axiosInstance.get<{ emails: Email[] }>("emails");
  return resp.data;
}

const EmailsApi = {
  getAll,
  deleteEmail,
  create,
};

export default EmailsApi;

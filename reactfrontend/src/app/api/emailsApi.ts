import axios from "axios";

const axiosInstance = axios.create({
  baseURL: `http://localhost:5025/`, //add BE url
});

async function create(payload: any) {
  const resp = await axiosInstance.post<any>("emails");
  return resp.data;
}

async function deleteEmail(id: number) {
  const resp = await axiosInstance.delete<{}>(`emails/${id}`);
  return resp.data;
}

async function getAll() {
  const resp = await axiosInstance.get<any>("emails");
  return resp.data;
}

const EmailsApi = {
  getAll,
  deleteEmail,
  create,
};

export default EmailsApi;

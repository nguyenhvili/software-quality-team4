import axios from "axios";
import { ReportAll, ReportSend, ReportType } from "../types/report";

const axiosInstance = axios.create({
  baseURL: `http://localhost:5025/`, //add BE url
});

async function send(payload: ReportSend) {
  const resp = await axiosInstance.post<ReportSend>("reports/send", payload);
  return resp.data;
}

async function getAll() {
  const resp = await axiosInstance.get<{ reports: ReportAll[] }>("reports", {
    params: { page: 1, pageSize: 20 },
  });
  return resp.data;
}

async function getReport(id: string) {
  const resp = await axiosInstance.get<{ report: ReportType }>("reports/" + id, {
  });
  return resp.data;
}

const ReportsApi = {
  getAll,
  send,
  getReport
};

export default ReportsApi;

import { ReportBase, ReportSend, ReportType } from "../types/report";
import { base } from "./base";
import { GetParams } from "../types/common";

async function send(payload: ReportSend) {
  const resp = await base.post<ReportSend>("reports/send", payload);
  return resp.data;
}

async function getAll(params: GetParams) {
  const resp = await base.get<{ reports: ReportBase[] }>("reports", {
    params,
  });
  return resp.data;
}

async function getReport(id: string) {
  const resp = await base.get<ReportType>("reports/" + id);
  return resp.data;
}

const ReportsApi = {
  getAll,
  send,
  getReport,
};

export default ReportsApi;

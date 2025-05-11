import { Email, EmailCreate } from "../types/email";
import { base } from "./base";
import { GetParams } from "../types/common";

async function create(payload: EmailCreate) {
  const resp = await base.post<Email>("emails", payload);
  return resp.data;
}

async function deleteEmail(id: string) {
  const resp = await base.delete<{}>(`emails/${id}`);
  return resp.data;
}

async function getAll(params: GetParams) {
  const resp = await base.get<{ emails: Email[] }>("emails", {
    params,
  });
  return resp.data;
}

const EmailsApi = {
  getAll,
  deleteEmail,
  create,
};

export default EmailsApi;

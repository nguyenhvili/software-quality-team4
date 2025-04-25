import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { AxiosError } from "axios";
import ReportsApi from "../api/reportsApi";
import { ReportSend } from "../types/report";
import toast from "react-hot-toast";

const reportsQueryKeys = {
  all: () => ["reports"] as const,
  detail: (reportId: string) => [...reportsQueryKeys.all(), reportId] as const,
};

export const useReports = () => {
  return useQuery({
    queryKey: reportsQueryKeys.all(),
    queryFn: () => ReportsApi.getAll(),
  });
};

export const useReport = (id: string) => {
  return useQuery({
    queryKey: reportsQueryKeys.detail(id),
    queryFn: () => ReportsApi.getReport(id),
  });
};

export const useReportSend = () => {
  return useMutation({
    mutationFn: async (payload: ReportSend) => {
      return ReportsApi.send(payload);
    },
    onSuccess: () => toast.error("Emails were sent."),
    onError: (err: AxiosError) => {
      toast.error(err.message);
    },
  });
};

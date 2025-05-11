import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { AxiosError } from "axios";
import ReportsApi from "../api/reportsApi";
import { ReportSend } from "../types/report";
import toast from "react-hot-toast";

const PAGE_SIZE = 10;

const reportsQueryKeys = {
  all: () => ["reports"] as const,
  page: (page: number) => [...reportsQueryKeys.all(), page] as const,
  detail: (reportId: string) => [...reportsQueryKeys.all(), reportId] as const,
};

export const useReports = (page: number) => {
  return useQuery({
    queryKey: reportsQueryKeys.page(page),
    queryFn: () => ReportsApi.getAll({ page, pageSize: PAGE_SIZE }),
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
    onSuccess: () => toast.success("Emails were sent."),
    onError: (err: AxiosError) => {
      toast.error(err.message);
    },
  });
};

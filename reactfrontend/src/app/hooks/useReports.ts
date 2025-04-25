import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { AxiosError } from "axios";
import ReportsApi from "../api/reportsApi";
import { ReportSend } from "../types/report";

const reportsQueryKey = ["reports"];

export const useReports = () => {
  return useQuery({
    queryKey: reportsQueryKey,
    queryFn: () => ReportsApi.getAll(),
  });
};

export const useReport = (id: string) => {
  return useQuery({
    queryKey: reportsQueryKey,
    queryFn: () => ReportsApi.getReport(id),
  });
};

export const useReportSend = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (payload: ReportSend) => {
      return ReportsApi.send(payload);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: reportsQueryKey });
    },
    onError: (err: AxiosError) => {
      alert(err.message);
    },
  });
};

import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { AxiosError } from "axios";
import EmailsApi from "../api/emailsApi";
import { EmailCreate } from "../types/email";
import toast from "react-hot-toast";

const PAGE_SIZE = 10;
const emailsQueryKey = ["emails"];

export const useEmails = (page: number) => {
  return useQuery({
    queryKey: ["emails", page],
    queryFn: () => EmailsApi.getAll({ page, pageSize: PAGE_SIZE }),
  });
};

export const useEmailCreate = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (payload: EmailCreate) => {
      return EmailsApi.create(payload);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: emailsQueryKey });
      toast.success("Email was created.");
    },
    onError: (err: AxiosError) => {
      toast.error(err.message);
    },
  });
};

export const useEmailDelete = (id: string) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async () => {
      return EmailsApi.deleteEmail(id);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: emailsQueryKey });
      toast.success("Email was deleted.");
    },
    onError: (err: AxiosError) => {
      toast.error(err.message);
    },
  });
};

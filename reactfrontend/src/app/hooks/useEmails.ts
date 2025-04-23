import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import toast from "react-hot-toast";
import { AxiosError } from "axios";
import EmailsApi from "../api/emailsApi";
import { EmailCreate } from "../types/email";

const emailsQueryKey = ["emails"];

export const useEmails = () => {
  return useQuery({
    queryKey: emailsQueryKey,
    queryFn: () => EmailsApi.getAll(),
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
    },
    onError: (err: AxiosError) => {
      toast.error(err.message);
    },
  });
};

export const useEmailDelete = (id: number) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async () => {
      return EmailsApi.deleteEmail(id);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: emailsQueryKey });
    },
    onError: (err: AxiosError) => {
      toast.error(err.message);
    },
  });
};

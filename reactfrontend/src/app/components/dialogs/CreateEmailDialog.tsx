import { FC, useCallback } from "react";
import { Button, Dialog, DialogPanel, DialogTitle } from "@headlessui/react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEmailCreate } from "../../hooks/useEmails";
import CustomButton from "../CustomButton";
import toast from "react-hot-toast";

type CreateEmailDialogProps = {
  onClose: (value: boolean) => void;
};

const emailSchema = z.object({
  name: z.string().email("Please enter a valid email address"),
});

type EmailFormData = z.infer<typeof emailSchema>;

const CreateEmailDialog: FC<CreateEmailDialogProps> = (props) => {
  const { onClose } = props;

  const { mutateAsync: createEmail } = useEmailCreate();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<EmailFormData>({
    resolver: zodResolver(emailSchema),
  });

  const onSubmit = (data: EmailFormData) => {
    createEmail(data);
    toast.success("Email was created.");
    handleClose();
  };

  const handleClose = useCallback(() => onClose(false), [onClose]);

  return (
    <Dialog
      open={true}
      as="div"
      className="relative z-50"
      onClose={handleClose}
    >
      <div
        className="fixed inset-0 bg-black/50 backdrop-blur-sm"
        aria-hidden="true"
      />
      <div className="fixed inset-0 flex items-center justify-center p-4">
        <DialogPanel className="w-full max-w-sm rounded-2xl bg-white p-6 shadow-xl transition-all">
          <DialogTitle className="text-lg font-semibold text-gray-900">
            Add a new email
          </DialogTitle>

          <form onSubmit={handleSubmit(onSubmit)} className="mt-4">
            <label htmlFor="email" className="block text-sm text-gray-700 mb-1">
              Email address
            </label>
            <input
              id="email"
              {...register("name")}
              className="w-full rounded-md border border-gray-300 px-3 py-2 text-sm focus:border-gray-700 focus:ring-gray-700 focus:outline-none"
            />
            {errors.name && (
              <p className="mt-1 text-sm text-red-600">{errors.name.message}</p>
            )}

            <div className="mt-6 flex justify-end gap-3">
              <CustomButton variant="secondary" onClick={handleClose}>
                Cancel
              </CustomButton>
              <CustomButton variant="primary" type={"submit"}>
                Create
              </CustomButton>
            </div>
          </form>
        </DialogPanel>
      </div>
    </Dialog>
  );
};

export default CreateEmailDialog;

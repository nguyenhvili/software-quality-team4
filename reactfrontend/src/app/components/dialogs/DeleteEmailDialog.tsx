import { Button, Dialog, DialogPanel, DialogTitle } from "@headlessui/react";
import { FC } from "react";
import { useEmailDelete } from "../../hooks/useEmails";
import CustomButton from "../CustomButton";
import toast from "react-hot-toast";

type DeleteEmailDialogProps = {
  emailId: string;
  onClose: (value: null) => void;
};

const DeleteEmailDialog: FC<DeleteEmailDialogProps> = (props) => {
  const { emailId, onClose } = props;

  const { mutateAsync: deleteEmail } = useEmailDelete(emailId);

  const handleDelete = () => {
    deleteEmail();
    toast.error("Deleted");
    handleClose();
  };

  const handleClose = () => {
    onClose(null);
  };

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
            Delete this email?
          </DialogTitle>
          <p className="mt-2 text-sm text-gray-600">
            Are you sure you want to delete this email? This action cannot be
            undone.
          </p>
          <div className="mt-6 flex justify-end gap-3">
            <CustomButton variant="secondary" onClick={handleClose}>
              Cancel
            </CustomButton>

            <CustomButton variant="danger" onClick={handleDelete}>
              Delete
            </CustomButton>
          </div>
        </DialogPanel>
      </div>
    </Dialog>
  );
};

export default DeleteEmailDialog;

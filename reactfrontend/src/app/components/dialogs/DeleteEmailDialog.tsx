import { Button, Dialog, DialogPanel, DialogTitle } from "@headlessui/react";
import { FC } from "react";

type DeleteEmailDialogProps = {
  emailId: number | null;
  onClose: (value: null) => void;
};

const DeleteEmailDialog: FC<DeleteEmailDialogProps> = (props) => {
  const { emailId, onClose } = props;
  const handleDelete = () => {
    // delete email
    console.log(`Deleting email with ID: ${emailId}`);
    handleClose();
  };

  const handleClose = () => {
    onClose(null);
  };

  console.log("delete dialog");

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
            <Button
              className="rounded-md px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-400"
              onClick={handleClose}
            >
              Cancel
            </Button>
            <Button
              className="rounded-md bg-red-600 px-4 py-2 text-sm font-medium text-white hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500"
              onClick={handleDelete}
            >
              Delete
            </Button>
          </div>
        </DialogPanel>
      </div>
    </Dialog>
  );
};

export default DeleteEmailDialog;

import { Button, Dialog, DialogPanel, DialogTitle } from "@headlessui/react";
import { FC } from "react";
import { useReport } from "../../hooks/useReports";
import { formatDate } from "../../utils/formatDate";
import CustomButton from "../CustomButton";

type ReportDetailDialogProps = {
  reportId: string;
  onClose: (value: null) => void;
};

const ReportDetailDialog: FC<ReportDetailDialogProps> = (props) => {
  const { reportId, onClose } = props;

  const { data: report } = useReport(reportId);

  const handleClose = () => {
    onClose(null);
  };

  if (!report) return;

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
            Report detail
          </DialogTitle>
          <p className="mt-2 text-sm text-gray-600">
            Created on: {formatDate(report.createdAt)}
          </p>
          <div className="mt-6 flex justify-end gap-3">
            <CustomButton variant="primary" onClick={handleClose}>
              Ok
            </CustomButton>
          </div>
        </DialogPanel>
      </div>
    </Dialog>
  );
};

export default ReportDetailDialog;

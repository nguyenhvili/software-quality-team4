import { Dialog, DialogPanel, DialogTitle } from "@headlessui/react";
import { createColumnHelper } from "@tanstack/react-table";
import { FC, useCallback, useMemo, useState } from "react";
import { ReportAll, ReportSend } from "src/app/types/report";
import { useEmails } from "../../hooks/useEmails";
import { useReportSend } from "../../hooks/useReports";
import { Email } from "src/app/types/email";
import Table from "../Table";
import { formatDate } from "../../utils/formatDate";
import CustomButton from "../CustomButton";
import toast from "react-hot-toast";

type SendEmailDialogProps = {
  report: ReportAll;
  onClose: (value: null) => void;
};

const SendEmailDialog: FC<SendEmailDialogProps> = (props) => {
  const { report, onClose } = props;

  const { data: emails, isLoading } = useEmails();

  const [checkedEmails, setCheckedEmails] = useState<Set<string>>(new Set());
  const { mutateAsync: sendEmail } = useReportSend();

  const handleClose = () => {
    onClose(null);
  };

  const handleSend = () => {
    const reportSend: ReportSend = {
      reportId: report.id,
      emailIds: Array.from(checkedEmails),
    };
    sendEmail(reportSend);
    toast.success("Emails were sent.");
    handleClose();
  };

  const checkEmail = useCallback(
    (email: string) => {
      setCheckedEmails((prev) => {
        const updated = new Set(prev);
        if (updated.has(email)) {
          updated.delete(email);
        } else {
          updated.add(email);
        }
        return updated;
      });
    },
    [setCheckedEmails]
  );

  const columnHelper = createColumnHelper<Email>();

  const cols = useMemo(
    () => [
      columnHelper.display({
        id: "input",
        header: "",
        cell: ({ row }) => (
          <input
            type="checkbox"
            checked={checkedEmails.has(row.original.id)}
            onChange={() => checkEmail(row.original.id)}
          />
        ),
      }),
      columnHelper.accessor("emailValue", {
        header: "Emails",
        cell: ({ renderValue }) => <span>{renderValue()}</span>,
      }),
    ],
    [checkedEmails, checkEmail]
  );

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
            {formatDate(report.createdAt)}
          </DialogTitle>
          <Table
            cols={cols}
            data={emails?.emails || []}
            noContentMessage="No emails found."
            isLoading={isLoading}
          />
          <div className="mt-6 flex justify-end gap-3">
            <CustomButton variant="secondary" onClick={handleClose}>
              Cancel
            </CustomButton>
            <CustomButton variant="primary" onClick={handleSend}>
              Send
            </CustomButton>
          </div>
        </DialogPanel>
      </div>
    </Dialog>
  );
};
export default SendEmailDialog;

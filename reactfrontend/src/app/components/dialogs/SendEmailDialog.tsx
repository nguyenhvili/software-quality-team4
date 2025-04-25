import { Button, Dialog, DialogPanel, DialogTitle } from "@headlessui/react";
import { ColumnDef, RowExpanding } from "@tanstack/react-table";
import { FC, useMemo, useState } from "react";
import { ReportSend, ReportType } from "src/app/types/report";
import SendEmailTable from "../tables/SendEmailTable";
import { useEmails } from "../../hooks/useEmails";
import { useReportSend, useReport } from "../../hooks/useReports";


type SendEmailDialogProps = {
    reportId: string;
    onClose: (value: null) => void;
};

const SendEmailDialog: FC<SendEmailDialogProps> = (props) => {
    const { reportId, onClose } = props;

    const { data: report } = useReport(reportId);

    const [checkedEmails, setCheckedEmails] = useState<Set<string>>(new Set());
    const { mutateAsync: sendEmail } = useReportSend();

    const { data } = useEmails();
    const emails = data?.emails?.map(item => item.emailValue) ?? [];

    const handleClose = () => {
        onClose(null);
    };

    const handleSend = () => {
        const reportSend: ReportSend = {
            filePath: report?.report?.filePath ?? "",
            emails: Array.from(checkedEmails)
        };
        sendEmail(reportSend);
        onClose(null);
    };

    const formatDate = () => {
        const value = report?.report?.createdAt ?? "";
        return value;
        // const safeString = value.includes(".")
        //     ? value.split(".")[0] + "Z"
        //     : value;
        // const date = new Date(safeString);
        // return date.toLocaleString("en-US")
    };

    const checkEmail = (email: string) => {
        setCheckedEmails(prev => {
            const updated = new Set(prev);
            if (updated.has(email)) {
                updated.delete(email);
            } else {
                updated.add(email);
            }
            return updated;
        });
    };

    const cols = useMemo<ColumnDef<string>[]>(() => [
        {
            id: "select",
            header: "",
            cell: ({ row }) => (
                <input
                    type="checkbox"
                    checked={checkedEmails.has(row.original)}
                    onChange={() => checkEmail(row.original)}
                />
            ),
        },
        {
            id: "email",
            header: "Email Address",
            cell: ({ row }) => <span>{row.original}</span>,
        },
    ], [checkedEmails]);


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
                        {formatDate()}
                    </DialogTitle>
                    <SendEmailTable cols={cols} emails={emails} />
                    <div className="mt-6 flex justify-end gap-3">
                        <Button
                            className="rounded-md px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-400"
                            onClick={handleClose}
                        >
                            Cancel
                        </Button>
                        <Button
                            className="rounded-md bg-red-600 px-4 py-2 text-sm font-medium text-white hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500"
                            onClick={handleSend}
                        >
                            Send
                        </Button>
                    </div>
                </DialogPanel>
            </div>
        </Dialog>
    );
}
export default SendEmailDialog;

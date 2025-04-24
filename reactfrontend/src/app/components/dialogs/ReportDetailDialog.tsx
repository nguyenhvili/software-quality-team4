import { Button, Dialog, DialogPanel, DialogTitle } from "@headlessui/react";
import { FC } from "react";
import { ReportType } from "src/app/types/report";

type ReportDetailDialogProps = {
    report: ReportType;
    onClose: (value: null) => void;
};

const ReportDetailDialog: FC<ReportDetailDialogProps> = (props) => {
    const { report, onClose } = props;

    const handleClose = () => {
        onClose(null);
    };

    const formatDate = () => {
        const value = report.createdAt;
        const safeString = value.includes(".")
            ? value.split(".")[0] + "Z"
            : value;
        const date = new Date(safeString);
        return date.toLocaleString("en-US")
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
                        {formatDate()}

                    </DialogTitle>
                    <div className="mt-6 flex justify-end gap-3">
                        <Button
                            className="rounded-md px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-400"
                            onClick={handleClose}
                        >
                            Cancel
                        </Button>
                    </div>
                </DialogPanel>
            </div>
        </Dialog>
    );
};

export default ReportDetailDialog;

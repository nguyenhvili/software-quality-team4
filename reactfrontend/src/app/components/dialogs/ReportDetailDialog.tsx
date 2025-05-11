import { Dialog, DialogPanel, DialogTitle } from "@headlessui/react";
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
    <Dialog open={true} as="div" className="relative z-50" onClose={handleClose}>
      <div className="fixed inset-0 bg-black/50 backdrop-blur-sm" aria-hidden="true" />
      <div className="fixed inset-0 flex items-center justify-center p-4 overflow-y-auto">
        <DialogPanel className="w-full max-w-5xl rounded-2xl bg-white p-6 shadow-xl transition-all">
          <DialogTitle className="text-xl font-semibold text-gray-900 mb-4">
            Report Details
          </DialogTitle>

          <div className="space-y-2">
            <p><strong>Created on:</strong> {formatDate(report.createdAt)}</p>
          </div>

          <h3 className="mt-6 mb-3 font-semibold text-gray-800">Holdings:</h3>

          <div className="max-h-96 overflow-y-auto border rounded-md shadow-inner">
            <table className="w-full table-auto text-left text-sm text-gray-700 border-collapse">
              <thead className="sticky top-0 bg-gray-100 z-10">
                <tr>
                  <th className="px-3 py-2 border-b">Company Name</th>
                  <th className="px-3 py-2 border-b">Ticker</th>
                  <th className="px-3 py-2 border-b text-right">Shares</th>
                  <th className="px-3 py-2 border-b text-right">Shares %</th>
                  <th className="px-3 py-2 border-b text-right">Weight</th>
                </tr>
              </thead>
              <tbody>
                {report.holdings.map((holding) => (
                  <tr key={holding.id} className="border-b hover:bg-gray-50">
                    <td className="px-3 py-2">{holding.companyName}</td>
                    <td className="px-3 py-2">{holding.ticker}</td>
                    <td className="px-3 py-2 text-right">{holding.shares.toLocaleString()}</td>
                    <td className="px-3 py-2 text-right">{holding.sharesPercent.toFixed(2)}%</td>
                    <td className="px-3 py-2 text-right">{holding.weight.toFixed(2)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          <div className="mt-6 flex justify-end">
            <CustomButton variant="primary" onClick={handleClose}>
              Close
            </CustomButton>
          </div>
        </DialogPanel>
      </div>
    </Dialog>
  );
};

export default ReportDetailDialog;

import { FC, useMemo, useState } from "react";
import AppLayout from "../layouts/AppLayout";
import { createColumnHelper } from "@tanstack/react-table";
import { IconDetail } from "../../assets/IconDetail";
import { IconEmail } from "../../assets/IconEmail";
import { useReports } from "../hooks/useReports";
import ReportDetailDialog from "../components/dialogs/ReportDetailDialog";
import { ReportBase } from "../types/report";
import SendEmailDialog from "../components/dialogs/SendEmailDialog";
import Table from "../components/Table";
import Pagination from "../components/Pagination";

type ReportsPageProps = {};

const ReportsPage: FC<ReportsPageProps> = (props) => {
  const [isDetailDialogOpen, setIsDetailDialogOpen] = useState<string | null>(
    null
  );
  const [isSendDialogOpen, setIsSendDialogOpen] = useState<ReportBase | null>(
    null
  );
  const [page, setPage] = useState(1);

  const { data, isLoading } = useReports(page);
  const hasNext = useMemo(() => !data || data.reports.length < 10, [data]);

  const columnHelper = createColumnHelper<ReportBase>();

  const cols = useMemo(
    () => [
      columnHelper.accessor("createdAt", {
        header: "Created at",
        cell: ({ getValue }) => {
          const raw = getValue();
          const safeString = raw.includes(".") ? raw.split(".")[0] + "Z" : raw;
          const date = new Date(safeString);
          return <div>{date.toLocaleString("en-US")}</div>;
        },
      }),
      columnHelper.display({
        id: "actions",
        header: "Actions",
        cell: ({ row }) => (
          <div className="flex gap-2">
            <button
              className="p-1 rounded bg-blue-500 transition hover:bg-blue-600"
              onClick={() => setIsDetailDialogOpen(row.original.id)}
              title="View details"
            >
              <IconDetail className="w-5 h-5 text-white" />
            </button>
            <button
              className="p-1 rounded bg-gray-500 transition hover:bg-gray-600"
              onClick={() => setIsSendDialogOpen(row.original)}
              title="Send by email"
            >
              <IconEmail className="w-5 h-5 text-white" />
            </button>
          </div>
        ),
      }),
    ],
    [setIsDetailDialogOpen, setIsSendDialogOpen]
  );

  return (
    <AppLayout title="Reports">
      <div className="mb-6">
        <div className="flex items-center justify-between mb-4 gap-8">
          <p className="text-gray-600">
            Here are all generated reports — you can select a report and send an
            email.
          </p>
        </div>
        <Table
          cols={cols}
          data={data?.reports || []}
          noContentMessage="No reports found."
          isLoading={isLoading}
        />
        <Pagination page={page} setPage={setPage} hasNext={hasNext} />
      </div>
      {isDetailDialogOpen && (
        <ReportDetailDialog
          reportId={isDetailDialogOpen}
          onClose={setIsDetailDialogOpen}
        />
      )}
      {isSendDialogOpen && (
        <SendEmailDialog
          report={isSendDialogOpen}
          onClose={setIsSendDialogOpen}
        />
      )}
    </AppLayout>
  );
};

export default ReportsPage;

import { FC, useMemo, useState } from "react";
import AppLayout from "../layouts/AppLayout";
import { createColumnHelper } from "@tanstack/react-table";
import { Email } from "../types/email";
import { IconDelete } from "../../assets/IconDelete";
import { Button } from "@headlessui/react";
import DeleteEmailDialog from "../components/dialogs/DeleteEmailDialog";
import CreateEmailDialog from "../components/dialogs/CreateEmailDialog";
import { useEmails } from "../hooks/useEmails";
import Table from "../components/Table";
type EmailsPageProps = {};

const EmailsPage: FC<EmailsPageProps> = (props) => {
  const {} = props;

  const [isOpenDeleteDialog, setIsOpenDeleteDialog] = useState<null | string>(
    null
  );
  const [isOpenCreateDialog, setIsOpenCreateDialog] = useState<boolean>(false);

  const { data } = useEmails();

  const columnHelper = createColumnHelper<Email>();

  const cols = useMemo(
    () => [
      columnHelper.accessor("emailValue", {
        cell: ({ renderValue }) => <div>{renderValue()}</div>,
      }),
      columnHelper.display({
        id: "actions",
        header: "Actions",
        cell: ({ row }) => (
          <button
            className="p-1 rounded bg-red-500 transition"
            onClick={() => setIsOpenDeleteDialog(row.original.id)}
          >
            <IconDelete className="w-5 h-5 text-white" />
          </button>
        ),
      }),
    ],
    [setIsOpenDeleteDialog]
  );

  return (
    <AppLayout title="Emails">
      <div className="mb-6">
        <div className="flex items-center justify-between mb-4 gap-8">
          <p className="text-gray-600">
            Here are all the emails you have stored — you can view, add, or
            manage them from this page.
          </p>
          <Button
            className="bg-gray-700 text-white px-4 py-2 rounded-lg hover:bg-gray-800 transition"
            onClick={() => setIsOpenCreateDialog(true)}
          >
            Add new
          </Button>
        </div>
        <Table
          cols={cols}
          data={data?.emails || []}
          noContentMessage="No emails found."
        />
      </div>
      {isOpenDeleteDialog && (
        <DeleteEmailDialog
          emailId={isOpenDeleteDialog}
          onClose={setIsOpenDeleteDialog}
        />
      )}
      {isOpenCreateDialog && (
        <CreateEmailDialog onClose={setIsOpenCreateDialog} />
      )}
    </AppLayout>
  );
};

export default EmailsPage;

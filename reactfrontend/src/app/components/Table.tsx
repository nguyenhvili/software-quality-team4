import {
  ColumnDef,
  flexRender,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";

type TableProps<T> = {
  cols: ColumnDef<T, any>[];
  data: T[];
  noContentMessage?: string;
  isLoading?: boolean;
};

function Table<T>(props: TableProps<T>) {
  const { cols, data, noContentMessage, isLoading } = props;

  const table = useReactTable({
    columns: cols,
    data: data,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <div className="bg-white border border-gray-200 rounded-xl shadow overflow-hidden">
      <table className="w-full text-left">
        <thead className="bg-gray-50">
          {table.getHeaderGroups().map((headerGroup) => (
            <tr key={headerGroup.id}>
              {headerGroup.headers.map((header) => (
                <th
                  key={header.id}
                  className="text-gray-700 text-sm font-semibold px-6 py-4"
                >
                  {header.isPlaceholder
                    ? null
                    : flexRender(
                        header.column.columnDef.header,
                        header.getContext()
                      )}
                </th>
              ))}
            </tr>
          ))}
        </thead>
        <tbody>
          {table.getRowModel().rows.length !== 0 &&
            table.getRowModel().rows.map((row) => (
              <tr
                key={row.id}
                className="border-t border-gray-200 hover:bg-gray-50"
              >
                {row.getVisibleCells().map((cell) => (
                  <td key={cell.id} className="px-6 py-4 text-sm text-gray-600">
                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                  </td>
                ))}
              </tr>
            ))}
          {table.getRowModel().rows.length === 0 && isLoading && (
            <tr>
              <td
                colSpan={cols.length}
                className="text-center py-6 text-gray-500"
              >
                {noContentMessage}
              </td>
            </tr>
          )}
          {table.getRowModel().rows.length === 0 && !isLoading && (
            <tr>
              <td
                colSpan={cols.length}
                className="text-center py-6 text-gray-500"
              >
                {noContentMessage}
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}

export default Table;

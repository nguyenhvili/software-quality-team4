import {
    useReactTable,
    getCoreRowModel,
    flexRender,
    ColumnDef,
} from "@tanstack/react-table";
import { FC } from "react";

type SendEmailTableProps = {
    cols: ColumnDef<string, any>[];
    emails: string[]
};


const SendEmailTable: FC<SendEmailTableProps> = ({ cols, emails }) => {

    const table = useReactTable({
        data: emails,
        columns: cols,
        getCoreRowModel: getCoreRowModel(),
    });

    return (
        <div className="bg-white border border-gray-200 rounded-xl shadow overflow-hidden mt-4">
            <table className="w-full text-left">
                <thead className="bg-gray-50">
                    {table.getHeaderGroups().map(headerGroup => (
                        <tr key={headerGroup.id}>
                            {headerGroup.headers.map(header => (
                                <th
                                    key={header.id}
                                    className="text-gray-700 text-sm font-semibold px-6 py-4"
                                >
                                    {header.isPlaceholder
                                        ? null
                                        : flexRender(header.column.columnDef.header, header.getContext())}
                                </th>
                            ))}
                        </tr>
                    ))}
                </thead>
                <tbody>
                    {table.getRowModel().rows.length ? (
                        table.getRowModel().rows.map(row => (
                            <tr
                                key={row.id}
                                className="border-t border-gray-200 hover:bg-gray-50"
                            >
                                {row.getVisibleCells().map(cell => (
                                    <td key={cell.id} className="px-6 py-4 text-sm text-gray-600">
                                        {flexRender(cell.column.columnDef.cell, cell.getContext())}
                                    </td>
                                ))}
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan={cols.length} className="text-center py-6 text-gray-500">
                                No emails found.
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
};

export default SendEmailTable;

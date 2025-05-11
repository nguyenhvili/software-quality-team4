import { FC } from "react";
import { IconArrowLeft } from "../../assets/IconArrowLeft";
import { IconArrowRight } from "../../assets/IconArrowRight";

type PaginationProps = {
  page: number;
  setPage: (page: number) => void;
  hasNext: boolean;
};

const Pagination: FC<PaginationProps> = (props) => {
  const { page, setPage, hasNext } = props;

  return (
    <div className="flex flex-row w-full justify-center gap-4 pt-4 items-center">
      <button
        className="p-1 rounded bg-gray-500 transition flex flex-row gap-2 items-center text-white px-2 justify-center disabled:opacity-40"
        disabled={page === 1}
        onClick={() => setPage(page - 1)}
      >
        <IconArrowLeft className="w-5 h-5 text-white" />
        Previous
      </button>
      <p>{page}</p>
      <button
        className="p-1 rounded bg-gray-500 transition flex flex-row gap-2 items-center text-white px-2 w-26 justify-center disabled:opacity-40"
        onClick={() => setPage(page + 1)}
        disabled={hasNext}
      >
        Next
        <IconArrowRight className="w-5 h-5 text-white" />
      </button>
    </div>
  );
};

export default Pagination;

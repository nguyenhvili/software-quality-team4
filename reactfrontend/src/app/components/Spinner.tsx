import { FC } from "react";

type SpinnerProps = {
  size?: number;
  colorClass?: string;
};

const Spinner: FC<SpinnerProps> = (props) => {
  const { size, colorClass } = props;

  return (
    <div
      className={`animate-spin rounded-full border-4 border-t-transparent ${colorClass}`}
      style={{ width: size, height: size }}
    />
  );
};

export default Spinner;

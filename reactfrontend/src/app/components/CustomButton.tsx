import { clsx } from "clsx";
import { ButtonHTMLAttributes, FC, ReactNode } from "react";

type Variant = "primary" | "secondary" | "danger";

interface CustomButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: Variant;
  children: ReactNode;
}

const baseClasses =
  "rounded-md px-4 py-2 text-sm font-medium focus:outline-none focus:ring-2 transition";

const variantClasses: Record<Variant, string> = {
  primary: "bg-gray-700 text-white hover:bg-gray-800 focus:ring-gray-900",
  secondary: "text-gray-700 bg-gray-100 hover:bg-gray-200 focus:ring-gray-400",
  danger: "bg-red-600 text-white hover:bg-red-700 focus:ring-red-500",
};

const CustomButton: FC<CustomButtonProps> = ({
  variant = "primary",
  children,
  className,
  ...props
}) => {
  return (
    <button
      className={clsx(baseClasses, variantClasses[variant], className)}
      {...props}
    >
      {children}
    </button>
  );
};

export default CustomButton;

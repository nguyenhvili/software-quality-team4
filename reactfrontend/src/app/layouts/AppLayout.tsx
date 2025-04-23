import { FC, ReactNode } from "react";
import Navbar from "../components/Navbar";

type AppLayoutProps = {
  children: ReactNode;
  title: string;
};

const AppLayout: FC<AppLayoutProps> = (props) => {
  const { children, title } = props;

  return (
    <div className="min-h-screen flex bg-gray-100">
      <Navbar />
      <main className="flex-1 p-8">
        <h1 className="text-3xl font-semibold text-gray-800 mb-6">{title}</h1>
        <div className="bg-white p-6 rounded-2xl shadow-md border border-gray-200">
          {children}
        </div>
      </main>
    </div>
  );
};

export default AppLayout;

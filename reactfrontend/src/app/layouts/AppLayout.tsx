import { FC, ReactNode } from "react";
import Navbar from "../components/Navbar";

type AppLayoutProps = {
  children: ReactNode;
};

const AppLayout: FC<AppLayoutProps> = (props) => {
  const { children } = props;

  return (
    <div className="w-full min-h-full h-screen grid grid-cols-[8rem_1fr] p-8">
      <Navbar />
      <section>{children}</section>
    </div>
  );
};

export default AppLayout;

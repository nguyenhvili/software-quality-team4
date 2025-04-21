import { FC, ReactNode } from "react";

type AppLayoutProps = {
  children: ReactNode;
};

const AppLayout: FC<AppLayoutProps> = (props) => {
  const { children } = props;

  return (
    <div>
      <nav>hahahha</nav>
      <section>{children}</section>
    </div>
  );
};

export default AppLayout;

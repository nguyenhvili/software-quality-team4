import { FC } from "react";
import AppLayout from "../layouts/AppLayout";
import { useEmails } from "../hooks/useEmails";

type EmailsPageProps = {};

const EmailsPage: FC<EmailsPageProps> = (props) => {
  const {} = props;

  // const { data: emails } = useEmails();

  const emails = [
    "example@gmail.com",
    "example@gmail.com",
    "example@gmail.com",
    "example@gmail.com",
    "example@gmail.com",
    "example@gmail.com",
    "example@gmail.com",
    "example@gmail.com",
  ];

  return (
    <AppLayout>
      <div>emails page</div>
    </AppLayout>
  );
};

export default EmailsPage;

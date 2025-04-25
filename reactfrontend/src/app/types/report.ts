export type ReportType = {
  id: string;
  filePath: string;
  createdAt: string
};

export type ReportAll = {
  id: string;
  createdAt: string
};

export type ReportSend = {
  filePath: string;
  emails: string[]
};

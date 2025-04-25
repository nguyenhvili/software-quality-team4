export type ReportType = {
  id: string;
  filePath: string;
  createdAt: string;
  // add rest
};

export type ReportAll = {
  id: string;
  createdAt: string;
};

export type ReportSend = {
  filePath: string;
  emailIds: string[];
};

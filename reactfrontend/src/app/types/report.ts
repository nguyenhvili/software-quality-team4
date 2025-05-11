export type ReportBase = {
  id: string;
  createdAt: string;
};

export type ReportType = ReportBase & {
  filePath: string;
  holdings: Holding[];
};

export type Holding = {
  id: string;
  companyName: string;
  ticker: string;
  shares: number;
  sharesPercent: number;
  weight: number;
};

export type ReportSend = {
  reportId: string;
  emailIds: string[];
};

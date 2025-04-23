export type Email = {
  id: number;
  value: string;
};

export type EmailCreate = Omit<Email, "id">;

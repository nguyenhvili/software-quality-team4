export type Email = {
  id: number;
  name: string;
};

export type EmailCreate = Omit<Email, "id">;

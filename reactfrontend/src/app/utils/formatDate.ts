export const formatDate = (value: string) => {
  const safeString = value.includes(".") ? value.split(".")[0] + "Z" : value;
  const date = new Date(safeString);
  return date.toLocaleString("en-US");
};

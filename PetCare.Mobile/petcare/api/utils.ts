import { parseISO } from "date-fns";

const ISODateFormat = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d*)?(?:[-+]\d{2}:?\d{2}|Z)?$/;

const isIsoDateString = (value: unknown): value is string => {
  return typeof value === "string" && ISODateFormat.test(value);
};

export const handleDates = (data: unknown) => {
  if (isIsoDateString(data)) return parseISO(data);
  if (data === null || data === undefined || typeof data !== "object") return data;

  for (const [key, val] of Object.entries(data)) {
    if (isIsoDateString(val)) {
      // @ts-expect-error this is a hack to make the type checker happy
      data[key] = parseISO(val);
    }
    else if (typeof val === "object") handleDates(val);
  }

  return data
};
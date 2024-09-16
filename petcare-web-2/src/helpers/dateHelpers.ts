import { differenceInYears, differenceInMonths, differenceInWeeks } from 'date-fns';

export const calculateAge = (dob: Date): string => {
  const today = new Date();
  const years = differenceInYears(today, dob);

  if (years >= 1) {
    return `${years} year${years > 1 ? 's' : ''}`;
  }

  const months = differenceInMonths(today, dob);
  if (months >= 1) {
    return `${months} month${months > 1 ? 's' : ''}`;
  }

  const weeks = differenceInWeeks(today, dob);
  return `${weeks} week${weeks > 1 ? 's' : ''}`;
};

import type { PetType } from '../petType';
import type { Sex } from '../sex';

export interface PetDTO {
  id: string;
  name: string;
  dateOfBirth: Date;
  petType: PetType;
  sex: Sex;
}
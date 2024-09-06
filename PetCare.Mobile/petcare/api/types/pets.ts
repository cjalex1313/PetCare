export enum PetType {
  Unknown = 0,
  Dog = 1,
  Cat = 2
}

export interface PetDTO {
  id: string;
  name: string;
  dateOfBirth: Date;
  petType: PetType
}
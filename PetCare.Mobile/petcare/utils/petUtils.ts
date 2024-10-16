import { PetType, Sex } from "@/api/types/pets"
;
export const displayPetType = (petType: PetType): string => {
  if (petType == PetType.Cat) {
    return "cat";
  }
  if (petType == PetType.Dog) {
    return "dog";
  }
  return "unknown";
}

export const displayPetSex = (petSex: Sex): string => {
  if (petSex == Sex.Male) {
    return "male";
  }
  if (petSex == Sex.Female) {
    return "female";
  }
  return "unknown";
}
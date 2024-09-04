import type { PetType } from './petType'

export interface PetDTO {
  id: string
  name: string
  dateOfBirth: Date
  petType: PetType
}

export interface VaccineDTO {
  id: string;
  petId: string;
  name: string;
  administrationDate: Date;
  nextDueDate: Date;
  notes?: string;
}
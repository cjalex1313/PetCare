export interface VaccineDTO {
  id: string;
  petId: string;
  name: string;
  administrationDate: Date;
  notes?: string;
}

export interface UpcomingVaccineDTO {
  id: string;
  petId: string;
  name: string;
  date: Date;
  notes?: string;
}

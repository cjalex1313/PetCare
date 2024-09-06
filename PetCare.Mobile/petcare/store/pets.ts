import petApi from "@/api/petApi";
import { PetDTO } from "@/api/types/pets";
import { createSlice, PayloadAction } from "@reduxjs/toolkit";


export const petsSlice = createSlice({
  name: 'pet',
  initialState: {
    pets: [] as PetDTO[],
  },
  reducers: {
    setPets: (state, action: PayloadAction<PetDTO[]>) => {
      state.pets = [...action.payload]
    }
  }
});

export const { setPets } = petsSlice.actions

export default petsSlice.reducer
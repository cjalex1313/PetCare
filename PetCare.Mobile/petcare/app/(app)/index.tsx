import React, { useEffect } from 'react';
import petApi from '@/api/petApi';
import { useDispatch, useSelector } from 'react-redux';
import { setPets } from '@/store/pets'
import { IRootState } from '@/store/store';
import { PetDTO } from '@/api/types/pets';
import { View, Text } from 'react-native-ui-lib';

export default function HomeScreen() {
  const dispatch = useDispatch();

  const pets = useSelector<IRootState, PetDTO[]>((state) => state.pets.pets);

  useEffect(() => {
    loadPets()
  }, []);

  const loadPets = async () => {
    const pets = await petApi.getPets();
    dispatch(setPets(pets));

  }


  return (
    <View padding-20>
      {pets.map(pet => <Text key={pet.id}>{pet.name}</Text>)}
    </View >
  );
}
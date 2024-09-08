import React, { useEffect } from 'react';
import petApi from '@/api/petApi';
import { useDispatch, useSelector } from 'react-redux';
import { setPets } from '@/store/pets'
import { IRootState } from '@/store/store';
import { PetDTO, PetType } from '@/api/types/pets';
import { FlatList, StyleSheet, View, Text } from 'react-native';
import FontAwesome5 from '@expo/vector-icons/FontAwesome5';
import MaterialIcons from '@expo/vector-icons/build/MaterialIcons';
import AntDesign from '@expo/vector-icons/build/AntDesign';
import { Appbar, Button, IconButton, List } from 'react-native-paper';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { Stack, useRouter } from 'expo-router';

export default function HomeScreen() {
  const dispatch = useDispatch();

  const router = useRouter();

  const logOut = async () => {
    console.log('log out')
    const token = await AsyncStorage.getItem('JWT');
    if (token) {
      await AsyncStorage.clear()
    }
    router.replace('/login')
  }

  const pets = useSelector<IRootState, PetDTO[]>((state) => state.pets.pets);

  useEffect(() => {
    loadPets()
  }, []);

  const loadPets = async () => {
    const pets = await petApi.getPets();
    dispatch(setPets(pets));
  }

  const petClicked = (pet: PetDTO) => {
    console.log(pet.name);
  }

  const renderPetIcon = (petType: PetType) => {
    if (petType == PetType.Cat) {
      return <FontAwesome5 name="cat" size={24} color="black" />
    }
    if (petType == PetType.Dog) {
      return <FontAwesome5 name="dog" size={24} color="black" />
    }
    return <MaterialIcons name="pets" size={24} color="black" />
  }

  const renderPetItem = (pet: PetDTO) => {
    return <List.Item
      style={{padding: 10}}
      title={pet.name}
      onPress={() => petClicked(pet)}
      left={() => renderPetIcon(pet.petType)}
    />
  }


  return (
    <View padding-20>
      <Stack.Screen
        options={{
          headerRight: () => <IconButton
            icon="plus"
            size={20}
            onPress={() => console.log('Pressed')}
          />
        }}
      />
      <FlatList
        data={pets}
        renderItem={({ item }) => renderPetItem(item)}
        keyExtractor={item => item.id}
      />
    </View >
  );
}

const styles = StyleSheet.create({

})
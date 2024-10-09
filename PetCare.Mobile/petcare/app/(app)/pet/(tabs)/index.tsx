import petApi from '@/api/petApi';
import { PetDTO } from '@/api/types/pets';
import { displayPetSex, displayPetType } from '@/utils/petUtils';
import { Stack, useGlobalSearchParams, useNavigation } from 'expo-router';
import { useEffect, useState } from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { List } from 'react-native-paper';

export default function Tab() {

  const { id, petType } = useGlobalSearchParams();

  const [pet, setPet] = useState<PetDTO | undefined>();

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    const petResponse = await petApi.getPet(id as string);
    setPet(petResponse);
  }

  return (
    <View style={styles.container}>
      {pet && <List.Section>
        <List.Subheader>Pet Profile</List.Subheader>
        <List.Item title="Name" description={pet.name} />
        <List.Item title="Pet Type" description={displayPetType(pet.petType)} />
        <List.Item title="Sex" description={displayPetSex(pet.sex)} />
        <List.Item title="Date of Birth" description={pet.dateOfBirth.toLocaleDateString()} />
      </List.Section>}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
});

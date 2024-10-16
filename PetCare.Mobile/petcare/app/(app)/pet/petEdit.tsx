import { Stack, useGlobalSearchParams, useNavigation, useRouter } from "expo-router";
import { useEffect, useState } from "react";
import { View, Text, StyleSheet, TouchableOpacity } from "react-native";
import { Button, IconButton, List, RadioButton, TextInput } from "react-native-paper";
import DateTimePicker, {
  DateTimePickerEvent,
} from "@react-native-community/datetimepicker";
import petApi from "@/api/petApi";
import { PetDTO } from "@/api/types/pets";

export default function AddCatScreen() {
  const { id } = useGlobalSearchParams();
  const navigation = useNavigation();

  const [pet, setPet] = useState<PetDTO>();
  const [name, setName] = useState<string>("");
  const [dob, setDob] = useState<Date>(new Date());
  const [datePickerOpen, setDatePickerOpen] = useState<boolean>(false);
  const [sex, setSex] = useState<number>(0);
  const router = useRouter();

  const onConfrimDob = (event: DateTimePickerEvent, date: Date | undefined) => {
    const {
      type,
      nativeEvent: { timestamp, utcOffset },
    } = event;
    setDatePickerOpen(false);
    if (type == "set" && date) {
      setDob(date);
    }
  };

  const updatePet = async () => {
    if (pet) {
      await petApi.updatePet({
        id: pet.id,
        name: name,
        dateOfBirth: dob,
        sex: sex,
        petType: pet.petType
      })
      router.back();
    }
  }

  const loadPet = async () => {
    const petDTO = await petApi.getPet(id as string);
    setPet(petDTO);
    setName(petDTO.name);
    setSex(petDTO.sex);
    setDob(petDTO.dateOfBirth);
    navigation.setOptions({
      title: `Edit ${petDTO.name}`
    })
  }

  useEffect(() => {
    loadPet();
  }, [])

  return (
    <View padding-20>
      <Stack.Screen options={{}} />
      <View style={styles.formContainer}>
        <TextInput
          style={styles.input}
          label="Name"
          autoCapitalize="none"
          value={name}
          onChangeText={(newName) => {
            setName(newName);
          }}
        />
        <>
          <TouchableOpacity onPress={() => setDatePickerOpen(true)}>
            <View pointerEvents="none">
              <TextInput value={dob?.toLocaleDateString()} />
            </View>
          </TouchableOpacity>
          {datePickerOpen && (
            <DateTimePicker
              testID="dateTimePicker"
              value={dob}
              is24Hour={true}
              onChange={onConfrimDob}
            />
          )}
        </>
        <List.Section title="Sex" titleStyle={styles.sexGroup}>
        <RadioButton.Group
          onValueChange={(newValue) => setSex(parseInt(newValue))}
          value={sex.toString()}
        >
          <View style={styles.sexRadioInput}>
            <Text>Male</Text>
            <RadioButton value="0" />
          </View>
          <View style={styles.sexRadioInput}>
            <Text>Female</Text>
            <RadioButton value="1" />
          </View>
        </RadioButton.Group>
        </List.Section>
        <Button mode="contained" onPress={updatePet} style={styles.button}>
          Update pet
        </Button>
      </View>
    </View>
  );
}
const styles = StyleSheet.create({
  formContainer: {
    paddingHorizontal: 10,
  },
  input: {
    marginVertical: 10,
  },
  sexRadioInput: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    paddingVertical: 8,
    paddingHorizontal: 24,
  },
  sexGroup: {
    fontSize: 18
  },
  button: {
    margin: 4,
  }
});

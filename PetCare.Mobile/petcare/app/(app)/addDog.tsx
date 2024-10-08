import { Stack, useRouter } from "expo-router";
import { useState } from "react";
import { View, Text, StyleSheet, TouchableOpacity } from "react-native";
import { Button, IconButton, List, RadioButton, TextInput } from "react-native-paper";
import dogsApi from "@/api/dogsApi";
import DateTimePicker, {
  DateTimePickerEvent,
} from "@react-native-community/datetimepicker";

export default function AddDogScreen() {
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

  const addDog = async () => {
    if (name && dob) {
      await dogsApi.addDog(name, dob, sex);
      router.back();
    }
  }

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
        <Button mode="contained" onPress={addDog} style={styles.button}>
          Add dog
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

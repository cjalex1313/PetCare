import { Stack } from "expo-router";
import { useState } from "react";
import { View, Text, StyleSheet, TouchableOpacity } from "react-native";
import { IconButton, TextInput } from "react-native-paper";
import DateTimePicker, { DateTimePickerEvent } from '@react-native-community/datetimepicker';


export default function AddCatScreen() {

  const [name, setName] = useState<string>('');
  const [dob, setDob] = useState<Date>(new Date());
  const [datePickerOpen, setDatePickerOpen] = useState<boolean>(false);

  const onConfrimDob =  (event: DateTimePickerEvent, date: Date | undefined) => {
    const {
      type,
      nativeEvent: {timestamp, utcOffset},
    } = event;
    setDatePickerOpen(false);
    if (type == 'set' && date) {
      setDob(date);
    }
  };

  return (
    <View padding-20>
      <Stack.Screen
        options={{
          
        }}
      />
      <View style={styles.formContainer}>
        <TextInput
          style={styles.input}
          label="Name"
          autoCapitalize='none'
          value={name}
          onChangeText={(newName) => { setName(newName) }}
        />
        <>
          <TouchableOpacity onPress={() => setDatePickerOpen(true)}>
            <View pointerEvents="none">
              <TextInput value={dob?.toLocaleDateString()}/>
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
      </View>
    </View >
  );
}
const styles = StyleSheet.create({
  formContainer: {
    paddingHorizontal: 10
  },
  input: {
    marginVertical: 10
  }
})
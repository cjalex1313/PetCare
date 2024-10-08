import React, { useEffect, useState } from "react";
import petApi from "@/api/petApi";
import { useDispatch, useSelector } from "react-redux";
import { setPets } from "@/store/pets";
import { IRootState } from "@/store/store";
import { PetDTO, PetType } from "@/api/types/pets";
import { FlatList, StyleSheet, View, Text } from "react-native";
import FontAwesome5 from "@expo/vector-icons/FontAwesome5";
import MaterialIcons from "@expo/vector-icons/build/MaterialIcons";
import AntDesign from "@expo/vector-icons/build/AntDesign";
import {
  Appbar,
  Button,
  FAB,
  IconButton,
  List,
  Menu,
  Portal,
} from "react-native-paper";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { Stack, useNavigation, useRouter, usePathname } from "expo-router";

export default function HomeScreen() {
  const dispatch = useDispatch();

  const router = useRouter();
  const route = usePathname();

  const navigation = useNavigation();
  const isFocused = navigation.isFocused();

  const logOut = async () => {
    const token = await AsyncStorage.getItem("JWT");
    if (token) {
      await AsyncStorage.clear();
    }
    router.replace("/login");
  };

  const pets = useSelector<IRootState, PetDTO[]>((state) => state.pets.pets);
  const [menuVisible, setMenuVisible] = useState<boolean>(false);
  const [fabOpen, setFabOpen] = useState<boolean>(false);

  useEffect(() => {
    if (isFocused) {
      loadPets();
    }
  }, [isFocused]);

  const loadPets = async () => {
    const pets = await petApi.getPets();
    dispatch(setPets(pets));
  };

  const petClicked = (pet: PetDTO) => {
    console.log(pet.name);
  };

  const renderPetIcon = (petType: PetType) => {
    if (petType == PetType.Cat) {
      return <FontAwesome5 name="cat" size={24} color="black" />;
    }
    if (petType == PetType.Dog) {
      return <FontAwesome5 name="dog" size={24} color="black" />;
    }
    return <MaterialIcons name="pets" size={24} color="black" />;
  };

  const renderPetItem = (pet: PetDTO) => {
    return (
      <List.Item
        style={{ padding: 10 }}
        title={pet.name}
        onPress={() => petClicked(pet)}
        left={() => renderPetIcon(pet.petType)}
      />
    );
  };

  const openMenu = () => {
    setMenuVisible(true);
  };

  const closeMenu = () => {
    setMenuVisible(false);
  };

  const onFabStateChange = (state: { open: boolean }) => {
    setFabOpen(state.open);
  };

  const goToAddCat = () => {
    router.push("./addCat");
  };

  const goToAddDog = () => {
    router.push("./addDog");
  };

  return (
    <View padding-20>
      <Stack.Screen
        options={{
          headerRight: () => (
            <Menu
              anchorPosition="bottom"
              visible={menuVisible}
              onDismiss={closeMenu}
              anchor={
                <IconButton icon="dots-vertical" size={20} onPress={openMenu} />
              }
            >
              <Menu.Item onPress={logOut} title="Logout" />
            </Menu>
          ),
        }}
      />
      <FlatList
        data={pets}
        renderItem={({ item }) => renderPetItem(item)}
        keyExtractor={(item) => item.id}
      />
      <Portal>
        {route == "/" && (
          <FAB.Group
            open={fabOpen}
            visible
            icon={fabOpen ? "close" : "plus"}
            actions={[
              {
                icon: "cat",
                size: "medium",
                onPress: goToAddCat,
              },
              {
                icon: "dog",
                size: "medium",
                onPress: goToAddDog,
              },
            ]}
            onStateChange={onFabStateChange}
            onPress={() => {
              if (fabOpen) {
                setFabOpen(false);
              }
            }}
          />
        )}
      </Portal>
    </View>
  );
}

const styles = StyleSheet.create({
  fab: {
    // position: 'absolute',
    margin: 16,
    right: 0,
    bottom: 0,
  },
});

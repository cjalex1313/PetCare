import AsyncStorage from "@react-native-async-storage/async-storage";
import axios from "axios";
import { handleDates } from "./utils";

const instance = axios.create({
  baseURL: process.env.EXPO_PUBLIC_API_URL,
});

instance.interceptors.request.use(async function (config) {
  const token = await AsyncStorage.getItem('JWT')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
}, error => {
})

instance.interceptors.response.use((rep) => {
  handleDates(rep.data);
  return rep;
});

export default instance;
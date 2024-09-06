import { configureStore } from '@reduxjs/toolkit'
import counterReducer from './counter'
import userReducer from './user'
import petsReducer from './pets'

const store = configureStore({
  reducer: {
    counter: counterReducer,
    user: userReducer,
    pets: petsReducer
  },
})

export type IRootState = ReturnType<typeof store.getState>

export default store;
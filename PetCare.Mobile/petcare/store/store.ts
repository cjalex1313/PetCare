import { configureStore } from '@reduxjs/toolkit'
import counterReducer from './counter'
import userReducer from './user'

const store = configureStore({
  reducer: {
    counter: counterReducer,
    user: userReducer,
  },
})

export type IRootState = ReturnType<typeof store.getState>

export default store;
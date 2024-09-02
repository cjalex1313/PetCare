import { createSlice } from '@reduxjs/toolkit'

export const userSlice = createSlice({
  name: 'user',
  initialState: {
    isLoggedIn: false
  },
  reducers: {
    setIsLoggedIn: (state, action) => {
        state.isLoggedIn = action.payload
    },
  },
})

// Action creators are generated for each case reducer function
export const { setIsLoggedIn } = userSlice.actions

export default userSlice.reducer
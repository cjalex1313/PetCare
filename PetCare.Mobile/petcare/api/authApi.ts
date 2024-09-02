import baseApi from '@/api/baseApi'

export default {
    login: async (username: string, password: string) => {
        try {
            const result = await baseApi.post('/Auth/Login', {
                username,
                password
            });
            return result.data;
        }
        catch(e) {
            console.log(JSON.stringify(e));
        }
    }
}
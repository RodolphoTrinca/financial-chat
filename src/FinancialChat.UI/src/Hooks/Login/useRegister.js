const REGISTER_URL = "/register";
import { useAxiosFunction } from '../Axios/useAxiosFunction';

export const useRegister = () => {
    const [response, error, loading, fetch] = useAxiosFunction();

    const postData = (data) => {
        fetch({
            axiosInstance: axios,
            method: 'POST',
            url: REGISTER_URL,
            data: data
        })
    };

    return [response, error, loading, postData];
}
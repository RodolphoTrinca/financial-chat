import axios from 'axios';

export const BASE_URL = "http://localhost:10000/api";

export default axios.create({
    baseURL: BASE_URL,
    headers: {
        'Content-Type': 'application/json; charset=utf-8',
        'Accept': 'application/json'
    }
});
import axios from 'axios';

const BASE_URL = "http://localhost:5071/";

export default axios.create({
    baseURL: BASE_URL,
    headers: {
        'Content-Type': 'application/json; charset=utf-8',
        'Accept': 'application/json'
    }
});
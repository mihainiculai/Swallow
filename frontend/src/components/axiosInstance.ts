import axios from 'axios';

const baseURL = process.env.NEXT_PUBLIC_API_URL;

export const axiosInstance = axios.create({
    baseURL: baseURL + '/api/',
    headers: {
        'Content-Type': 'application/json',
    },
    timeout: 10000,
    withCredentials: true,
});
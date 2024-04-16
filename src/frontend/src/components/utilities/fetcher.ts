import { axiosInstance } from "./axiosInstance";

export const fetcher = (url: string) => axiosInstance.get(url).then(res => res.data)
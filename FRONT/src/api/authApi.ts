import { ApiMethod } from "../shared/hooks/useApi";

export function Login(username: string, password: string) {
    return {url: '/User/Login', method: ApiMethod.POST, body: {
        username,
        password
    }};
};

export function Logout(username: string) {
    return {url: '/User/Logout', method: ApiMethod.POST, body: {
        username,
    }};
};

export function Register(username: string, password: string, email: string, phoneNumber: string, deviceId: string) {
    return {url: '/User/Register', method: ApiMethod.POST, body: {
        username,
        password,
        email,
        phoneNumber,
        deviceId
    }};
};
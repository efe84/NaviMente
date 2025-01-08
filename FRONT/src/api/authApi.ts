import { ApiMethod } from "../shared/hooks/useApi";

export function Login(username: string, password: string) {
    return {url: '/User/Login', method: ApiMethod.POST, body: {
        username,
        password
    }};
};

export function GetUser(username: string) {
    return {url: `/User?username=${username}`, method: ApiMethod.GET};
};

export function EditEmail(username: string, newEmail: string) {
    return {url: `/User/EditEmail?username=${username}`, method: ApiMethod.POST, body: newEmail};
};

export function EditMainPhone(username: string, newMainPhone: string) {
    return {url: `/User/EditMainPhone?username=${username}`, method: ApiMethod.POST, body: newMainPhone};
};

export function AddPhone(username: string, newPhone: string) {
    return {url: `/User/AddPhone?username=${username}`, method: ApiMethod.POST, body: newPhone};
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
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
    return {url: `/User/EditEmail?username=${username}`, method: ApiMethod.PUT, body: {newEmail}};
};

export function EditMainPhone(username: string, newMainPhone: string) {
    return {url: `/User/EditMainPhone?username=${username}`, method: ApiMethod.PUT, body: {newMainPhone}};
};

export function AddPhone(username: string, newPhone: string) {
    return {url: `/User/AddPhone?username=${username}`, method: ApiMethod.POST, body: {newPhone}};
};

export function RemovePhone(username: string, phoneNumber: string) {
    return {url: `/User/DeletePhone?username=${username}&phoneNumber=${phoneNumber}`, method: ApiMethod.DELETE};
};

/* istanbul ignore next */
export function Logout(username: string) {
    return {url: '/User/Logout', method: ApiMethod.POST, body: {
        username,
    }};
};

export function Register(username: string, password: string, email: string, mainPhone: string, deviceId: string, deviceName: string) {
    return {url: '/User/Register', method: ApiMethod.POST, body: {
        username,
        password,
        email,
        mainPhone,
        deviceId,
        deviceName
    }};
};

export function GenerateTelegramCode(userId: number) {
    return {url: `/User/GenerateCode?userId=${userId}`, method: ApiMethod.POST};
};

export function UnlinkTelegram(userId: number) {
    return {url: `/User/UnlinkTelegram?userId=${userId}`, method: ApiMethod.PUT};
};
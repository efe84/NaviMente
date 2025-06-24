import { ApiMethod } from "../shared/hooks/useApi";

export function Zones(serialNumber: string) {
    return {url: `/Device/Zones?serialNumber=${serialNumber}`, method: ApiMethod.GET};
};

export function BlockZone(payload: { serialNumber: string; shapes: ({ type: string; coordinates: number[][][]; } | null) []; }) {
    return {url: '/Device/BlockZone', method: ApiMethod.POST, body: payload};
};

export function GetDevices(userId: string) {
    return {url: `/Device/List?userId=${userId}`, method: ApiMethod.GET};
};

export function UnassignDevice(userId: number, serialNumber: string) {
    return {url: `/Device/Unassign?userId=${userId}&&serialNumber=${serialNumber}`, method: ApiMethod.DELETE};
};

export function RegisterDevice(deviceRegister: any) {
    return {url: `/Device/Register`, method: ApiMethod.POST, body: deviceRegister};
};
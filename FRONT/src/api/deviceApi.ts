import { ApiMethod } from "../shared/hooks/useApi";

export function Zones(serialNumber: string) {
    return {url: '/Device/Zones', method: ApiMethod.POST, body: serialNumber};
};

export function BlockZone(payload: { serialNumber: string; shapes: ({ type: string; coordinates: number[][][]; } | null) []; }) {
    return {url: '/Device/BlockZone', method: ApiMethod.POST, body: payload};
  
export function GetDevices(userId: string) {
    return {url: `/Device/List?userId=${userId}`, method: ApiMethod.GET};
};
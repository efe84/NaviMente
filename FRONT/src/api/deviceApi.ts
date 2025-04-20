import { ApiMethod } from "../shared/hooks/useApi";

export function List(userName: string) {
    return {url: '/Device/List', method: ApiMethod.POST, body: userName};
};

export function Zones(serialNumber: string) {
    return {url: '/Device/Zones', method: ApiMethod.POST, body: serialNumber};
};

export function BlockZone(payload: { serialNumber: string; shapes: ({ type: string; coordinates: number[][][]; } | null) []; }) {
    return {url: '/Device/BlockZone', method: ApiMethod.POST, body: payload};
};
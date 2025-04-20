import { ApiMethod } from "../shared/hooks/useApi";

export function SearchRoute(serialNumber: string, startDate: any, endDate: any) {
    return {url: '/Location/Route', method: ApiMethod.POST, body: {serialNumber, startDate, endDate}};
};

export function SearchLastLocation(serialNumber: string) {
    return {url: '/Location/Last', method: ApiMethod.POST, body: serialNumber};
};
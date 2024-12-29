import { ApiMethod } from "../shared/hooks/useApi";

export function GetDevices(userId: string) {
    return {url: '/Device/List', method: ApiMethod.POST, body: userId};
};
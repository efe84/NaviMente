import { ApiMethod } from "../shared/hooks/useApi";

export function GetDevices(userId: string) {
    return {url: `/Device/List?userId=${userId}`, method: ApiMethod.GET};
};
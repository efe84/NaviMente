import { ApiMethod } from "../shared/hooks/useApi";

export function List(userName: string) {
    return {url: '/Device/List', method: ApiMethod.POST, body: userName};
};
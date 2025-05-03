import { ApiMethod } from "../shared/hooks/useApi";

export function GetLogs(serialNumber: string, severity?: number | null) {
    const url = severity != null 
      ? `/Binnacle/${serialNumber}?severity=${severity}`
      : `/Binnacle/${serialNumber}`;
  
    return { url, method: ApiMethod.GET };
  }
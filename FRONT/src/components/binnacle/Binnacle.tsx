import React, { useEffect, useState } from "react";
import DeviceList from "./DeviceList";
import ChatLog from "./ChatLog";
import QuickActions from "./QuickActions";
import Footer from "../layout/Footer";
import { GetLogs } from "../../api/binnacleApi";
import { useApi } from "../../shared/hooks/useApi";
import { GetDevices } from "../../api/deviceApi";

const Binnacle: React.FC = () => {
  const callApi = useApi();
  const [devices, setDevices] = useState<{ serialNumber: string; name: string }[]>([]);
  const [messages, setMessages] = useState<any[]>([]);
  const [selectedDevice, setSelectedDevice] = useState<{ serialNumber: string; name: string } | null>(null);

  const fetchLogs = (serialNumber: string, severity?: number | null) => {
    callApi(GetLogs(serialNumber, severity)).then((response: any) => {
      setMessages(response);
    });
  };

  useEffect(() => {
    callApi(GetDevices("1")).then((response: any) => {
      setDevices(response);
      if (response.length > 0) {
        setSelectedDevice(response[0]);
        fetchLogs(response[0].serialNumber);
      }
    });
  }, []);

  const handleDeviceSelect = (device: { serialNumber: string; name: string }) => {
    setSelectedDevice(device);
    fetchLogs(device.serialNumber);
  };

  const handleFilter = (severity: number | null) => {
    if (selectedDevice) {
      fetchLogs(selectedDevice.serialNumber, severity);
    }
  };

  return (
    <>
      <div style={{ backgroundColor: "#fafafa", height: "calc(100vh - 150px)", display: "flex" }}>

        <div className="col-2 p-0">
          <DeviceList devices={devices} onSelectDevice={handleDeviceSelect} selectedDevice={selectedDevice} />
        </div>

        <div className="col-8 p-0">
          <ChatLog messages={messages} />
        </div>

        <div className="col-2 p-0">
          <QuickActions onFilter={handleFilter} />
        </div>

      </div>
      <div>
        <Footer />
      </div>
    </>
  );
};

export default Binnacle;
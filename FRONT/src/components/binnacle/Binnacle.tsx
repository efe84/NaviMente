import React, { useState } from "react";
import DeviceList from "./DeviceList";
import ChatLog from "./ChatLog";
import QuickActions from "./QuickActions";
import Footer from "../layout/Footer";

const Binnacle: React.FC = () => {
  const [selectedDevice, setSelectedDevice] = useState<{ id: number; name: string } | null>({id: 1,name: "David Band"}); //Recuperar las bands del usuario y pasar la primera si hay alguna o null si no hay

  const handleDeviceSelect = (device: { id: number; name: string }) => {
    setSelectedDevice(device);
  };

  return (
    <>
    <div style={{ backgroundColor: "#fafafa", height: "calc(100vh - 150px)", display:"flex" }}>
      {/* Lista de dispositivos */}
      <div className="col-2 p-0">
        <DeviceList onSelectDevice={handleDeviceSelect} />
      </div>

      {/* Chat log central */}
      <div className="col-8 p-0">
        <ChatLog device={selectedDevice} />
      </div>

      {/* Acciones rápidas */}
      <div className="col-2 p-0">
        <QuickActions device={selectedDevice} />
      </div>
    </div>
    <div>
        <Footer/>
    </div>
    </>
  );
};

export default Binnacle;
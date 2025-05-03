import React from "react";

interface DeviceListProps {
  devices: { serialNumber: string; name: string }[];
  onSelectDevice: (device: { serialNumber: string; name: string }) => void;
  selectedDevice: { serialNumber: string; name: string } | null;
}
  
  const DeviceList: React.FC<DeviceListProps> = ({ devices, onSelectDevice, selectedDevice }) => {
    return (
      <div>
        <h5 className="px-3" style={{ paddingTop:"12%" }}><b>Devices</b></h5>
        <div className="d-flex flex-column gap-2 px-3 pt-2">
          {devices.map((device) => (
            <button
              key={device.serialNumber}
              className="btn"
              style={{
                textAlign: "left",
                padding: "10px 15px",
                border: "none",
                boxShadow: "none",
                backgroundColor: selectedDevice?.serialNumber === device.serialNumber ? "#E8E8E8" : "transparent",
                transition: "background-color 0.2s",
                cursor: "pointer",
              }}
              onClick={() => onSelectDevice(device)}
            >
              {device.name}
            </button>
          ))}
        </div>
      </div>
    );
  };
  
  export default DeviceList;
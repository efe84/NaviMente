import React from "react";

interface DeviceListProps {
    onSelectDevice: (device: { id: number; name: string }) => void;
  }
  
  const DeviceList: React.FC<DeviceListProps> = ({ onSelectDevice }) => {
    const devices = [
      { id: 1, name: "David Band" },
      { id: 2, name: "Martin Band" },
    ];
  
    return (
      <div>
        <h5 style={{ paddingTop:"12%", paddingLeft: "10%" }}><b>Devices</b></h5>
        <ul className="list-unstyled py-2" style={{ paddingLeft: "10%" }}>
          {devices.map((device) => (
            <li
              key={device.id}
              onClick={() => onSelectDevice(device)}
              style={{
                padding: "10px",
                cursor: "pointer",
                borderRadius: "5px",
                transition: "background-color 0.2s",
              }}
              onMouseEnter={(e) =>
                (e.currentTarget.style.backgroundColor = "#E8E8E8")
              }
              onMouseLeave={(e) =>
                (e.currentTarget.style.backgroundColor = "transparent")
              }
            >
              {device.name}
            </li>
          ))}
        </ul>
      </div>
    );
  };
  
  export default DeviceList;
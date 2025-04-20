import React from "react";

interface QuickActionsProps {
    device: { id: number; name: string } | null;
  }
  
  const QuickActions: React.FC<QuickActionsProps> = ({ device }) => {
    const handleAction = (action: string) => {
      alert(`Action: ${action} executed for ${device?.name || "no device"}`);
    };
  
    return (
      <div>
        <h5 className="px-3" style={{ paddingTop:"12%" }}><b>Quick Actions</b></h5>
        <div className="d-flex flex-column gap-2 px-3 pt-2">
          <button
            className="btn"
            style={{
              textAlign: "left",
              padding: "10px 15px",
              border: "none",
              boxShadow: "none",
              transition: "background-color 0.2s",
            }}
            onMouseEnter={(e) =>
                (e.currentTarget.style.backgroundColor = "#E8E8E8")
            }
            onMouseLeave={(e) =>
                (e.currentTarget.style.backgroundColor = "transparent")
            }
            onClick={() => handleAction("Check Last Connection")}
            disabled={!device}
          >
            Check Last Connection
          </button>
          <button
            className="btn"
            style={{
              textAlign: "left",
              padding: "10px 15px",
              border: "none",
              boxShadow: "none",
              transition: "background-color 0.2s",
            }}
            onMouseEnter={(e) =>
                (e.currentTarget.style.backgroundColor = "#E8E8E8")
            }
            onMouseLeave={(e) =>
                (e.currentTarget.style.backgroundColor = "transparent")
            }
            onClick={() => handleAction("Database Status")}
            disabled={!device}
          >
            Check Database Status
          </button>
          <button
            className="btn"
            style={{
              textAlign: "left",
              padding: "10px 15px",
              border: "none",
              boxShadow: "none",
              transition: "background-color 0.2s",
            }}
            onMouseEnter={(e) =>
                (e.currentTarget.style.backgroundColor = "#E8E8E8")
            }
            onMouseLeave={(e) =>
                (e.currentTarget.style.backgroundColor = "transparent")
            }
            onClick={() => handleAction("Reconnect Device")}
            disabled={!device}
          >
            Reconnect Device
          </button>
        </div>
      </div>
    );
  };
  
  export default QuickActions;
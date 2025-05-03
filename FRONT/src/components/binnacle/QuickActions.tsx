import React, { useState } from "react";

interface QuickActionsProps {
  onFilter: (severity: number | null) => void;
}

const QuickActions: React.FC<QuickActionsProps> = ({ onFilter }) => {
  const [activeSeverity, setActiveSeverity] = useState<number | null>(null);

  const handleAction = (severity: number | null) => {
    setActiveSeverity(severity);
    onFilter(severity);
  };

  const buttonStyle = (severity: number | null): React.CSSProperties => ({
    textAlign: "left",
    padding: "10px 15px",
    border: "none",
    boxShadow: "none",
    backgroundColor: activeSeverity === severity ? "#E8E8E8" : "transparent",
    transition: "background-color 0.2s"
  });

  return (
    <div>
      <h5 className="px-3" style={{ paddingTop:"12%", marginLeft:"10px" }}><b>Filter</b></h5>
      <div className="d-flex flex-column gap-2 px-3 pt-2">
      <button
          className="btn"
          style={buttonStyle(null)}
          onClick={() => handleAction(null)}
        >
          All Messages
        </button>

        <button
          className="btn"
          style={buttonStyle(1)}
          onClick={() => handleAction(1)}
        >
          Information Messages
        </button>

        <button
          className="btn"
          style={buttonStyle(2)}
          onClick={() => handleAction(2)}
        >
          Warning Messages
        </button>

        <button
          className="btn"
          style={buttonStyle(3)}
          onClick={() => handleAction(3)}
        >
          Error Messages
        </button>
      </div>
    </div>
  );
};

export default QuickActions;
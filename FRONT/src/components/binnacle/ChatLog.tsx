import React, { useRef } from "react";

interface ChatLogProps {
    messages: Message[];
}

interface Message {
    timestamp: string;
    severity: number;
    message: string;
}

const severityLabels: Record<number, { label: string; color: string }> = {
    1: { label: "[INF]", color: "green" },
    2: { label: "[WAR]", color: "orange" },
    3: { label: "[ERR]", color: "red" },
};

const ChatLog: React.FC<ChatLogProps> = ({ messages }) => {
    const containerRef = useRef<HTMLDivElement>(null);

    const groupMessagesByDate = (messages: Message[]) => {
        const groups: { [date: string]: Message[] } = {};
        messages.forEach((msg) => {
            const [date] = msg.timestamp.split(' ');
            if (!groups[date]) {
                groups[date] = [];
            }
            groups[date].push(msg);
        });
        return groups;
    };

    const groupedMessages = groupMessagesByDate(messages);

    return (
        <div
            ref={containerRef}
            className="d-flex flex-column border-left border-right"
            style={{
                paddingTop: "5%",
                paddingLeft: "8%",
                paddingRight: "8%",
                borderLeft: "2px solid black",
                borderRight: "2px solid black",
                height: "100%",
            }}
        >
            <div className="flex-grow-1 px-3 overflow-auto" style={{ height: "70vh", overflowY: "auto" }}>
                {Object.keys(groupedMessages).length === 0 ? (
                    <div style={{ textAlign: "center", fontWeight: "bold", marginTop: "25px", marginBottom: "10px", color: "gray" }}>
                        No messages registered yet
                    </div>
                ) : (
                    Object.entries(groupedMessages).map(([date, logs]) => (
                        <div key={date}>
                            <div style={{ textAlign: "center", fontWeight: "bold", marginTop: "25px", marginBottom: "10px", color: "gray" }}>
                                {date}
                            </div>
                            {logs.map((msg, index) => {
                                const [_, time] = msg.timestamp.split(' ');
                                const severityInfo = severityLabels[msg.severity] || { label: "[UNK]", color: "gray" };
                                return (
                                    <div key={index} className="mb-2 d-flex align-items-center">
                                        <div>
                                            <span style={{ color: "black", marginRight: "8px" }}>
                                                {time} -
                                            </span>
                                            <span style={{ color: severityInfo.color, marginRight: "8px" }}>
                                                {severityInfo.label}
                                            </span>
                                            {msg.message}
                                        </div>
                                    </div>
                                );
                            })}
                        </div>
                    ))
                )}
            </div>
        </div>
    );
};

export default ChatLog;
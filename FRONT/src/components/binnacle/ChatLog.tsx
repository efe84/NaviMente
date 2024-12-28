import React, { useState, useEffect } from "react";
import botIcon from '../../assets/bot.png';
import userIcon from '../../assets/user.png';

interface ChatLogProps {
    device: { id: number; name: string } | null;
}

interface Message {
    id: number;
    text: string;
    creator: 'user' | 'bot';
  }

const ChatLog: React.FC<ChatLogProps> = ({ device }) => {
    const [messages, setMessages] = React.useState<Message[]>([]);

    React.useEffect(() => {
        if (device) {
            setMessages([
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
                { id: 1, text: `New connection: ${device.name} connected.`, creator: "bot" },
            ]);
        }
    }, [device]);

    const handleSendMessage = (text: string, creator: 'user' | 'bot') => {
        const newMessage = {
          id: messages.length + 1,
          text: text,
          creator: creator,
        };
      
        setMessages((prevMessages) => [...prevMessages, newMessage]);
      
        if (creator === 'user') {
          setTimeout(() => {
            handleSendMessage("Hello, how can I assist you?", 'bot');
          }, 1000);
        }
      };

    return (
        <div className="d-flex flex-column" style={{ paddingTop: "5%", paddingLeft: "8%", paddingRight: "8%" }}>
            <div className="flex-grow-1 px-3 overflow-auto " style={{ height: "70vh", overflowY: "auto" }}>
                {messages.map((msg) => (
                    <div key={msg.id} className={`mb-2 d-flex align-items-center ${msg.creator === 'bot' ? 'justify-content-start' : 'justify-content-end'}`}>
                    {msg.creator === 'bot' ? (
                      <img
                        src={botIcon}
                        alt="Icono de mensaje"
                        style={{ width: "30px", height: "30px", marginRight: "10px" }}
                      />
                    ):(
                        <img
                        src={userIcon}
                        alt="Icono de mensaje"
                        style={{ width: "30px", height: "30px", marginRight: "10px" }}
                      />
                    )}
                    <div>
                      {msg.text}
                    </div>
                  </div>
                ))}
            </div>
        </div>
    );
};

export default ChatLog;
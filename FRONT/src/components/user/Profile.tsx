import React, { useState, useEffect } from 'react';
import { useParams } from "react-router-dom";
import Footer from '../layout/Footer';
import image from '../../assets/user.png';
import add from '../../assets/add.png';
import linked from '../../assets/linked.png'
import { useApi } from '../../shared/hooks/useApi';
import { AddPhone, GetUser } from '../../api/authApi';
import { GetDevices } from '../../api/deviceApi';

type User = {
    userId: number;
    username: string;
    email: string;
    mainPhone: string;
    otherPhones: string[];
    role: number;
    telegramLinked: boolean;
    devices: Device[];
};

type Device = {
    deviceName: string;
    lastUpdate: Date | null;
}

export default function Profile() {
    const callApi = useApi();
    const { username } = useParams<string>();
    const [user, setUser] = useState<User | null>(null);
    const [selectedDevice, setSelectedDevice] = useState("");
    const [showAddPhone, setShowAddPhone] = useState(false);
    const [newPhone, setNewPhone] = useState("");
    const [showAddDevice, setShowAddDevice] = useState(false);
    const [newDevice, setNewDevice] = useState("");

    useEffect(() => {
        if (username != null && username != undefined) {
            callApi(GetUser(username)).then((result: any) => {
                const parsedUser: User = {
                    userId: result.userId,
                    username: result.username,
                    email: result.email,
                    mainPhone: result.mainPhone,
                    otherPhones: result.otherPhones || [],
                    role: result.role,
                    telegramLinked: false, //TODO: CAMBIAR POR EL BOOLEANO DE SI ESTA LINKED
                    devices: []
                };
                setUser(parsedUser);
                return callApi(GetDevices(result.userId));
            }).then((devices: any) => {
                setUser((prevUser) =>
                    prevUser
                        ? {
                            ...prevUser,
                            devices: devices,
                        }
                        : null
                );
            })
        };
    }, [username]);

    const addAdditionalPhone = () => {
        if(username != null){
            callApi(AddPhone(username, newPhone)).then((result: any) => {
                setUser((prevUser) =>
                    prevUser
                        ? {
                              ...prevUser,
                              otherPhones: result,
                          }
                        : null
                );
            })
        }
    };

    const addDevice = () => {

    };

    const toggleTelegramLink = () => {

    };

    return (
        <div style={{ width: '100vw', fontFamily: 'Arial, sans-serif', color: '#333', backgroundColor: '#fafafa', padding: '20px 0' }}>
            <div style={{ display: 'flex', marginLeft: '50px', paddingTop: '20px', alignItems: 'center', marginBottom: '20px' }}>
                <div style={{ marginRight: '20px' }}>
                    <img src={image} style={{ height: "90px" }} />
                </div>
                <div>
                    <h1 style={{ margin: 0, fontSize: '24px', fontWeight: 'bold', }}>{user?.username}</h1>
                    <p style={{ margin: 0, fontSize: '16px', color: '#777', }}>User's Profile</p>
                </div>
            </div>

            <div style={{ display: 'flex', flexDirection: 'row', margin: '0 50px' }}>
                <div style={{ flex: 1, marginRight: '20px' }}>
                    <div style={{ marginBottom: '20px', padding: '20px' }}>
                        <h3>User Information</h3>
                        <div style={{ marginBottom: '10px', padding: '10px 0', borderBottom: '1px solid #ddd' }}>
                            <strong>Email: </strong>
                            <span>{user?.email}</span>
                        </div>
                        <div style={{ marginBottom: '10px', padding: '10px 0', borderBottom: '1px solid #ddd' }}>
                            <strong>Main Phone: </strong>
                            <span>{user?.mainPhone}</span>
                        </div>
                        <div style={{ marginBottom: '10px', padding: '10px 0', borderBottom: '1px solid #ddd' }}>
                            <strong>Additional Phones: </strong>
                            <ul style={{ margin: 0, padding: 0, listStyleType: 'none' }}>
                                {user?.otherPhones.map((phone, index) => (
                                    <li key={index}> - {phone}</li>
                                ))}
                            </ul>
                            <div style={{ marginTop: "5px" }}>
                                <img
                                    src={add}
                                    alt="Add Phone"
                                    style={{ width: '32px', height: '32px', cursor: 'pointer' }}
                                    onClick={() => setShowAddPhone(!showAddPhone)}
                                />
                                {showAddPhone && (
                                    <input
                                        type="text"
                                        placeholder="Enter new phone"
                                        value={newPhone}
                                        onChange={(e) => setNewPhone(e.target.value)}
                                        style={{
                                            marginLeft: '10px',
                                            padding: '10px',
                                            fontSize: '14px',
                                            border: '1px solid #ddd',
                                            borderRadius: '8px',
                                            width: '200px',
                                            outline: 'none',
                                            boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
                                        }}
                                    />
                                )}
                                {showAddPhone && (
                                    <button
                                        onClick={() => {
                                            if (newPhone.trim()) {
                                                addAdditionalPhone();
                                                setNewPhone('');
                                                setShowAddPhone(false);
                                            }
                                        }}
                                        style={{
                                            marginLeft: '10px',
                                            padding: '10px 15px',
                                            fontSize: '14px',
                                            backgroundColor: '#DFDFDF',
                                            border: 'none',
                                            borderRadius: '8px',
                                            cursor: 'pointer',
                                            boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
                                        }}
                                    >
                                        Add
                                    </button>
                                )}
                            </div>
                        </div>
                        <div style={{ marginBottom: '10px', padding: '10px 0', borderBottom: '1px solid #ddd' }}>
                            <strong>Devices:</strong>
                            <ul style={{ margin: 0, padding: 0, listStyleType: 'none' }}>
                                {user?.devices.map((device, index) => (
                                    <li key={index}> - {device.deviceName}</li>
                                ))}
                            </ul>
                            <div style={{ marginTop: "5px" }}>
                                <img
                                    src={add}
                                    alt="Add Device"
                                    style={{ width: '32px', height: '32px', cursor: 'pointer' }}
                                    onClick={() => setShowAddDevice(!showAddDevice)}
                                />
                                {showAddDevice && (
                                    <input
                                        type="text"
                                        placeholder="Enter new device"
                                        value={newDevice}
                                        onChange={(e) => setNewDevice(e.target.value)}
                                        style={{
                                            marginLeft: '10px',
                                            padding: '10px',
                                            fontSize: '14px',
                                            border: '1px solid #ddd',
                                            borderRadius: '8px',
                                            width: '200px',
                                            outline: 'none',
                                            boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
                                        }}
                                    />
                                )}
                                {showAddDevice && (
                                    <button
                                        onClick={() => {
                                            if (newDevice.trim()) {
                                                addDevice();
                                                setNewDevice('');
                                                setShowAddDevice(false);
                                            }
                                        }}
                                        style={{
                                            marginLeft: '10px',
                                            padding: '10px 15px',
                                            fontSize: '14px',
                                            backgroundColor: '#DFDFDF',
                                            border: 'none',
                                            borderRadius: '8px',
                                            cursor: 'pointer',
                                            boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
                                        }}
                                    >
                                        Add
                                    </button>
                                )}
                            </div>
                        </div>
                    </div>
                </div>

                <div style={{ flex: 1, marginRight: '20px' }}>
                    <div style={{ marginBottom: '20px', padding: '20px' }}>
                        <h3>Unlink a Device</h3>
                        <div style={{ display: 'flex', alignItems: 'center', gap: '30px' }}>
                            <select
                                style={{
                                    width: '100%',
                                    padding: '10px',
                                    fontSize: '16px',
                                    borderRadius: '5px',
                                    border: '1px solid #ddd'
                                }}
                                value={selectedDevice}
                                onChange={(e) => setSelectedDevice(e.target.value)}
                            >
                                {user?.devices.map((device, index) => (
                                    <option key={index} value={device.deviceName}>
                                        {device.deviceName}
                                    </option>
                                ))}
                            </select>
                            <button
                                onClick={() => {
                                    if (window.confirm(`Are you sure you want to remove ${selectedDevice}?`)) {
                                        toggleTelegramLink();
                                        setSelectedDevice('');
                                    }
                                }}
                                style={{
                                    padding: '10px 15px',
                                    fontSize: '14px',
                                    backgroundColor: '#DFDFDF',
                                    border: 'none',
                                    borderRadius: '8px',
                                    cursor: 'pointer',
                                    boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
                                }}
                            >
                                Remove
                            </button>
                        </div>
                    </div>
                    <div style={{ marginBottom: '20px', padding: '20px' }}>
                        <h3>Security Settings</h3>
                        <p>Control access to Telegram integration.</p>
                        <button
                            style={{
                                padding: '10px 15px',
                                fontSize: '16px',
                                backgroundColor: '#DFDFDF',
                                border: 'none',
                                borderRadius: '8px',
                                cursor: 'pointer',
                                boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
                            }}
                            onClick={toggleTelegramLink}
                        >
                            {user?.telegramLinked ? 'Unlink Telegram' : 'Link Telegram'}
                        </button>
                        {user?.telegramLinked ?
                            <img src={linked} style={{ height: "30px", marginLeft: "10px" }} />
                            :
                            <></>
                        }
                    </div>
                </div>
            </div>
            <Footer />
        </div>
    );
};

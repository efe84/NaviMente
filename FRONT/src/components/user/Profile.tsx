import React, { useState } from 'react';
import Footer from '../layout/Footer';
import image from '../../assets/user.png';
import add from '../../assets/add.png';
import linked from '../../assets/linked.png'

export default function Profile() {
    const [user, setUser] = useState({
        name: 'David Fernandez',
        email: 'd.fernandez4@udc.es',
        mainPhone: '685528877',
        additionalPhones: ['630549122', '679708148'],
        devices: ['David Band', 'Martin Band'],
        telegramLinked: false,
    });
    const [selectedDevice, setSelectedDevice] = useState(user.devices[0]);
    const [showAddPhone, setShowAddPhone] = useState(false);
    const [newPhone, setNewPhone] = useState('');
    const [showAddDevice, setShowAddDevice] = useState(false);
    const [newDevice, setNewDevice] = useState('');

    const addAdditionalPhone = () => {
        const newPhone = prompt('Enter a new phone number:');
        if (newPhone) {
            setUser((prevState) => ({
                ...prevState,
                additionalPhones: [...prevState.additionalPhones, newPhone],
            }));
        }
    };

    const addDevice = () => {
        const newDevice = prompt('Enter a new device name:');
        if (newDevice) {
            setUser((prevState) => ({
                ...prevState,
                devices: [...prevState.devices, newDevice],
            }));
        }
    };

    const toggleTelegramLink = () => {
        setUser((prevState) => ({
            ...prevState,
            telegramLinked: !prevState.telegramLinked,
        }));
    };

    return (
        <div style={{ width: '100vw', fontFamily: 'Arial, sans-serif', color: '#333', backgroundColor: '#fafafa', padding: '20px 0' }}>
            <div style={{ display: 'flex', marginLeft: '50px', paddingTop: '20px', alignItems: 'center', marginBottom: '20px' }}>
                <div style={{ marginRight: '20px' }}>
                    <img src={image} style={{ height: "90px" }} />
                </div>
                <div>
                    <h1 style={{ margin: 0, fontSize: '24px', fontWeight: 'bold', }}>{user.name}</h1>
                    <p style={{ margin: 0, fontSize: '16px', color: '#777', }}>User's Profile</p>
                </div>
            </div>

            <div style={{ display: 'flex', flexDirection: 'row', margin: '0 50px' }}>
                <div style={{ flex: 1, marginRight: '20px' }}>
                    <div style={{ marginBottom: '20px', padding: '20px' }}>
                        <h3>User Information</h3>
                        <div style={{ marginBottom: '10px', padding: '10px 0', borderBottom: '1px solid #ddd' }}>
                            <strong>Email: </strong>
                            <span>{user.email}</span>
                        </div>
                        <div style={{ marginBottom: '10px', padding: '10px 0', borderBottom: '1px solid #ddd' }}>
                            <strong>Main Phone: </strong>
                            <span>{user.mainPhone}</span>
                        </div>
                        <div style={{ marginBottom: '10px', padding: '10px 0', borderBottom: '1px solid #ddd' }}>
                            <strong>Additional Phones: </strong>
                            <ul style={{ margin: 0, padding: 0, listStyleType: 'none' }}>
                                {user.additionalPhones.map((phone, index) => (
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
                                                setUser((prevState) => ({
                                                    ...prevState,
                                                    additionalPhones: [...prevState.additionalPhones, newPhone],
                                                }));
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
                                {user.devices.map((device, index) => (
                                    <li key={index}> - {device}</li>
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
                                                setUser((prevState) => ({
                                                    ...prevState,
                                                    devices: [...prevState.devices, newDevice],
                                                }));
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
                                {user.devices.map((device, index) => (
                                    <option key={index} value={device}>
                                        {device}
                                    </option>
                                ))}
                            </select>
                            <button
                                onClick={() => {
                                    if (window.confirm(`Are you sure you want to remove ${selectedDevice}?`)) {
                                        setUser((prevState) => ({
                                            ...prevState,
                                            devices: prevState.devices.filter((device) => device !== selectedDevice),
                                        }));
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
                            {user.telegramLinked ? 'Unlink Telegram' : 'Link Telegram'}
                        </button>
                        {user.telegramLinked ?
                            <img src={linked} style={{height: "30px", marginLeft: "10px"}}/>
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

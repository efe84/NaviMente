import React, { useState } from 'react';
import Footer from '../layout/Footer';

export default function Profile() {
    const [user, setUser] = useState({
        name: 'John Doe',
        email: 'johndoe@example.com',
        mainPhone: '+1234567890',
        additionalPhones: ['+1987654321', '+1123456789'],
        devices: ['Device1', 'Device2', 'Device3'],
        telegramLinked: false,
    });

    const [selectedDevice, setSelectedDevice] = useState(user.devices[0]);

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
        <div style={styles.container}>
            {/* Header Section */}
            <div style={styles.header}>
                <div style={styles.profilePicture}>
                    <span style={styles.profileIcon}>📸</span>
                </div>
                <div>
                    <h1 style={styles.username}>{user.name}</h1>
                    <p style={styles.profileTitle}>User's Profile</p>
                </div>
            </div>

            {/* Main Content */}
            <div style={styles.mainContent}>
                {/* Left Column */}
                <div style={styles.column}>
                    {/* User Information Section */}
                    <div style={styles.card}>
                        <h3>User Information</h3>
                        <div style={styles.infoRow}>
                            <strong>Email:</strong>
                            <span>{user.email}</span>
                        </div>
                        <div style={styles.infoRow}>
                            <strong>Main Phone:</strong>
                            <span>{user.mainPhone}</span>
                        </div>
                        <div style={styles.infoRow}>
                            <strong>Additional Phones:</strong>
                            <ul style={styles.list}>
                                {user.additionalPhones.map((phone, index) => (
                                    <li key={index}>{phone}</li>
                                ))}
                            </ul>
                            <button style={styles.addButton} onClick={addAdditionalPhone}>
                                Add Phone
                            </button>
                        </div>
                        <div style={styles.infoRow}>
                            <strong>Devices:</strong>
                            <ul style={styles.list}>
                                {user.devices.map((device, index) => (
                                    <li key={index}>{device}</li>
                                ))}
                            </ul>
                            <button style={styles.addButton} onClick={addDevice}>
                                Add Device
                            </button>
                        </div>
                    </div>
                </div>

                {/* Right Column */}
                <div style={styles.column}>
                    {/* Select a Device Section */}
                    <div style={styles.card}>
                        <h3>Unlink a Device</h3>
                        <div style={{ display: 'flex', alignItems: 'center', gap: '30px' }}>
                            <select
                                style={styles.select}
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
                                style={styles.removeButton}
                                onClick={() => {
                                    if (window.confirm(`Are you sure you want to remove ${selectedDevice}?`)) {
                                        setUser((prevState) => ({
                                            ...prevState,
                                            devices: prevState.devices.filter((device) => device !== selectedDevice),
                                        }));
                                        setSelectedDevice('');
                                    }
                                }}
                            >
                                Remove
                            </button>
                        </div>
                    </div>
                    {/* Security Settings Section */}
                    <div style={styles.card}>
                        <h3>Security Settings</h3>
                        <p>Control access to Telegram integration.</p>
                        <button
                            style={{
                                ...styles.telegramButton,
                                backgroundColor: user.telegramLinked ? '#28a745' : '#dc3545',
                            }}
                            onClick={toggleTelegramLink}
                        >
                            {user.telegramLinked ? 'Unlink Telegram' : 'Link Telegram'}
                        </button>
                    </div>
                </div>
            </div>

            <Footer />
        </div>
    );
};

const styles = {
    container: {
        width: '100vw',
        fontFamily: 'Arial, sans-serif',
        color: '#333',
        backgroundColor: '#fafafa',
        padding: '20px 0',
    },
    header: {
        display: 'flex',
        marginLeft: '50px',
        paddingTop: '20px',
        alignItems: 'center',
        marginBottom: '20px',
    },
    profilePicture: {
        width: '80px',
        height: '80px',
        borderRadius: '50%',
        backgroundColor: '#ddd',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        marginRight: '20px',
        fontSize: '24px',
    },
    profileIcon: {
        fontSize: '30px',
    },
    username: {
        margin: 0,
        fontSize: '24px',
        fontWeight: 'bold',
    },
    profileTitle: {
        margin: 0,
        fontSize: '16px',
        color: '#777',
    },
    mainContent: {
        display: 'flex',
        flexDirection: 'row',
        margin: '0 50px',
    },
    column: {
        flex: 1,
        marginRight: '20px',
    },
    card: {
        marginBottom: '20px',
        padding: '20px',
        backgroundColor: '#fff',
        borderRadius: '8px',
        boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
    },
    infoRow: {
        marginBottom: '10px',
        padding: '10px 0',
        borderBottom: '1px solid #ddd',
    },
    list: {
        margin: 0,
        padding: 0,
        listStyleType: 'none',
    },
    addButton: {
        marginTop: '10px',
        padding: '8px 12px',
        backgroundColor: '#007bff',
        color: '#fff',
        border: 'none',
        borderRadius: '15px',
        cursor: 'pointer',
    },
    select: {
        width: '100%',
        padding: '10px',
        fontSize: '16px',
        borderRadius: '5px',
        border: '1px solid #ddd',
    },
    telegramButton: {
        width: '100%',
        padding: '10px',
        color: '#fff',
        border: 'none',
        borderRadius: '15px',
        fontWeight: 'bold',
        cursor: 'pointer',
    },
    removeButton: {
        padding: '8px 12px',
        marginRight: '10px',
        backgroundColor: '#dc3545',
        color: '#fff',
        border: 'none',
        borderRadius: '15px',
        fontWeight: 'bold',
        cursor: 'pointer',
    }
};

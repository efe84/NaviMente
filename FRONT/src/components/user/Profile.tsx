import { useState, useEffect } from 'react';
import { useParams } from "react-router-dom";
import Footer from '../layout/Footer';
import image from '../../assets/user.png';
import add from '../../assets/add.png';
import linked from '../../assets/linked.png';
import edit from '../../assets/edit.png';
import deleteIcon from '../../assets/delete.png';
import { useApi } from '../../shared/hooks/useApi';
import { AddPhone, RemovePhone, EditEmail, EditMainPhone, GenerateTelegramCode, GetUser, UnlinkTelegram } from '../../api/authApi';
import { GetDevices, UnassignDevice, RegisterDevice } from '../../api/deviceApi';

type User = {
    userId: number;
    username: string;
    email: string;
    mainPhone: string;
    otherPhones: string[];
    role: number;
    telegramChatId: number;
    devices: Device[];
};

type Device = {
    serialNumber: string;
    name: string;
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
    const [telegramCode, setTelegramCode] = useState<number>();
    const [showTelegramInstructions, setShowTelegramInstructions] = useState(false);
    const [editingEmail, setEditingEmail] = useState(false);
    const [newEmail, setNewEmail] = useState("");
    const [editingMainPhone, setEditingMainPhone] = useState(false);
    const [newMainPhone, setNewMainPhone] = useState("");
    const [showAddDeviceForm, setShowAddDeviceForm] = useState(false);
    const [newDeviceName, setNewDeviceName] = useState("");
    const [newSerialNumber, setNewSerialNumber] = useState("");

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
                    telegramChatId: result.telegramChatId,
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
        if (user != null) {
            callApi(AddPhone(username, newPhone)).then((result: any) => {
                const parsedUser: User = {
                    userId: result.userId,
                    username: result.username,
                    email: result.email,
                    mainPhone: result.mainPhone,
                    otherPhones: result.otherPhones || [],
                    role: result.role,
                    telegramChatId: result.telegramChatId,
                    devices: []
                };
                setUser(parsedUser);
            })
        }
    };

    const removePhone = (phoneNumber: string) => {
        if (user) {
            callApi(RemovePhone(username, phoneNumber)).then((result: any) => {
                const updatedUser: User = {
                    userId: result.userId,
                    username: result.username,
                    email: result.email,
                    mainPhone: result.mainPhone,
                    otherPhones: result.otherPhones || [],
                    role: result.role,
                    telegramChatId: result.telegramChatId,
                    devices: result.devices || []
                };
                setUser(updatedUser);
            }).catch(err => {
                console.error("Error removing phone:", err);
                alert("Failed to remove phone number");
            });
        }
    };

    const registerDevice = () => {
        if (!user) return;

        const payload = {
            DeviceName: newDeviceName.trim(),
            SerialNumber: newSerialNumber.trim(),
            UserId: user.userId
        };

        if (!payload.DeviceName || !payload.SerialNumber) {
            alert("Please fill in all fields.");
            return;
        }

        callApi(RegisterDevice(payload)).then(() => {
            window.location.reload();
        }).catch(() => {
            alert("Error registering device.");
        });
    };

    const editEmail = (newEmail: string) => {
        if (user != null) {
            callApi(EditEmail(username, newEmail)).then(() => {
                window.location.reload();
            });
        }
    };

    const editMainPhone = (newPhone: string) => {
        if (user != null) {
            callApi(EditMainPhone(username, newPhone)).then(() => {
                window.location.reload();
            });
        }
    };

    const unlinkDevice = () => {
        if (user != null) {
            callApi(UnassignDevice(user?.userId, selectedDevice)).then(() => {
                window.location.reload();
            })
        }
    };

    const toggleTelegramLink = async (userId: any) => {
        if (!userId) return;

        if (user?.telegramChatId != null) {
            const confirmed = window.confirm("Are you sure you want to unlink your Telegram account?");
            if (confirmed) {
                callApi(UnlinkTelegram(userId)).then(() => {
                    if (username != null) {
                        callApi(GetUser(username)).then((result: any) => {
                            const updatedUser: User = {
                                userId: result.userId,
                                username: result.username,
                                email: result.email,
                                mainPhone: result.mainPhone,
                                otherPhones: result.otherPhones || [],
                                role: result.role,
                                telegramChatId: result.telegramChatId,
                                devices: user?.devices || []
                            };
                            setUser(updatedUser);
                        });
                    }
                });
            }
        } else {
            callApi(GenerateTelegramCode(userId)).then((result: any) => {
                setTelegramCode(result.code);
                setShowTelegramInstructions(true);
            });
        }
    };

    return (
        <>
            <div style={{ width: '100vw', fontFamily: 'Arial, sans-serif', color: '#333', backgroundColor: '#fafafa', minHeight: 'calc(100vh - 150px)' }}>
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
                            <div style={{ marginBottom: '10px', padding: '10px 0', borderBottom: '1px solid #ddd', display: 'flex', alignItems: 'center' }}>
                                <strong style={{ marginRight: '10px' }}>Email: </strong>
                                {editingEmail ? (
                                    <input
                                        type="text"
                                        value={newEmail}
                                        onChange={(e) => setNewEmail(e.target.value)}
                                        style={{
                                            padding: '5px 10px',
                                            fontSize: '14px',
                                            border: '1px solid #ddd',
                                            borderRadius: '5px',
                                            width: '250px',
                                            marginRight: '10px'
                                        }}
                                    />
                                ) : (
                                    <span style={{ marginRight: '10px' }}>{user?.email}</span>
                                )}
                                <img
                                    src={editingEmail ? linked : edit}
                                    alt={editingEmail ? "Save Email" : "Edit Email"}
                                    style={{ width: '20px', height: '20px', cursor: 'pointer' }}
                                    onClick={() => {
                                        if (editingEmail) {
                                            if (newEmail.trim()) {
                                                editEmail(newEmail);
                                                setEditingEmail(false);
                                            }
                                        } else {
                                            setNewEmail(user?.email || "");
                                            setEditingEmail(true);
                                        }
                                    }}
                                />
                            </div>
                            <div style={{ marginBottom: '10px', padding: '10px 0', borderBottom: '1px solid #ddd', display: 'flex', alignItems: 'center' }}>
                                <strong style={{ marginRight: '10px' }}>Main Phone: </strong>
                                {editingMainPhone ? (
                                    <input
                                        type="text"
                                        value={newMainPhone}
                                        onChange={(e) => setNewMainPhone(e.target.value)}
                                        style={{
                                            padding: '5px 10px',
                                            fontSize: '14px',
                                            border: '1px solid #ddd',
                                            borderRadius: '5px',
                                            width: '200px',
                                            marginRight: '10px'
                                        }}
                                    />
                                ) : (
                                    <span style={{ marginRight: '10px' }}>{user?.mainPhone}</span>
                                )}
                                <img
                                    src={editingMainPhone ? linked : edit}
                                    alt={editingMainPhone ? "Save Main Phone" : "Edit Main Phone"}
                                    style={{ width: '20px', height: '20px', cursor: 'pointer' }}
                                    onClick={() => {
                                        if (editingMainPhone) {
                                            if (newMainPhone.trim()) {
                                                editMainPhone(newMainPhone);
                                            }
                                            setEditingMainPhone(false);
                                        } else {
                                            setNewMainPhone(user?.mainPhone || "");
                                            setEditingMainPhone(true);
                                        }
                                    }}
                                />
                            </div>
                            <div style={{ marginBottom: '10px', padding: '10px 0', borderBottom: '1px solid #ddd' }}>
                                <strong>Additional Phones: </strong>
                                <ul style={{ margin: 0, padding: 0, listStyleType: 'none' }}>
                                    {user?.otherPhones.map((phone, index) => (
                                        <li key={index} style={{ display: 'flex', alignItems: 'center', justifyContent: 'start', marginBottom: '5px' }}>
                                            <span>- {phone}</span>
                                            <img
                                                src={deleteIcon}
                                                alt="Delete Phone"
                                                style={{ width: '16px', height: '16px', cursor: 'pointer', marginLeft: '10px' }}
                                                onClick={() => removePhone(phone)}
                                            />
                                        </li>
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
                                    {user?.devices.map((device) => (
                                        <li key={device.serialNumber}> - {device.name}</li>
                                    ))}
                                </ul>
                                <div style={{ marginTop: "5px" }}>
                                    <img
                                        src={add}
                                        alt="Add Device"
                                        style={{ width: '32px', height: '32px', cursor: 'pointer' }}
                                        onClick={() => setShowAddDeviceForm(!showAddDeviceForm)}
                                    />
                                    {showAddDeviceForm && (
                                        <div style={{ marginTop: '10px' }}>
                                            <input
                                                type="text"
                                                placeholder="Device Name"
                                                value={newDeviceName}
                                                onChange={(e) => setNewDeviceName(e.target.value)}
                                                style={{
                                                    padding: '10px',
                                                    fontSize: '14px',
                                                    border: '1px solid #ddd',
                                                    borderRadius: '8px',
                                                    width: '200px',
                                                    marginRight: '10px'
                                                }}
                                            />
                                            <input
                                                type="text"
                                                placeholder="Serial Number"
                                                value={newSerialNumber}
                                                onChange={(e) => setNewSerialNumber(e.target.value)}
                                                style={{
                                                    padding: '10px',
                                                    fontSize: '14px',
                                                    border: '1px solid #ddd',
                                                    borderRadius: '8px',
                                                    width: '200px',
                                                    marginRight: '10px'
                                                }}
                                            />
                                            <button
                                                onClick={() => {
                                                    registerDevice();
                                                    setShowAddDeviceForm(false);
                                                    setNewDeviceName("");
                                                    setNewSerialNumber("");
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
                                                Save
                                            </button>
                                        </div>
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
                                    {user?.devices.map((device) => (
                                        <option key={device.serialNumber} value={device.serialNumber}>
                                            {device.name}
                                        </option>
                                    ))}
                                </select>
                                <button
                                    onClick={() => {
                                        if (window.confirm(`Are you sure you want to remove the device?`)) {
                                            unlinkDevice();
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
                                onClick={() => toggleTelegramLink(user?.userId)}
                            >
                                {user?.telegramChatId != null ? 'Unlink Telegram' : 'Link Telegram'}
                            </button>
                            {user?.telegramChatId != null ?
                                <img src={linked} style={{ height: "30px", marginLeft: "10px" }} />
                                :
                                <></>
                            }
                        </div>
                        {showTelegramInstructions && telegramCode && (
                            <div style={{
                                backgroundColor: "#f1f1f1",
                                padding: "15px",
                                marginTop: "15px",
                                marginLeft: "15px",
                                borderRadius: "8px",
                                width: "fit-content"
                            }}>
                                <p>
                                    1. Click here to open the Telegram Bot:{" "}
                                    <a href="https://t.me/NaviMente_Bot" target="_blank" rel="noopener noreferrer">
                                        NaviMente BOT
                                    </a>
                                </p>
                                <p>
                                    2. Once open, type this message.
                                </p>
                                <code style={{ background: "#ddd", padding: "6px 10px", borderRadius: "5px", display: "inline-block", fontSize: "16px" }}>
                                    /vincular {telegramCode}
                                </code>
                                <p>
                                    3. After telegram confirms validation, reload the page.
                                </p>
                                <p style={{ marginTop: "10px", fontStyle: "italic", color: "#555" }}>
                                    This codes expires in 10 minutes.
                                </p>
                            </div>
                        )}
                    </div>
                </div>
            </div>
            <Footer />
        </>
    );
};

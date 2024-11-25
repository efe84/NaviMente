import React, { useEffect } from "react";
import { useState } from 'react';
import { useNavigate } from "react-router-dom";
import { useApi } from "../../shared/hooks/useApi";
import { Login, Register } from "../../api/authApi";
import loginImage from '../../assets/login.jpg';

export default function Auth({ isLogin }: { isLogin: boolean }) {
    const callApi = useApi();
    const navigate = useNavigate();
    const [isLoginMode, setIsLoginMode] = useState(isLogin);
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [email, setEmail] = useState('');
    const [phoneNumber, setPhoneNumber] = useState('');
    const [deviceId, setDeviceId] = useState('');

    const onChangeField = (event: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;
        if (name === "username") {
            setUsername(value);
        } else if (name === "password") {
            setPassword(value);

        } else if (name === "email") {
            setEmail(value);

        } else if (name === "phoneNumber") {
            setPhoneNumber(value);
        } else if (name === "deviceId") {
            setDeviceId(value);
        }
    };

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        if (isLoginMode) {
            callApi(Login(username, password)).then(() => {
                window.localStorage.setItem('userName', username);
                navigate('/');
            });
        } else {
            callApi(Register(username, password, email, phoneNumber, deviceId)).then(() => {
                window.localStorage.setItem('userName', username);
                navigate('/');
            });
        }
    };

    return (
        <div style={{ height: "calc(100vh - 70px)", display: "flex" }}>
            <div
                style={{
                    flex: 1,
                    backgroundImage: `url(${loginImage})`,
                    backgroundSize: "cover",
                    backgroundPosition: "center",
                }}
            ></div>

            <div
                style={{
                    flex: 1,
                    backgroundColor: "#fafafa",
                    display: "flex",
                    flexDirection: "column",
                    justifyContent: "center",
                    alignItems: "center",
                    padding: "40px",
                }}
            >
                <h1 style={{ fontSize: "2.3rem", color: "#333" }}>NaviMente</h1>
                <p style={{
                    fontSize: "1.2rem",
                    color: "black",
                    fontWeight: "900",
                    marginBottom: "30px",
                    textAlign: "center",
                }}
                >
                    WELCOME TO THE FUTURE
                </p>
                <form onSubmit={handleSubmit} style={{ width: "100%", maxWidth: "400px" }}>
                    <div style={{ marginBottom: "20px" }}>
                        <label
                            htmlFor="username"
                            style={{ display: "block", fontSize: "0.9rem", marginBottom: "5px" }}
                        >
                            Username
                        </label>
                        <input
                            type="text"
                            id="username"
                            name="username"
                            required
                            style={{
                                width: "100%",
                                padding: "10px",
                                border: "1px solid #ddd",
                                borderRadius: "5px",
                                fontSize: "1rem",
                            }}
                            onChange={onChangeField}
                        />
                    </div>
                    <div style={{ marginBottom: "20px" }}>
                        <label
                            htmlFor="password"
                            style={{ display: "block", fontSize: "0.9rem", marginBottom: "5px" }}
                        >
                            Password
                        </label>
                        <input
                            type="password"
                            id="password"
                            name="password"
                            required
                            style={{
                                width: "100%",
                                padding: "10px",
                                border: "1px solid #ddd",
                                borderRadius: "5px",
                                fontSize: "1rem",
                            }}
                            onChange={onChangeField}
                        />
                    </div>

                    {!isLoginMode && (
                        <>
                            <div style={{ marginBottom: "20px" }}>
                                <label
                                    htmlFor="email"
                                    style={{ display: "block", fontSize: "0.9rem", marginBottom: "5px" }}
                                >
                                    Email
                                </label>
                                <input
                                    type="email"
                                    id="email"
                                    name="email"
                                    required
                                    style={{
                                        width: "100%",
                                        padding: "10px",
                                        border: "1px solid #ddd",
                                        borderRadius: "5px",
                                        fontSize: "1rem",
                                    }}
                                    onChange={onChangeField}
                                />
                            </div>
                            <div style={{ marginBottom: "20px" }}>
                                <label
                                    htmlFor="phoneNumber"
                                    style={{ display: "block", fontSize: "0.9rem", marginBottom: "5px" }}
                                >
                                    Phone Number
                                </label>
                                <input
                                    type="tel"
                                    id="phoneNumber"
                                    name="phoneNumber"
                                    style={{
                                        width: "100%",
                                        padding: "10px",
                                        border: "1px solid #ddd",
                                        borderRadius: "5px",
                                        fontSize: "1rem",
                                    }}
                                    onChange={onChangeField}
                                />
                            </div>
                            <div style={{ marginBottom: "20px" }}>
                                <label
                                    htmlFor="deviceId"
                                    style={{ display: "block", fontSize: "0.9rem", marginBottom: "5px" }}
                                >
                                    Device ID
                                </label>
                                <input
                                    type="text"
                                    id="deviceId"
                                    name="deviceId"
                                    style={{
                                        width: "100%",
                                        padding: "10px",
                                        border: "1px solid #ddd",
                                        borderRadius: "5px",
                                        fontSize: "1rem",
                                    }}
                                    onChange={onChangeField}
                                />
                            </div>
                        </>
                    )}

                    <div style={{ marginBottom: "20px", display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                        <span style={{ fontSize: "0.9rem", color: "black" }}>Remember</span>

                        <div className="form-check form-switch">
                            <input
                                className="form-check-input"
                                type="checkbox"
                                id="rememberSwitch"
                                style={{ marginRight: 0 }}
                            />
                        </div>
                    </div>
                    <button
                        type="submit"
                        style={{
                            width: "100%",
                            padding: "10px",
                            backgroundColor: "black",
                            color: "#fff",
                            border: "none",
                            borderRadius: "5px",
                            fontSize: "1rem",
                            cursor: "pointer",
                        }}
                    >
                        {isLoginMode ? "Login" : "Register"}
                    </button>
                </form>

                <div style={{ marginTop: "20px" }}>
                    <span style={{ fontSize: "0.9rem", color: "black" }}>
                        {isLoginMode ? "Aún no estás registrado?" : "Ya tienes una cuenta?"}
                    </span>
                    <a
                        href="#"
                        onClick={(e) => { e.preventDefault(); setIsLoginMode(!isLoginMode); }}
                        style={{ color: "blue", cursor: "pointer", marginLeft: "5px", fontSize: "15px" }}
                    >
                        {isLoginMode ? "Regístrate" : "Inicia sesión"}
                    </a>
                </div>
            </div>
        </div>
    );
}
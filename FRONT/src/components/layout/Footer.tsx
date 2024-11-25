import React from 'react';
import faceIcon from '../../assets/facebook.png';
import InstaIcon from '../../assets/instagram.png';
import XIcon from '../../assets/x.png';


export default function Footer() {

    return (
        <div>
            <footer className="d-flex justify-content-between align-items-center px-4 py-3" style={{ backgroundColor: "#fff", borderTop: "1px solid #ddd" }}>

                <div className="w-100" style={{ fontSize: "0.8rem", color: "#888" }}>
                    <p className="mb-1">© 2025 NaviMente</p>
                    <p className="mb-1">All rights reserved</p>
                </div>

                <div className="d-flex align-items-center" style={{ fontSize: "0.8rem", gap: "15px" }}>
                    <a href="https://facebook.com" className="mr-2" style={{ color: "#888" }}><img style={{ height: '20px', width: '20px' }} src={faceIcon} /></a>
                    <a href="https://x.com" className="mr-2" style={{ color: "#888" }}><img style={{ height: '20px', width: '20px' }} src={XIcon} /></a>
                    <a href="https://instagram.com" className="mr-2" style={{ color: "#888" }}><img style={{ height: '20px', width: '20px' }} src={InstaIcon} /></a>
                </div>
            </footer>
        </div>
    )
}
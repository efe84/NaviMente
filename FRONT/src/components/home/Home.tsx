import React from "react";
import mapImage from "../../assets/logo.png";
import chatIcon from '../../assets/chat.png';
import mapIcon from '../../assets/map.png';
import faceIcon from '../../assets/facebook.png';
import InstaIcon from '../../assets/instagram.png';
import XIcon from '../../assets/x.png';

export default function Home() {
  
  return (
    <>
      <div style={{ backgroundColor: "#fafafa", minHeight: "90vh" }}>

        <div className="text-center mt-5">
         <h1>Welcome to NaviMente</h1>
          <p style={{ fontSize: "1.2rem", color: "#555", marginTop: '20px' }}>
            Discover a new way to help people with dementia through a GPS device<br/>Join the protection now!
          </p>
          <div>
            <button className="btn btn-dark mx-2">Login</button>
            <button className="btn btn-outline-dark mx-2">Register</button>
          </div>
        </div>

        <div className="text-center">
          <img src={mapImage} alt="Map" style={{ maxWidth: "380px", maxHeight: "380px" }} />
        </div>

        <div className="d-flex justify-content-center mb-5">

          <div className="card mx-3 p-2" style={{ width: "400px", boxShadow: "0px 4px 10px rgba(0, 0, 0, 0.1)" }}>
            <div className="card-body text-center">
              <img src={mapIcon} alt="Map Icon" style={{ width: "40px", height: "40px"}} />
              <h5 className="card-title">
                <br />
                Find Your Device
              </h5>
              <p className="card-text text-muted">
                Access to a map where you can follow your devices and filter the last moves by time.
              </p>
            </div>
          </div>
        
          <div className="card mx-3 p-2" style={{ width: "400px", boxShadow: "0px 4px 10px rgba(0, 0, 0, 0.1)" }}>
            <div className="card-body text-center">
              <img src={chatIcon} alt="Chat Icon" style={{ width: "40px", height: "40px"}} />
              <h5 className="card-title">
                <br />
                Chat With Telegram Bot
              </h5>
              <p className="card-text text-muted">
                Communicate with a chatbot in real-time to get information about a device conected to your account.
              </p>
            </div>
          </div>

        </div>

      </div>

      <div>

        <footer className="d-flex justify-content-between align-items-center px-4 py-3" style={{ backgroundColor: "#fff", borderTop: "1px solid #ddd" }}>

          <div className="w-100" style={{ fontSize: "0.8rem", color: "#888" }}>
            <p className="mb-1">© 2025 NaviMente</p>
            <p className="mb-1">All rights reserved</p>
          </div>
        
          <div className="d-flex align-items-center" style={{ fontSize: "0.8rem", gap: "15px" }}>
            <a href="https://facebook.com" className="mr-2" style={{ color: "#888" }}><img style={{height: '20px', width: '20px'}} src={faceIcon}/></a>
            <a href="https://x.com" className="mr-2" style={{ color: "#888" }}><img style={{height: '20px', width: '20px'}} src={XIcon}/></a>
            <a href="https://instagram.com" className="mr-2" style={{ color: "#888" }}><img style={{height: '20px', width: '20px'}} src={InstaIcon}/></a>
         </div>

        </footer>

      </div>
    </>
  );
  }
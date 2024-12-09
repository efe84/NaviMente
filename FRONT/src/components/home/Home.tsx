import React from "react";
import mapImage from "../../assets/logo.png";
import chatIcon from '../../assets/chat.png';
import mapIcon from '../../assets/map.png';
import faceIcon from '../../assets/facebook.png';
import InstaIcon from '../../assets/instagram.png';
import XIcon from '../../assets/x.png';
import { useNavigate } from "react-router-dom";
import Footer from "../layout/Footer";

export default function Home() {
  const navigate = useNavigate();
  const username = localStorage.getItem('userName');

  return (
    <>
      <div style={{ backgroundColor: "#fafafa", minHeight: "calc(100vh - 70px)" }}>

        <div className="text-center pt-5">
          <h1>Welcome to NaviMente</h1>
          <p style={{ fontSize: "1.2rem", color: "#555", marginTop: '20px' }}>
            Discover a new way to help people with dementia through a GPS device<br />Join the protection now!
          </p>
          {username == null &&
            <div>
              <button onClick={() => navigate('/Login')} className="btn btn-dark mx-2">Login</button>
              <button onClick={() => navigate('/Register')} className="btn btn-outline-dark mx-2">Register</button>
            </div>
          }
        </div>

        <div className="text-center">
          <img src={mapImage} alt="Map" style={{ maxWidth: "380px", maxHeight: "380px" }} />
        </div>

        <div className="d-flex justify-content-center mb-5">

          <div className="card mx-3 p-2" style={{ width: "400px", boxShadow: "0px 4px 10px rgba(0, 0, 0, 0.1)", cursor: 'pointer' }} onClick={ () => navigate("/Map") }>
            <div className="card-body text-center">
              <img src={mapIcon} alt="Map Icon" style={{ width: "40px", height: "40px" }} />
              <h5 className="card-title">
                <br />
                Find Your Device
              </h5>
              <p className="card-text text-muted">
                Access to a map where you can follow your devices and filter the last moves by time.
              </p>
            </div>
          </div>

          <div className="card mx-3 p-2" style={{ width: "400px", boxShadow: "0px 4px 10px rgba(0, 0, 0, 0.1)", cursor: 'pointer' }} onClick={ () => navigate("/Chat") }>
            <div className="card-body text-center">
              <img src={chatIcon} alt="Chat Icon" style={{ width: "40px", height: "40px" }} />
              <h5 className="card-title">
                <br />
                Check last moves / Telegram Bot
              </h5>
              <p className="card-text text-muted">
                Check the last information about your devices. You can also try our new Telegram Bot to ask any doubt.
              </p>
            </div>
          </div>

        </div>
        <Footer/>
      </div>


    </>
  );
}
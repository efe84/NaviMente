import React from "react";
import { useNavigate } from "react-router-dom";
import logo from '../../assets/logoNavBar.png';

export default function Header({ navigateTo }) {
  const navigate = useNavigate();

  return (
    <nav className="navbar fixed-top d-flex align-items-center px-3" style={{ height: '70px', backgroundColor: 'white', boxShadow: '0px 4px 8px rgba(0, 0, 0, 0.1)' }}>
      <div className="d-flex align-items-center">
        <img src={logo} alt="logo" className="img-fluid" style={{width: '60px', height: '50px'}}/>
        <label style={{marginLeft: '10px', marginTop: '10px'}}>NaviMente</label>
      </div>

      <div style={{ justifyContent: 'flex-end' }}>
        <button
          className="btn btn-link"
          style={{ textDecoration: 'none', marginRight: '10px' }}
        >
          <p className="m-0 text-dark">Home</p>
        </button>

        <button
          className="btn btn-link"
          style={{ textDecoration: 'none', marginRight: '10px'}}
        >
          <p className="m-0 text-dark">Map</p>
        </button>

        <button
          className="btn btn-link text-white ml-100"
          style={{ textDecoration: 'none', marginRight: '10px' }}
        >
          <p className="m-0 text-dark">Chat</p>
        </button>

        <button
          onClick={() => navigate('/login')}
          className="btn btn-link text-white"
          style={{ textDecoration: 'none', marginRight: '10px'}}
          aria-label="logout"
        >
          <p className="m-0 text-dark">Login</p>
        </button>
      </div>
      
    </nav>
  );
}
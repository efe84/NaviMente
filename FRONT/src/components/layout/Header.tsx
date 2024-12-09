import React from "react";
import { useNavigate } from "react-router-dom";
import logo from '../../assets/logoNavBar.png';
import { useApi } from "../../shared/hooks/useApi";
import { Logout } from "../../api/authApi";

export default function Header({navigateTo}) {
  const callApi = useApi();
  const username = localStorage.getItem("userName");

  const handleLogout = (event: any) => {
    event.preventDefault();
    if (username != null) {
      callApi(Logout(username)).then(() => {
        window.localStorage.removeItem('userName');
        navigateTo('/');
      });
    }
  };

  return (
    <nav className="navbar fixed-top d-flex align-items-center px-3" style={{ height: '70px', backgroundColor: 'white', boxShadow: '0px 4px 8px rgba(0, 0, 0, 0.1)' }}>
      <div className="d-flex align-items-center">
        <img onClick={() => navigateTo('/')} src={logo} alt="logo" className="img-fluid" style={{ width: '60px', height: '50px' }} />
        <label onClick={() => navigateTo('/')} style={{ marginLeft: '10px', marginTop: '10px' }}>NaviMente</label>
      </div>

      <div style={{ justifyContent: 'flex-end' }}>
        <button
          onClick={() => navigateTo("/")}
          className="btn btn-link"
          style={{ textDecoration: 'none', marginRight: '10px' }}
        >
          <p className="m-0 text-dark">Home</p>
        </button>

        <button
          onClick={() => navigateTo("Map")}
          className="btn btn-link"
          style={{ textDecoration: 'none', marginRight: '10px' }}
        >
          <p className="m-0 text-dark">Map</p>
        </button>

        <button
          className="btn btn-link text-white ml-100"
          style={{ textDecoration: 'none', marginRight: '10px' }}
        >
          <p className="m-0 text-dark">Binnacle</p>
        </button>



        {username == null ? (
          <button
            onClick={() => navigateTo('/login')}
            className="btn btn-link text-white"
            style={{ textDecoration: 'none', marginRight: '10px' }}
            aria-label="login"
          >
            <p className="m-0 text-dark">Login</p>
          </button>
        ) : (<>
          <button
            className="btn btn-link text-white ml-100"
            style={{ textDecoration: 'none', marginRight: '10px' }}
          >
            <p className="m-0 text-dark">Profile</p>
          </button>

          <button
            onClick={handleLogout}
            className="btn btn-link text-white"
            style={{ textDecoration: 'none', marginRight: '10px' }}
            aria-label="login"
          >
            <p className="m-0 text-dark">Logout</p>
          </button>
        </>)}

      </div>

    </nav>
  );
}
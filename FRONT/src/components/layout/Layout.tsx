import React, { useCallback, useState } from "react";
import { Outlet, useNavigate } from "react-router-dom";
import Header from "./Header";

function Layout() {
    const navigate = useNavigate();

    const navigateTo = useCallback(function navigateTo(url) {
        navigate(url);
    }, [navigate]);

    return (
        <div style={{ display: 'flex', height: '100%' }}>
          <Header navigateTo={navigateTo} />
          
          <div
            style={{
              flexGrow: 1,
              margin: 0,
              marginTop: '70px',
              overflow: 'hidden'
            }}
          >
            <Outlet />
          </div>
        </div>
      );
}

export default Layout;
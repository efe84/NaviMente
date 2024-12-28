import React from 'react';
import { Route, Routes } from 'react-router-dom';
import './App.css'
import "./i18n"
import Layout from './components/layout/Layout.tsx'
import Home from './components/home/Home.tsx'
import Auth from './components/user/auth.tsx';
import ContextProvider from './shared/contexts/ContextProvider.tsx';
import Map from './components/map/Map.tsx'
import Profile from './components/user/Profile.tsx';
import Binnacle from './components/binnacle/Binnacle.tsx';

export default function App() {

  return (
    <ContextProvider>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route path="" element={<Home />} />
          <Route path="Register" element={<Auth isLogin={false} />} />
          <Route path="Login" element={<Auth isLogin={true} />} />
          <Route path="Map" element={<Map />} />
          <Route path="Profile" element={<Profile />} />
          <Route path="Binnacle" element={<Binnacle />} />
        </Route>
      </Routes>
    </ContextProvider>
  )
}

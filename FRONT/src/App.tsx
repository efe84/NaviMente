import React from 'react';
import { Route, Routes } from 'react-router-dom';
import './App.css'
import "./i18n"
import Layout from './components/layout/Layout.jsx'
import Home from './components/home/Home.jsx'

export default function App() {

  return (
    <Routes>
      <Route path="/" element={<Layout/>}>
        <Route path="" element={<Home />} />
      </Route>
    </Routes>
  )
}

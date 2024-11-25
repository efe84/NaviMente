import React, { useState } from 'react';
import { GoogleMap, LoadScript, Marker } from '@react-google-maps/api';
import Footer from '../layout/Footer';

const center = {
  lat: -34.397,
  lng: 150.644,
};

const Map: React.FC = () => {
  const apiKey = 'AIzaSyCXyVi8y7BTQSkqD3oKi8Jt3C97m1wZB8g';
  const [markers, setMarkers] = useState<{ lat: number; lng: number }[]>([center]);

  const containerStyle = {
    width: '100%',
    minHeight: 'calc(100vh - 150px)',
  };

  const mapStyle = {
    width: '80%',
    height: '100%',
  };

  const menuStyle = {
    width: '20%',
    padding: '20px',
    backgroundColor: '#f9f9f9',
    boxShadow: '0 0 10px rgba(0, 0, 0, 0.1)'
  };

  const addMarker = (lat: number, lng: number) => {
    setMarkers([...markers, { lat, lng }]);
  };

  const bands = [
    { id: '123456', lastUpdate: '5 mins ago' },
    { id: '789012', lastUpdate: '10 mins ago' },
    { id: '345678', lastUpdate: '15 mins ago' },
  ];

  return (
    <>
      <div style={{ display: 'flex', minHeight: 'calc(100vh - 150px)' }}>
        <div style={mapStyle}>
          <LoadScript googleMapsApiKey={apiKey} libraries={['places']}>
            <GoogleMap mapContainerStyle={containerStyle} center={center} zoom={8}>
              {markers.map((marker, index) => (
                <Marker key={index} position={marker} />
              ))}
            </GoogleMap>
          </LoadScript>
        </div>

        <div 
          style={{ width: '20%',
                    padding: '20px',
                    backgroundColor: '#f9f9f9',
                    boxShadow: '0 0 10px rgba(0, 0, 0, 0.1)' 
          }}>
          <h3 style={{ textAlign: 'center' }}><b>NaviBand Details</b></h3>
          {bands.map((band) => (
            <div
              key={band.id}
              style={{
                marginBottom: '15px',
                padding: '15px',
                backgroundColor: '#fff',
                borderRadius: '8px',
                boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
              }}
            >
              <p>
                <strong>Band ID:</strong> {band.id}
              </p>
              <p>
                <strong>Last Update:</strong> {band.lastUpdate}
              </p>
              <button
                style={{
                  padding: '10px 15px',
                  backgroundColor: '#000',
                  color: '#fff',
                  border: 'none',
                  borderRadius: '5px',
                  cursor: 'pointer',
                  width: '100%',
                  fontWeight: 'bold',
                }}
              >
                View Details
              </button>
            </div>
          ))}
        </div>
      </div>

      <Footer />
    </>
  );
};

export default Map;
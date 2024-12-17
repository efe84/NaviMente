import React, { useEffect, useState } from 'react';
import { GoogleMap, LoadScript, Marker, useLoadScript } from '@react-google-maps/api';
import Footer from '../layout/Footer';
import { useApi } from '../../shared/hooks/useApi';
import { List } from '../../api/deviceApi';

const center = {
  lat: -34.397,
  lng: 150.644,
};

const Map: React.FC = () => {
  const callApi = useApi();
  const apiKey = 'AIzaSyCXyVi8y7BTQSkqD3oKi8Jt3C97m1wZB8g';
  const [markers, setMarkers] = useState<{ lat: number; lng: number }[]>([center]);
  const [devices, setDevices] = useState<{ deviceName: string; lastUpdate: string }[]>([]);
  const userName = localStorage.getItem('userName');

  useEffect(() => {
    if (userName) {
      callApi(List(userName)).then((response: any) => {
        setDevices(response);
      });
    }
  }, [userName]);

  const containerStyle = {
    width: '100%',
    minHeight: 'calc(100vh - 150px)',
  };

  const mapStyle = {
    width: '80%',
    height: '100%',
  };

  function formatDate(dateString: string): string {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');
  
    return `${day}-${month}-${year} ${hours}:${minutes}:${seconds}`;
  }

  const addMarker = (lat: number, lng: number) => {
    setMarkers([...markers, { lat, lng }]);
  };

  const { isLoaded, loadError } = useLoadScript({
    googleMapsApiKey: apiKey,
    libraries: ['places'],
  });

  if (loadError) {
    return <div>Error loading maps</div>;
  }

  if (!isLoaded) {
    return <div>Loading...</div>;
  }

  return (
    <>
      <div style={{ display: 'flex', minHeight: 'calc(100vh - 150px)' }}>
        <div style={mapStyle}>
          <GoogleMap mapContainerStyle={containerStyle} center={center} zoom={8}>
            {markers.map((marker, index) => (
              <Marker key={index} position={marker} />
            ))}
          </GoogleMap>
        </div>

        <div style={{
          width: '20%',
          padding: '20px',
          backgroundColor: '#f9f9f9',
          boxShadow: '0 0 10px rgba(0, 0, 0, 0.1)'
        }}>
          <h3 style={{ textAlign: 'center' }}><b>NaviBand Details</b></h3>
          {devices.map((band) => (
            <div
              key={band.deviceName}
              style={{
                marginBottom: '15px',
                padding: '15px',
                backgroundColor: '#fff',
                borderRadius: '8px',
                boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
              }}
            >
              <p>
                <strong>Band:</strong> {band.deviceName}
              </p>
              <p>
                <strong>Last Update:</strong> {band.lastUpdate ? formatDate(band.lastUpdate) : "Not used"}
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
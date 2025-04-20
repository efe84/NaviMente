import React, { useEffect, useState } from 'react';
import { GoogleMap, useLoadScript, Circle, Rectangle, Polygon, Marker } from '@react-google-maps/api';
import Footer from '../layout/Footer';
import { useApi } from '../../shared/hooks/useApi';
import { BlockZone, GetDevices, Zones } from '../../api/deviceApi';
import { SearchLastLocation, SearchRoute } from '../../api/locationApi';

const center = {
  lat: 43.212625,
  lng: -8.691061,
};

type Shape =
  | { type: 'circle'; center: google.maps.LatLngLiteral; radius: number }
  | { type: 'rectangle'; bounds: google.maps.LatLngBoundsLiteral }
  | { type: 'polygon'; path: google.maps.LatLngLiteral[] };

const Map: React.FC = () => {
  const callApi = useApi();
  const [devices, setDevices] = useState<{ deviceName: string; lastUpdate: string }[]>([]);
  const [openDetailsDevice, setOpenDetailsDevice] = useState<string | null>(null);
  const [drawMode, setDrawMode] = useState<'circle' | 'rectangle' | 'free' | null>(null);
  const [freeDrawPoints, setFreeDrawPoints] = useState<google.maps.LatLngLiteral[]>([]);
  const [shapes, setShapes] = useState<Shape[]>([]);
  const [zones, setZones] = useState<any[]>([]);

  const apiKey = 'AIzaSyAaI8czgtGYcqc046Vv-icjEsKmCVcLcb0';
  const userName = localStorage.getItem('userName');

  useEffect(() => {
    if (userName) {
      callApi(GetDevices("1")).then((response: any) => {
        setDevices(response);
      });
      // callApi(Zones("B2412021V1")).then((response: any) => {
      //   setZones(response);
      // });
    }
  }, [userName]);

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

  const handleMapClick = (e: google.maps.MapMouseEvent) => {
    if (!e.latLng) return;

    const latLng = {
      lat: e.latLng.lat(),
      lng: e.latLng.lng(),
    };

    if (drawMode === 'circle') {
      const circle = (
        <Circle
          key={Date.now()}
          center={latLng}
          radius={100}
          options={{
            fillColor: '#00f',
            fillOpacity: 0.2,
            strokeColor: '#00f',
            strokeOpacity: 0.5,
            strokeWeight: 2,
            editable: true
          }}
        />
      );
      setShapes(prev => [...prev, { type: 'circle', center: latLng, radius: 100 }]);
    }

    if (drawMode === 'rectangle') {
      const bounds = {
        north: latLng.lat + 0.0005,
        south: latLng.lat - 0.0005,
        east: latLng.lng + 0.0005,
        west: latLng.lng - 0.0005,
      };

      const rectangle = (
        <Rectangle
          key={Date.now()}
          bounds={bounds}
          options={{
            fillColor: '#0a0',
            fillOpacity: 0.2,
            strokeColor: '#0a0',
            strokeOpacity: 0.5,
            strokeWeight: 2,
            editable: true
          }}
        />
      );
      setShapes(prev => [...prev, { type: 'rectangle', bounds }]);
    }

    if (drawMode === 'free' && freeDrawPoints.length < 4) {
      setFreeDrawPoints((prev) => [...prev, latLng]);
    }
  };

  const renderFreeDrawPolygon = () => {
    if (freeDrawPoints.length === 4) {
      const polygon = (
        <Polygon
          key={Date.now()}
          path={freeDrawPoints}
          options={{
            fillColor: '#f00',
            fillOpacity: 0.2,
            strokeColor: '#f00',
            strokeOpacity: 0.6,
            strokeWeight: 2,
            editable: true,
          }}
        />
      );

      setShapes(prev => [...prev, { type: 'polygon', path: freeDrawPoints }]);
      setFreeDrawPoints([]);
    }
    return null;
  };

  const searchRoute = (serialNumber, startDate, endDate) => {
    if (!startDate || !endDate) {
      alert("Please select both start and end dates.");
      return;
    }

    callApi(SearchRoute(serialNumber, startDate, endDate)).then(() => {
      alert("Ruta recuperada");
    });
  };

  const lastLocation = (serialNumber) => {
    if (!serialNumber) {
      alert("No serialNumber selected");
      return;
    }

    callApi(SearchLastLocation(serialNumber)).then(() => {
    });
  };

  const saveZones = async (serialNumber: string) => {
    const geoJsonShapes = shapes.map((shape) => {
      switch (shape.type) {
        case 'circle': {
          const steps = 32;
          const coords: number[][] = [];

          for (let i = 0; i <= steps; i++) {
            const angle = (i / steps) * 2 * Math.PI;
            const dx = shape.radius * Math.cos(angle) / 111320;
            const dy = shape.radius * Math.sin(angle) / 111320;
            coords.push([
              shape.center.lng + dx,
              shape.center.lat + dy,
            ]);
          }

          return {
            type: 'Polygon',
            coordinates: [coords],
          };
        }

        case 'rectangle': {
          const { north, south, east, west } = shape.bounds;
          return {
            type: 'Polygon',
            coordinates: [[
              [west, north],
              [east, north],
              [east, south],
              [west, south],
              [west, north],
            ]],
          };
        }

        case 'polygon': {
          const coordinates = shape.path.map(p => [p.lng, p.lat]);
          coordinates.push(coordinates[0]);
          return {
            type: 'Polygon',
            coordinates: [coordinates],
          };
        }

        default:
          return null;
      }
    });

    const payload = {
      serialNumber,
      shapes: geoJsonShapes
    };

    callApi(BlockZone(payload)).then(() => {
      alert('Zona bloqueada guardada');
      resetFigure();
    });
  };

  const resetFigure = () => {
    setShapes([]);
    setFreeDrawPoints([]);
  };

  if (loadError) return <div>Error loading maps</div>;
  if (!isLoaded) return <div>Loading...</div>;

  return (
    <>
      <div style={{ display: 'flex', minHeight: 'calc(100vh - 150px)' }}>
        <div style={{ width: '80%', height: '100%' }}>
          <GoogleMap
            mapContainerStyle={{ width: '100%', minHeight: 'calc(100vh - 150px)' }}
            center={center}
            zoom={17}
            onClick={handleMapClick}
          >
            {zones.map(zone =>
              zone.shapes.map((shape: any, index: number) => (
                <Polygon
                  key={index}
                  paths={shape.coordinates[0].map((coord: any) => ({
                    lat: coord.Latitude,
                    lng: coord.Longitude,
                  }))}
                  options={{
                    fillColor: '#f00',
                    fillOpacity: 0.2,
                    strokeColor: '#f00',
                    strokeOpacity: 0.6,
                    strokeWeight: 2,
                  }}
                />
              ))
            )}

            {shapes.map((shape, idx) => {
              switch (shape.type) {
                case 'circle':
                  return (
                    <Circle
                      key={idx}
                      center={shape.center}
                      radius={shape.radius}
                      options={{
                        fillColor: '#00f',
                        fillOpacity: 0.2,
                        strokeColor: '#00f',
                        strokeOpacity: 0.5,
                        strokeWeight: 2,
                        editable: true
                      }}
                    />
                  );
                case 'rectangle':
                  return (
                    <Rectangle
                      key={idx}
                      bounds={shape.bounds}
                      options={{
                        fillColor: '#0a0',
                        fillOpacity: 0.2,
                        strokeColor: '#0a0',
                        strokeOpacity: 0.5,
                        strokeWeight: 2,
                        editable: true
                      }}
                    />
                  );
                case 'polygon':
                  return (
                    <Polygon
                      key={idx}
                      path={shape.path}
                      options={{
                        fillColor: '#f00',
                        fillOpacity: 0.2,
                        strokeColor: '#f00',
                        strokeOpacity: 0.6,
                        strokeWeight: 2,
                        editable: true
                      }}
                    />
                  );
                default:
                  return null;
              }
            })}

            {freeDrawPoints.map((point, index) => (
              <Marker
                key={`marker-${index}`}
                position={point}
                icon={{
                  path: google.maps.SymbolPath.CIRCLE,
                  scale: 5,
                  fillColor: '#f00',
                  fillOpacity: 1,
                  strokeWeight: 0,
                }}
              />
            ))}

            {renderFreeDrawPolygon()}

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
                onClick={() =>
                  setOpenDetailsDevice((prev) => (prev === band.deviceName ? null : band.deviceName))
                }
                className="btn w-100 bg-dark text-white"
              >
                Search Locations
              </button>

              {openDetailsDevice === band.deviceName && (
                <div style={{ borderTop: '1px solid #ccc', paddingTop: '10px' }}>
                  <div className="mb-2">
                    <label htmlFor={`start-${band.deviceName}`} className="form-label">From:</label>
                    <input type="datetime-local" id={`start-${band.deviceName}`} className="form-control" />
                  </div>

                  <div className="mb-3">
                    <label htmlFor={`end-${band.deviceName}`} className="form-label">To:</label>
                    <input type="datetime-local" id={`end-${band.deviceName}`} className="form-control" />
                  </div>

                  <button className="btn btn-dark w-100 mb-2" onClick={() =>
                    searchRoute(
                      "B2412021V1",
                      (document.getElementById(`start-${band.deviceName}`) as HTMLInputElement).value,
                      (document.getElementById(`end-${band.deviceName}`) as HTMLInputElement).value
                    )
                  }>Search</button>
                  <button className="btn btn-outline-dark w-100" onClick={() =>lastLocation("B2412021V1")}>See last location</button>
                </div>
              )}

              <div style={{ display: 'flex', gap: '10px', marginTop: '10px' }}>
                <select
                  id="shape-select"
                  className="form-select"
                  value={drawMode || ''}
                  onChange={(e) => {
                    const value = e.target.value;
                    setDrawMode(value === '' ? null : value as 'circle' | 'rectangle' | 'free');
                    setFreeDrawPoints([]);
                  }}
                >
                  <option value="">Select figure</option>
                  <option value="circle">Circle</option>
                  <option value="rectangle">Rectangle</option>
                  <option value="free">Free figure</option>
                </select>
              </div>

              <div style={{ display: 'flex', gap: '10px', marginTop: '10px' }}>
                <button
                  className="btn w-50 bg-dark text-white"
                  onClick={() => saveZones(band.deviceName)}
                >
                  Block zone
                </button>

                <button
                  className="btn btn-outline-dark w-50"
                  onClick={resetFigure}
                >
                  Reset figure
                </button>
              </div>

            </div>
          ))}

        </div>
      </div>

      <Footer />
    </>
  );
};

export default Map;
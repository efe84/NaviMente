import React, { useEffect, useState } from 'react';
import { GoogleMap, useLoadScript, Circle, Rectangle, Polygon, Marker, DirectionsRenderer } from '@react-google-maps/api';
import Footer from '../layout/Footer';
import { useApi } from '../../shared/hooks/useApi';
import { BlockZone, GetDevices, Zones, DeleteZone } from '../../api/deviceApi';
import { SearchLastLocation, SearchRoute } from '../../api/locationApi';

const center = {
  lat: 43.212625,
  lng: -8.691061,
};

type Shape =
  | { type: 'circle'; center: google.maps.LatLngLiteral; radius: number }
  | { type: 'rectangle'; bounds: google.maps.LatLngBoundsLiteral }
  | { type: 'polygon'; path: google.maps.LatLngLiteral[] };

type ShapeDTO =
  | { type: 'circle'; center: google.maps.LatLngLiteral; radius: number; zoneId: any }
  | { type: 'rectangle'; bounds: google.maps.LatLngBoundsLiteral; zoneId: any }
  | { type: 'polygon'; path: google.maps.LatLngLiteral[]; zoneId: any };

const Map: React.FC = () => {
  const callApi = useApi();
  const [devices, setDevices] = useState<{ serialNumber: string; name: string; lastUpdate: string }[]>([]);
  const [openDetailsDevice, setOpenDetailsDevice] = useState<string | null>(null);
  const [drawModes, setDrawModes] = useState<{ [deviceName: string]: 'circle' | 'rectangle' | 'free' | null }>({});
  const [freeDrawPoints, setFreeDrawPoints] = useState<google.maps.LatLngLiteral[]>([]);
  const [shapes, setShapes] = useState<Shape[]>([]);
  const [activeDevice, setActiveDevice] = useState<string | null>(null);
  const mapRef = React.useRef<google.maps.Map | null>(null);
  const [lastPositionMarker, setLastPositionMarker] = useState<google.maps.LatLngLiteral | null>(null);
  const [directions, setDirections] = useState<google.maps.DirectionsResult | null>(null);
  const [existingShapes, setExistingShapes] = useState<ShapeDTO[]>([]);

  const apiKey = 'AIzaSyAaI8czgtGYcqc046Vv-icjEsKmCVcLcb0';
  const userName = localStorage.getItem('userName');

  useEffect(() => {
    if (userName) {
      callApi(GetDevices("1")).then((response: any) => {
        setDevices(response);
      });
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

  /* istanbul ignore next */
  if (loadError) {
    return <div>Error loading maps</div>;
  }

  /* istanbul ignore next */
  if (!isLoaded) {
    return <div>Loading...</div>;
  }

  /* istanbul ignore next */
  const onMapLoad = (map: google.maps.Map) => {
    mapRef.current = map;
  };

  /* istanbul ignore next */
  const loadRestrictedZones = (serialNumber: string) => {
    callApi(Zones(serialNumber)).then((response: any) => {
      if (response && Array.isArray(response)) {
        const parsedShapes: ShapeDTO[] = [];

        response.forEach((zone) => {
          if (!zone.shape) return;

          switch (zone.shape.type) {
            case 'circle':
              if (zone.shape.center && zone.shape.radius) {
                parsedShapes.push({
                  type: 'circle',
                  center: { lat: zone.shape.center[1], lng: zone.shape.center[0] },
                  radius: zone.shape.radius,
                  zoneId: zone.zoneId
                });
              }
              break;

            case 'rectangle':
              if (zone.shape.bounds) {
                parsedShapes.push({
                  type: 'rectangle',
                  bounds: {
                    north: zone.shape.bounds.north,
                    south: zone.shape.bounds.south,
                    east: zone.shape.bounds.east,
                    west: zone.shape.bounds.west,
                  },
                  zoneId: zone.zoneId
                });
              }
              break;

            case 'polygon':
              if (zone.shape.coordinates) {
                const path = zone.shape.coordinates.map((coord: number[]) => ({
                  lat: coord[1],
                  lng: coord[0],
                }));

                parsedShapes.push({
                  type: 'polygon',
                  path,
                  zoneId: zone.zoneId
                });
              }
              break;

            default:
              console.warn('Unknown shape type:', zone.shape.type);
          }
        });

        setExistingShapes(parsedShapes);
      }
    });
  };

  /* istanbul ignore next */
  const handleMapClick = (e: google.maps.MapMouseEvent) => {
    if (!e.latLng || !activeDevice) return;

    const drawMode = drawModes[activeDevice];
    if (!drawMode) return;

    const latLng = {
      lat: e.latLng.lat(),
      lng: e.latLng.lng(),
    };

    if (drawMode === 'circle') {
      setShapes(prev => [...prev, { type: 'circle', center: latLng, radius: 100 }]);
    }

    if (drawMode === 'rectangle') {
      const bounds = {
        north: latLng.lat + 0.0005,
        south: latLng.lat - 0.0005,
        east: latLng.lng + 0.0005,
        west: latLng.lng - 0.0005,
      };
      setShapes(prev => [...prev, { type: 'rectangle', bounds }]);
    }

    if (drawMode === 'free' && freeDrawPoints.length < 4) {
      setFreeDrawPoints((prev) => [...prev, latLng]);
    }
  };

  /* istanbul ignore next */
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

  /* istanbul ignore next */
  const calculateRoute = (points: google.maps.LatLngLiteral[]) => {
    if (points.length < 2) {
      alert("Se necesitan al menos dos puntos para calcular la ruta.");
      return;
    }

    const directionsService = new google.maps.DirectionsService();

    directionsService.route(
      {
        origin: points[0],
        destination: points[points.length - 1],
        waypoints: points.slice(1, -1).map((point) => ({ location: point, stopover: true })),
        travelMode: google.maps.TravelMode.WALKING,
      },
      (result, status) => {
        if (status === google.maps.DirectionsStatus.OK && result) {
          setDirections(result);
        } else {
          console.error("Error al calcular la ruta: ", status);
        }
      }
    );
  };

  /* istanbul ignore next */
  const searchRoute = (serialNumber: any, startDate: any, endDate: any) => {
    if (!startDate || !endDate) {
      alert("Please select both start and end dates.");
      return;
    }

    callApi(SearchRoute(serialNumber, startDate, endDate)).then((response: any) => {
      if (response && response.routes && response.routes.length > 0) {
        const coordinates = response.routes[0].coordinates.map((coord: any) => ({
          lat: coord[1],
          lng: coord[0],
        }));

        calculateRoute(coordinates);
      } else {
        alert("No se encontraron rutas.");
      }
    });
  };

  /* istanbul ignore next */
  const lastLocation = (serialNumber: string) => {
    callApi(SearchLastLocation(serialNumber)).then((response: any) => {
      if (response && response.latitude && response.longitude) {
        const position = {
          lat: response.latitude,
          lng: response.longitude,
        };
        setLastPositionMarker(position);
        if (mapRef.current) {
          mapRef.current.panTo(position);
          mapRef.current.setZoom(18);
        }
      }
    });
  };

  /* istanbul ignore next */
  const saveZones = async (serialNumber: string) => {
    const shapesToSend = shapes.map((shape) => {
      switch (shape.type) {
        case 'circle':
          return {
            type: 'circle',
            center: { lat: shape.center.lat, lng: shape.center.lng },
            radius: shape.radius,
          };

        case 'rectangle':
          return {
            type: 'rectangle',
            bounds: {
              north: shape.bounds.north,
              south: shape.bounds.south,
              east: shape.bounds.east,
              west: shape.bounds.west,
            },
          };

        case 'polygon': {
          let coordinates = shape.path.map(p => [p.lng, p.lat]);
          const first = coordinates[0];
          const last = coordinates[coordinates.length - 1];

          if (first[0] !== last[0] || first[1] !== last[1]) {
            coordinates.push(first);
          }

          return {
            type: 'polygon',
            coordinates: coordinates,
          };
        }

        default:
          return null;
      }
    });

    const payload = {
      serialNumber,
      shapes: shapesToSend,
    };

    callApi(BlockZone(payload)).then(() => {
      loadRestrictedZones(payload.serialNumber);
      resetFigure();
    });
  };

  /* istanbul ignore next */
  function renderShape(shape: Shape, idx: number, onClick?: () => void) {
    switch (shape.type) {
      case 'circle':
        return (
          <Circle
            key={idx}
            center={shape.center}
            radius={shape.radius}
            options={{
              fillColor: '#f00',
              fillOpacity: 0.2,
              strokeColor: '#f00',
              strokeOpacity: 0.5,
              strokeWeight: 2,
              editable: !!onClick
            }}
            onClick={onClick}
          />
        );
      case 'rectangle':
        return (
          <Rectangle
            key={idx}
            bounds={shape.bounds}
            options={{
              fillColor: '#f00',
              fillOpacity: 0.2,
              strokeColor: '#f00',
              strokeOpacity: 0.5,
              strokeWeight: 2,
              editable: !!onClick
            }}
            onClick={onClick}
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
              editable: !!onClick
            }}
            onClick={onClick}
          />
        );
      default:
        return null;
    }
  }

  /* istanbul ignore next */
  function renderEditableShapes(shapes: Shape[]) {
    return shapes.map((shape, idx) => renderShape(shape, idx));
  }

  /* istanbul ignore next */
  function renderFreeDrawMarkers(points: google.maps.LatLngLiteral[]) {
    return points.map((point, index) => (
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
    ));
  }

  const resetFigure = () => {
    setShapes([]);
    setFreeDrawPoints([]);
    setLastPositionMarker(null);
    setDirections(null);
  };

  const deleteZone = (zoneId: any, bandName: string) => {
    const confirmDelete = window.confirm("¿Are you sure you want to delete the zone?");
    if (!confirmDelete) return;

    const band = devices.find(b => b.name === bandName);
    if (band) {
      callApi(DeleteZone(zoneId)).then(() => {
        loadRestrictedZones(band.serialNumber);
      });
    }
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
            onLoad={onMapLoad}
          >

            {existingShapes.map((shape, idx) =>
              renderShape(shape, idx, () => {
                if (activeDevice) deleteZone(shape.zoneId, activeDevice);
              })
            )}

            {renderEditableShapes(shapes)}

            {renderFreeDrawMarkers(freeDrawPoints)}

            {renderFreeDrawPolygon()}

            {lastPositionMarker && (
              <Marker
                position={lastPositionMarker}
                icon={{
                  url: 'http://maps.google.com/mapfiles/ms/icons/red-dot.png',
                }}
                data-testid="marker"
              />
            )}

            {directions && (
              <DirectionsRenderer
                directions={directions}
              />
            )}

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
              key={band.name}
              style={{
                marginBottom: '15px',
                padding: '15px',
                backgroundColor: activeDevice === band.name ? '#e6f7ff' : '#fff',
                borderRadius: '8px',
                boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
                cursor: 'pointer'
              }}
              onClick={() => {
                if (band.name != activeDevice) {
                  setActiveDevice(band.name);
                  resetFigure();
                  loadRestrictedZones(band.serialNumber);
                }
              }}
            >
              <p>
                <strong>Band:</strong> {band.name}
              </p>
              <p>
                <strong>Last Update:</strong> {band.lastUpdate ? formatDate(band.lastUpdate) : "Not used"}
              </p>
              <button
                onClick={() =>
                  setOpenDetailsDevice((prev) => (prev === band.name ? null : band.name))
                }
                className="btn w-100 bg-dark text-white"
              >
                Search Locations
              </button>

              {openDetailsDevice === band.name && (
                <div style={{ borderTop: '1px solid #ccc', paddingTop: '10px' }}>
                  <div className="mb-2">
                    <label htmlFor={`start-${band.name}`} className="form-label">From:</label>
                    <input type="datetime-local" id={`start-${band.name}`} className="form-control" />
                  </div>

                  <div className="mb-3">
                    <label htmlFor={`end-${band.name}`} className="form-label">To:</label>
                    <input type="datetime-local" id={`end-${band.name}`} className="form-control" />
                  </div>

                  <button className="btn btn-dark w-100 mb-2" onClick={() =>
                    searchRoute(
                      band.serialNumber,
                      (document.getElementById(`start-${band.name}`) as HTMLInputElement).value,
                      (document.getElementById(`end-${band.name}`) as HTMLInputElement).value
                    )
                  }>Search</button>
                  <button className="btn btn-outline-dark w-100" onClick={() => lastLocation(band.serialNumber)}>See last location</button>
                </div>
              )}

              <div style={{ display: 'flex', gap: '10px', marginTop: '10px' }}>
                <select
                  id={`shape-select-${band.name}`}
                  data-testid="shape-select"
                  className="form-select"
                  value={drawModes[band.name] || ''}
                  onChange={(e) => {
                    const value = e.target.value;
                    setDrawModes((prev) => ({
                      ...prev,
                      [band.name]: value === '' ? null : (value as 'circle' | 'rectangle' | 'free')
                    }));
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
                  onClick={() => saveZones(band.serialNumber)}
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
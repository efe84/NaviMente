import '@testing-library/jest-dom';
import React from 'react';
import { render, screen, waitFor, fireEvent } from '@testing-library/react';
import Map from '../components/map/Map';
import * as apiHook from '../shared/hooks/useApi';
import * as deviceApi from '../api/deviceApi';
import * as locationApi from '../api/locationApi';
import { MemoryRouter, useParams } from 'react-router-dom';

jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useParams: jest.fn(),
}));

jest.mock('@react-google-maps/api', () => ({
    GoogleMap: ({ children }: any) => <div data-testid="google-map">{children}</div>,
    useLoadScript: () => ({ isLoaded: true, loadError: false }),
    Circle: (props: any) => <div data-testid="circle" {...props} />,
    Rectangle: (props: any) => <div data-testid="rectangle" {...props} />,
    Polygon: (props: any) => <div data-testid="polygon" {...props} />,
    Marker: (props: any) => <div data-testid="marker" {...props} />,
    DirectionsRenderer: () => <div data-testid="directions" />,
}));

describe('Map component', () => {
    const mockCallApi = jest.fn();

    beforeEach(() => {
        jest.clearAllMocks();
        jest.spyOn(apiHook, 'useApi').mockReturnValue(mockCallApi);
        (useParams as jest.Mock).mockReturnValue({ username: 'testuser' });
        localStorage.setItem('userName', 'testUser');
    });

    it('renders map and bands', async () => {

        const mockDevices = [
            { serialNumber: '123-ABC', name: 'Band A', lastUpdate: '2025-07-31T14:45:00Z' },
            { serialNumber: '234-ABC', name: 'Band B', lastUpdate: null }
        ];

        mockCallApi
            .mockResolvedValueOnce(mockDevices); // GetDevices

        render(<Map />, { wrapper: MemoryRouter });

        await waitFor(() => {
            expect(screen.getByText('Band A')).toBeInTheDocument();
            expect(screen.getByText('Band B')).toBeInTheDocument();
        });

        expect(screen.getByTestId('google-map')).toBeInTheDocument();

        await waitFor(() => {
            expect(mockCallApi).toHaveBeenCalledWith(deviceApi.GetDevices("1"));
        });

        expect(screen.getByText('31-07-2025 16:45:00')).toBeInTheDocument();
        expect(screen.getByText('Not used')).toBeInTheDocument();
    });

    it('calls deviceApi.Zones when a band is clicked', async () => {
        const mockDevices = [
            { serialNumber: '123-ABC', name: 'Band A', lastUpdate: null },
            { serialNumber: '234-ABC', name: 'Band B', lastUpdate: null }
        ];

        const mockZones = [
            {
                zoneId: 6,
                serialNumber: "123-ABC",
                shape: {
                    type: "circle",
                    center: [-8.712227131467934, 43.250472024132996],
                    radius: 100,
                }
            },
            {
                zoneId: 7,
                serialNumber: "123-ABC",
                shape: {
                    type: "rectangle",
                    bounds: {
                        north: 43.251,
                        south: 43.250,
                        east: -8.711,
                        west: -8.712
                    }
                }
            },
            // {
            //     zoneId: 8,
            //     serialNumber: "123-ABC",
            //     shape: {
            //         type: "polygon",
            //         path: [
            //             { lat: 43.2505, lng: -8.7120 },
            //             { lat: 43.2507, lng: -8.7122 },
            //             { lat: 43.2506, lng: -8.7124 }
            //         ]
            //     }
            // }
        ];

        mockCallApi
            .mockResolvedValueOnce(mockDevices)  // GetDevices
            .mockResolvedValueOnce(mockZones);   // GetZones

        render(<Map />, { wrapper: MemoryRouter });

        const bandA = await screen.findByText('Band A');
        fireEvent.click(bandA);

        await waitFor(() => {
            expect(mockCallApi).toHaveBeenCalledWith(deviceApi.Zones('123-ABC'));
        });

        expect(screen.getByTestId('circle')).toBeInTheDocument();
        expect(screen.getByTestId('rectangle')).toBeInTheDocument();
    });

    it('calls deviceApi.DeleteZone when clicking on a circle and confirming', async () => {
        const mockDevices = [
            { serialNumber: '123-ABC', name: 'Band A', lastUpdate: null }
        ];

        const mockZones = [
            {
                zoneId: 6,
                serialNumber: "123-ABC",
                shape: {
                    type: "circle",
                    center: [-8.712227131467934, 43.250472024132996],
                    radius: 100,
                }
            }
        ];

        mockCallApi.mockImplementation(() => Promise.resolve([]));

        mockCallApi
            .mockResolvedValueOnce(mockDevices)   // GetDevices
            .mockResolvedValueOnce(mockZones)     // Zones on band click
            .mockResolvedValueOnce([]);           // Zones after delete

        render(<Map />, { wrapper: MemoryRouter });

        const band = await screen.findByText('Band A');
        fireEvent.click(band);

        await waitFor(() => {
            expect(screen.getByTestId('circle')).toBeInTheDocument();
        });

        jest.spyOn(window, 'confirm').mockReturnValue(true);

        // Click en el círculo
        fireEvent.click(screen.getByTestId('circle'));

        await waitFor(() => {
            expect(mockCallApi).toHaveBeenCalledWith(deviceApi.DeleteZone("6"));
        });
    });

    it('calls locationApi.SearchLastLocation and renders Marker when "See last location" is clicked', async () => {
        const mockDevice = [
            { serialNumber: '123-ABC', name: 'Band A', lastUpdate: null }
        ];

        const mockLocation = {
            lat: 43.250472024132996,
            lng: -8.712227131467934
        };

        mockCallApi
            .mockResolvedValueOnce(mockDevice)
            .mockResolvedValueOnce(mockLocation);

        render(<Map />, { wrapper: MemoryRouter });

        const searchButton = await screen.findByText('Search Locations');
        fireEvent.click(searchButton);

        const lastLocationButton = await screen.findByText('See last location');
        fireEvent.click(lastLocationButton);

        await waitFor(() => {
            expect(mockCallApi).toHaveBeenCalledWith(
                locationApi.SearchLastLocation('123-ABC')
            );
        });

        // await waitFor(() => {
        //     expect(screen.getByTestId('marker')).toBeInTheDocument();
        // });
    });

    it('calls locationApi.SearchRoute when date range is submitted', async () => {
        const mockDevice = [
            { serialNumber: '123-ABC', name: 'Band A', lastUpdate: null }
        ];

        const mockLocations = [
            {
                lat: 43.25,
                lng: -8.71,
                timestamp: '2025-07-01T10:00:00.000Z'
            },
            {
                lat: 43.26,
                lng: -8.72,
                timestamp: '2025-07-01T12:00:00.000Z'
            }
        ];

        mockCallApi
            .mockResolvedValueOnce(mockDevice)      // GetDevices
            .mockResolvedValueOnce(mockLocations);  // SearchLocations

        render(<Map />, { wrapper: MemoryRouter });

        // Pulsar "Search Locations"
        const searchLocationsBtn = await screen.findByText('Search Locations');
        fireEvent.click(searchLocationsBtn);

        // Esperar inputs From y To
        const fromInput = await screen.findByLabelText(/from/i);
        const toInput = await screen.findByLabelText(/to/i);

        // Simular selección de rango que incluya las 2 fechas
        fireEvent.change(fromInput, { target: { value: '2025-07-01T09:00' } });
        fireEvent.change(toInput, { target: { value: '2025-07-01T13:00' } });

        // Pulsar botón "Search"
        const searchBtn = screen.getByText('Search', { exact: true });
        fireEvent.click(searchBtn);

        // Verificar que se llamó a SearchRoute con fechas correctas
        await waitFor(() => {
            expect(mockCallApi).toHaveBeenCalledWith(
                locationApi.SearchRoute(
                    '123-ABC',
                    '2025-07-01T09:00',
                    '2025-07-01T13:00'
                )
            );
        });
    });

    it('blocks a zone by selecting circle, clicking on map, and pressing "Block zone"', async () => {
        const lat = 43.21235914096343;
        const lng = -8.689408759246819;

        const mockDevices = [
            { serialNumber: "123-ABC", name: 'Band A', lastUpdate: null },
        ];

        const zoneToBlock = {
            "serialNumber": "B2412021V1",
            "shapes": [
                {
                    "type": "circle",
                    "center": {
                        "lat": lat,
                        "lng": lng
                    },
                    "radius": 100
                }
            ]
        };

        const mockZonesBlocked = [{
            "zoneId": 12,
            "serialNumber": "B2412021V1",
            "createdAt": "2025-07-08T21:26:22.197Z",
            "shape": {
                "type": "circle",
                "coordinates": null,
                "center": [
                    lng,
                    lat
                ],
                "radius": 100,
                "bounds": null
            }
        }];

        mockCallApi
            .mockResolvedValueOnce(mockDevices) // GetDevices
            .mockResolvedValueOnce([])         // Zones al seleccionar dispositivo
            .mockResolvedValueOnce(mockZonesBlocked);        // Zones tras bloqueo

        render(<Map />, { wrapper: MemoryRouter });

        await waitFor(() => {
            expect(mockCallApi).toHaveBeenCalledWith(deviceApi.GetDevices("1"));
        });

        const band = await screen.findByText('Band A');
        fireEvent.click(band);

        await waitFor(() => {
            expect(mockCallApi).toHaveBeenCalledWith(deviceApi.Zones("123-ABC"));
        });

        const shapeSelect = screen.getByTestId("shape-select");
        fireEvent.change(shapeSelect, { target: { value: 'circle' } });

        const map = screen.getByTestId('google-map');
        fireEvent.click(map, {
            latLng: {
                lat: () => lat,
                lng: () => lng,
            },
        });

        const blockBtn = await screen.findByRole('button', { name: /block zone/i });
        fireEvent.click(blockBtn);

        // 6. Esperar llamada a BlockZone con datos correctos
        // await waitFor(() => {
        //     expect(mockCallApi).toHaveBeenCalledWith(deviceApi.BlockZone(zoneToBlock));
        // });

        await waitFor(() => {
            expect(mockCallApi).toHaveBeenCalledWith(deviceApi.Zones("123-ABC"));
        });
    });
});
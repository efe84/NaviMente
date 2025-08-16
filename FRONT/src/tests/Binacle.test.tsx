import '@testing-library/jest-dom';
import React from 'react';
import { render, screen, waitFor, fireEvent } from '@testing-library/react';
import Binnacle from '../components/binnacle/Binnacle';
import * as api from '../shared/hooks/useApi';
import * as deviceApi from '../api/deviceApi';
import * as logApi from '../api/binnacleApi';
import { MemoryRouter } from 'react-router-dom';
import { ApiMethod } from '../shared/hooks/useApi';

jest.mock('../components/layout/Footer', () => () => <div data-testid="footer" />);
jest.mock('../components/binnacle/DeviceList', () => ({ devices, onSelectDevice, selectedDevice }: any) => (
    <div>
        {devices.map((device: any) => (
            <button key={device.serialNumber} onClick={() => onSelectDevice(device)} data-testid={`device-${device.serialNumber}`}>
                {device.name}
            </button>
        ))}
    </div>
));
jest.mock('../components/binnacle/ChatLog', () => ({ messages }: any) => (
    <div data-testid="chat-log">{messages.map((msg: any, i: number) => <div key={i}>{msg.message}</div>)}</div>
));
jest.mock('../components/binnacle/QuickActions', () => ({ onFilter }: any) => (
    <div>
        <button onClick={() => onFilter(null)} data-testid="filter-all">All</button>
        <button onClick={() => onFilter(1)} data-testid="filter-info">Info</button>
        <button onClick={() => onFilter(2)} data-testid="filter-warn">Warn</button>
        <button onClick={() => onFilter(3)} data-testid="filter-error">Error</button>
    </div>
));

const mockDevices = [
    { serialNumber: 'B2412021V1', name: 'David Band' },
];

const mockLogs = [
    { timestamp: '25/06/2025 02:00:19', severity: 1, message: 'Dispositivo NaviMente iniciado' },
];

describe('Binnacle component', () => {
    beforeEach(() => {
        // Mock del hook useApi que intercepta llamadas API
        jest.spyOn(api, 'useApi').mockReturnValue((request: any) => {
            if (request.url.startsWith('/Device/List')) {
                return Promise.resolve(mockDevices);
            }
            if (request.url.startsWith(`/Binnacle/${mockDevices[0].serialNumber}`)) {
                return Promise.resolve(mockLogs);
            }
            return Promise.resolve([]);
        });

        // Mock directo de GetDevices y GetLogs (por si el componente los llama directamente)
        jest.spyOn(deviceApi, 'GetDevices').mockImplementation((userId: string) => ({
            url: `/Device/List?userId=${userId}`,
            method: ApiMethod.GET,
        }));

        jest.spyOn(logApi, 'GetLogs').mockImplementation((serialNumber: string, severity?: number | null) => ({
            url: severity != null
                ? `/Binnacle/${serialNumber}?severity=${severity}`
                : `/Binnacle/${serialNumber}`,
            method: ApiMethod.GET,
        }));
    });

    afterEach(() => {
        jest.clearAllMocks();
    });

    it('renderiza correctamente los dispositivos y logs', async () => {
        render(<Binnacle />, { wrapper: MemoryRouter });

        await waitFor(() => {
            expect(screen.getByText('David Band')).toBeInTheDocument();
            expect(screen.getByText('Dispositivo NaviMente iniciado')).toBeInTheDocument();
        });
    });

    it('cambia de dispositivo al hacer click', async () => {
        const secondDevice = { serialNumber: 'B2412021V2', name: 'Second Band' };

        // Simulamos que GetDevices devuelve dos dispositivos
        (api.useApi as jest.Mock).mockReturnValue((request: any) => {
            if (request.url.startsWith('/Device/List')) {
                return Promise.resolve([mockDevices[0], secondDevice]);
            }
            if (request.url.includes(secondDevice.serialNumber)) {
                return Promise.resolve([{ timestamp: '26/06/2025 10:00:00', severity: 2, message: 'Mensaje del segundo dispositivo' }]);
            }
            return Promise.resolve(mockLogs);
        });

        render(<Binnacle />, { wrapper: MemoryRouter });

        const secondBtn = await screen.findByTestId(`device-${secondDevice.serialNumber}`);
        fireEvent.click(secondBtn);

        await waitFor(() => {
            expect(screen.getByText('Mensaje del segundo dispositivo')).toBeInTheDocument();
        });
    });

    it('filtra mensajes por severidad', async () => {
        render(<Binnacle />);
        // Clic en filtro de severidad: error
        fireEvent.click(screen.getByTestId('filter-error'));

        // Verificamos que desapareció el mensaje original
        await waitFor(() => {
            expect(screen.queryByText('Dispositivo NaviMente iniciado')).not.toBeInTheDocument();
        });
    });
});
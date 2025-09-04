import '@testing-library/jest-dom';
import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import Profile from '../components/user/Profile';
import * as apiHook from '../shared/hooks/useApi';
import * as authApi from '../api/authApi';
import * as deviceApi from '../api/deviceApi';
import { MemoryRouter, useParams } from 'react-router-dom';

// Mocks
jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useParams: jest.fn(),
}));

describe('Profile component', () => {
    const mockCallApi = jest.fn();

    beforeEach(() => {
        jest.clearAllMocks();
        jest.spyOn(apiHook, 'useApi').mockReturnValue(mockCallApi);
        (useParams as jest.Mock).mockReturnValue({ username: 'testuser' });
    });

    it('fetches user and devices on mount', async () => {
        const mockUser = {
            userId: 1,
            username: 'testuser',
            email: 'user@example.com',
            mainPhone: '1111',
            otherPhones: ['2222'],
            role: 1,
            telegramChatId: null,
        };

        const mockDevices = [
            { serialNumber: '123-ABC', name: 'Band A', lastUpdate: null }
        ];

        mockCallApi
            .mockResolvedValueOnce(mockUser) // GetUser
            .mockResolvedValueOnce(mockDevices); // GetDevices

        render(<Profile />, { wrapper: MemoryRouter });

        expect(await screen.findByText('testuser')).toBeInTheDocument();
        expect(screen.getByText('user@example.com')).toBeInTheDocument();
        expect(screen.getByText('Band A')).toBeInTheDocument();

        await waitFor(() => {
            expect(mockCallApi).toHaveBeenCalledWith(authApi.GetUser('testuser'));
            expect(mockCallApi).toHaveBeenCalledWith(deviceApi.GetDevices("1"));
        });
    });

    it('alerts when registering a device without required fields', async () => {
        mockCallApi
            .mockResolvedValueOnce({
                userId: 1,
                username: 'testuser',
                email: 'email@mail.com',
                mainPhone: '1111',
                otherPhones: [],
                role: 1,
                telegramChatId: null,
            })
            .mockResolvedValueOnce([]);

        window.alert = jest.fn();

        render(<Profile />, { wrapper: MemoryRouter });

        fireEvent.click(await screen.findByAltText(/add device/i));
        fireEvent.click(screen.getByText(/save/i));

        expect(window.alert).toHaveBeenCalledWith('Please fill in all fields.');
    });

    it('shows telegram code when linking', async () => {
        const mockUser = {
            userId: 1,
            username: 'testuser',
            email: 'email@mail.com',
            mainPhone: '1111',
            otherPhones: [],
            role: 1,
            telegramChatId: null,
        };

        mockCallApi
            .mockResolvedValueOnce(mockUser)
            .mockResolvedValueOnce([])
            .mockResolvedValueOnce({ code: 123456 });

        render(<Profile />, { wrapper: MemoryRouter });

        const button = await screen.findByText(/link telegram/i);
        fireEvent.click(button);

        expect(await screen.findByText(/\/vincular 123456/)).toBeInTheDocument();
    });

    it('confirms before unlinking telegram', async () => {
        const mockUser = {
            userId: 1,
            username: 'testuser',
            email: 'email@mail.com',
            mainPhone: '1111',
            otherPhones: [],
            role: 1,
            telegramChatId: 12345,
        };

        const updatedUser = {
            ...mockUser,
            telegramChatId: null,
        };

        mockCallApi
            .mockResolvedValueOnce(mockUser)
            .mockResolvedValueOnce([])
            .mockResolvedValueOnce({ success: true })
            .mockResolvedValueOnce(updatedUser);

        window.confirm = jest.fn(() => true);

        render(<Profile />, { wrapper: MemoryRouter });

        fireEvent.click(await screen.findByText(/unlink telegram/i));

        await waitFor(() => {
            expect(mockCallApi).toHaveBeenCalledWith(authApi.UnlinkTelegram(1));
        });
    });

    it('allows editing the main phone number', async () => {
        const mockUser = {
            userId: 1,
            username: 'testuser',
            email: 'user@example.com',
            mainPhone: '111111111',
            otherPhones: [],
            role: 1,
            telegramChatId: null,
        };

        const mockUserEdit = {
            userId: 1,
            username: 'testuser',
            email: 'user@example.com',
            mainPhone: '222222222',
            otherPhones: [],
            role: 1,
            telegramChatId: null,
        };

        mockCallApi
            .mockResolvedValueOnce(mockUser) // GetUser
            .mockResolvedValueOnce([])       // GetDevices
            .mockResolvedValueOnce(mockUserEdit);        // EditMainPhone

        render(<Profile />, { wrapper: MemoryRouter });

        const mainPhoneElement = await screen.findByText('111111111');
        expect(mainPhoneElement).toBeInTheDocument();

        fireEvent.click(screen.getByAltText(/edit main phone/i));

        const input = screen.getByDisplayValue('111111111');
        fireEvent.change(input, { target: { value: '222222222' } });

        fireEvent.click(screen.getByAltText(/save main phone/i));

        await waitFor(() => {
            expect(mockCallApi).toHaveBeenCalledWith(authApi.EditMainPhone('testuser', '222222222'));
        });
    });

    it('allows editing the email address', async () => {
        const mockUser = {
            userId: 1,
            username: 'testuser',
            email: 'user@example.com',
            mainPhone: '111111111',
            otherPhones: [],
            role: 1,
            telegramChatId: null,
        };

        const mockUserEdit = {
            userId: 1,
            username: 'testuser',
            email: 'newemail@example.com',
            mainPhone: '111111111',
            otherPhones: [],
            role: 1,
            telegramChatId: null,
        };

        mockCallApi
            .mockResolvedValueOnce(mockUser) // GetUser
            .mockResolvedValueOnce([])       // GetDevices
            .mockResolvedValueOnce(mockUserEdit);        // EditEmail

        render(<Profile />, { wrapper: MemoryRouter });

        const emailElement = await screen.findByText('user@example.com');
        expect(emailElement).toBeInTheDocument();

        fireEvent.click(screen.getByAltText(/edit email/i));

        const input = screen.getByDisplayValue('user@example.com');
        fireEvent.change(input, { target: { value: 'newemail@example.com' } });

        fireEvent.click(screen.getByAltText(/save email/i));

        await waitFor(() => {
            expect(mockCallApi).toHaveBeenCalledWith(authApi.EditEmail('testuser', 'newemail@example.com'));
        });
    });

    it('shows alert when removing a phone number fails', async () => {
        const mockUser = {
            userId: 1,
            username: 'testuser',
            email: 'user@example.com',
            mainPhone: '1111',
            otherPhones: ['222222222'],
            role: 1,
            telegramChatId: null,
        };

        // Mock de console.error y alert
        const consoleErrorSpy = jest.spyOn(console, 'error').mockImplementation(() => { });
        window.alert = jest.fn();

        mockCallApi
            .mockResolvedValueOnce(mockUser) // GetUser
            .mockResolvedValueOnce([])       // GetDevices
            .mockRejectedValueOnce(new Error('API error')); // RemovePhone falla

        render(<Profile />, { wrapper: MemoryRouter });

        expect(await screen.findByText(/222222222/)).toBeInTheDocument();

        fireEvent.click(screen.getByAltText(/delete phone/i));

        await waitFor(() => {
            expect(mockCallApi).toHaveBeenCalledWith(authApi.RemovePhone('testuser', '222222222'));
            expect(window.alert).toHaveBeenCalledWith('Failed to remove phone number');
            expect(consoleErrorSpy).toHaveBeenCalledWith(
                'Error removing phone:',
                expect.any(Error)
            );
        });

        consoleErrorSpy.mockRestore();
    });

    it('registers a device successfully when fields are filled', async () => {
        const mockUser = {
            userId: 1,
            username: 'testuser',
            email: 'user@example.com',
            mainPhone: '1111',
            otherPhones: [],
            role: 1,
            telegramChatId: null,
        };

        mockCallApi
            .mockResolvedValueOnce(mockUser) // GetUser
            .mockResolvedValueOnce([])       // GetDevices
            .mockResolvedValueOnce(["Sensor X"]);        // RegisterDevice

        render(<Profile />, { wrapper: MemoryRouter });

        fireEvent.click(await screen.findByAltText(/add device/i));

        fireEvent.change(screen.getByPlaceholderText(/Device Name/i), { target: { value: 'Sensor X' } });
        fireEvent.change(screen.getByPlaceholderText(/Serial Number/i), { target: { value: 'ABC123' } });

        fireEvent.click(screen.getByText(/save/i));

        await waitFor(() => {
            expect(mockCallApi).toHaveBeenCalledWith(
                expect.objectContaining(
                    deviceApi.RegisterDevice({ DeviceName: 'Sensor X', SerialNumber: 'ABC123', UserId: 1 })
                )
            );
        });
    });

    it('shows alert when device registration fails', async () => {
        const mockUser = {
            userId: 1,
            username: 'testuser',
            email: 'user@example.com',
            mainPhone: '1111',
            otherPhones: [],
            role: 1,
            telegramChatId: null,
        };

        mockCallApi
            .mockResolvedValueOnce(mockUser) // GetUser
            .mockResolvedValueOnce([])       // GetDevices
            .mockRejectedValueOnce(new Error('Registration failed')); // RegisterDevice

        window.alert = jest.fn();

        render(<Profile />, { wrapper: MemoryRouter });

        fireEvent.click(await screen.findByAltText(/add device/i));
        fireEvent.change(screen.getByPlaceholderText(/Device Name/i), { target: { value: 'Sensor X' } });
        fireEvent.change(screen.getByPlaceholderText(/Serial Number/i), { target: { value: 'ABC123' } });

        fireEvent.click(screen.getByText(/save/i));

        await waitFor(() => {
            expect(mockCallApi).toHaveBeenCalledWith(
                expect.objectContaining(
                    deviceApi.RegisterDevice({ DeviceName: 'Sensor X', SerialNumber: 'ABC123', UserId: 1 })
                )
            );
            expect(window.alert).toHaveBeenCalledWith('Error registering device.');
        });
    });

    it('unlinks a device and reloads the page', async () => {
        const mockUser = {
            userId: 1,
            username: 'testuser',
            email: 'user@example.com',
            mainPhone: '1111',
            otherPhones: [],
            role: 1,
            telegramChatId: null,
        };

        const mockDevices = [
            { serialNumber: 'ABC123', name: 'Sensor X', lastUpdate: null },
            { serialNumber: 'XYZ789', name: 'Tracker Y', lastUpdate: null }
        ];

        const mockUpdatedDevices = [
            { serialNumber: 'ABC123', name: 'Sensor X', lastUpdate: null }
        ];

        mockCallApi
            .mockResolvedValueOnce(mockUser)     // GetUser
            .mockResolvedValueOnce(mockDevices)  // GetDevices
            .mockResolvedValueOnce(mockUpdatedDevices);            // UnassignDevice

        render(<Profile />, { wrapper: MemoryRouter });

        expect(await screen.findByText('Sensor X')).toBeInTheDocument();
        expect(screen.getByText('Tracker Y')).toBeInTheDocument();

        const select = screen.getByRole('combobox');
        fireEvent.change(select, { target: { value: 'XYZ789' } });

        fireEvent.click(screen.getByText(/remove/i));

        await waitFor(() => {
            expect(mockCallApi).toHaveBeenCalledWith(deviceApi.UnassignDevice(1, 'XYZ789'));
        });

    });
});
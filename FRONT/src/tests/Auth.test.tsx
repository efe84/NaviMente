import '@testing-library/jest-dom';
import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import Auth from '../components/user/Auth';
import * as apiHook from '../shared/hooks/useApi';
import { MemoryRouter, useNavigate } from 'react-router-dom';

// Mock de useNavigate para verificar redirecciones
jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'),
  useNavigate: jest.fn(),
}));

describe('Auth component', () => {
  const mockCallApi = jest.fn();

  beforeEach(() => {
    jest.clearAllMocks();
    jest.spyOn(apiHook, 'useApi').mockReturnValue(mockCallApi);
  });

  it('login flow', async () => {
    (useNavigate as jest.Mock).mockReturnValue(jest.fn());

    render(<Auth isLogin={true} />, { wrapper: MemoryRouter });

    // Completa campos
    fireEvent.change(screen.getByLabelText(/username/i), { target: { value: 'testuser' } });
    fireEvent.change(screen.getByLabelText(/password/i), { target: { value: '1234' } });

    // Mock respuesta del login
    mockCallApi.mockResolvedValueOnce({ result: { success: true } });

    // Enviar formulario
    fireEvent.submit(screen.getByRole('button', { name: /login/i }));

    await waitFor(() => {
      expect(mockCallApi).toHaveBeenCalledWith({
        url: '/User/Login',
        method: 'POST',
        body: {
          username: 'testuser',
          password: '1234',
        },
      });
    });
  });

  it('register flow with device info', async () => {
    (useNavigate as jest.Mock).mockReturnValue(jest.fn());

    render(<Auth isLogin={false} />, { wrapper: MemoryRouter });

    // Completar campos
    fireEvent.change(screen.getByLabelText(/username/i), { target: { value: 'testuser' } });
    fireEvent.change(screen.getByLabelText(/password/i), { target: { value: 'pass' } });
    fireEvent.change(screen.getByLabelText(/email/i), { target: { value: 'mail@mail.com' } });
    fireEvent.change(screen.getByLabelText(/phone number/i), { target: { value: '123456' } });
    fireEvent.change(screen.getByLabelText(/naviBand code/i), { target: { value: '123-ABC' } });
    fireEvent.change(screen.getByLabelText(/naviBand name/i), { target: { value: 'Band A' } });

    mockCallApi.mockResolvedValueOnce({ result: { success: true } });

    fireEvent.submit(screen.getByRole('button', { name: /register/i }));

    await waitFor(() => {
      expect(mockCallApi).toHaveBeenCalledWith({
        url: '/User/Register',
        method: 'POST',
        body: {
          username: 'testuser',
          password: 'pass',
          email: 'mail@mail.com',
          phoneNumber: '123456',
          deviceId: '123-ABC',
          deviceName: 'Band A',
        },
      });
    });
  });

  it('shows alert if serialNumber is set but deviceName is empty', () => {
    window.alert = jest.fn(); // mock alert

    render(<Auth isLogin={false} />, { wrapper: MemoryRouter });

    fireEvent.change(screen.getByLabelText(/naviBand code/i), { target: { value: 'XYZ-001' } });
    fireEvent.submit(screen.getByRole('button', { name: /register/i }));

    expect(window.alert).toHaveBeenCalledWith(
      'If you add NaviBand Code, you must create a NaviBand Name too.'
    );
  });
});
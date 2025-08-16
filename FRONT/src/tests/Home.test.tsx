import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import Home from '../components/home/Home';
import { MemoryRouter } from 'react-router-dom';

// Mock useNavigate
const mockNavigate = jest.fn();

jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'),
  useNavigate: () => mockNavigate,
}));

// Mock imágenes
jest.mock('../assets/logo.png', () => '');
jest.mock('../assets/map.png', () => '');
jest.mock('../assets/chat.png', () => '');

// Mock Footer
jest.mock('../components/layout/Footer', () => () => <div data-testid="footer" />);

describe('Home Page', () => {
  beforeEach(() => {
    localStorage.clear();
    jest.clearAllMocks();
  });

  it('muestra el mensaje de bienvenida y los botones si el usuario NO está logueado', () => {
    render(<Home />, { wrapper: MemoryRouter });

    expect(screen.getByText(/Welcome to NaviMente/i)).toBeInTheDocument();
    expect(screen.getByText(/Discover a new way to help people/i)).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /Login/i })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /Register/i })).toBeInTheDocument();
  });

  it('navega a /Login cuando se hace clic en el botón Login', () => {
    render(<Home />, { wrapper: MemoryRouter });

    fireEvent.click(screen.getByRole('button', { name: /Login/i }));
    expect(mockNavigate).toHaveBeenCalledWith('/Login');
  });

  it('navega a /Register cuando se hace clic en el botón Register', () => {
    render(<Home />, { wrapper: MemoryRouter });

    fireEvent.click(screen.getByRole('button', { name: /Register/i }));
    expect(mockNavigate).toHaveBeenCalledWith('/Register');
  });

  it('NO muestra botones de login/register si el usuario está logueado', () => {
    localStorage.setItem('userName', 'Juan');

    render(<Home />, { wrapper: MemoryRouter });

    expect(screen.queryByRole('button', { name: /Login/i })).not.toBeInTheDocument();
    expect(screen.queryByRole('button', { name: /Register/i })).not.toBeInTheDocument();
  });

  it('navega a /Map al hacer clic en la tarjeta "Find Your Device"', () => {
    render(<Home />, { wrapper: MemoryRouter });

    const mapCard = screen.getByText(/Find Your Device/i).closest('.card');
    expect(mapCard).toBeInTheDocument();

    fireEvent.click(mapCard!);
    expect(mockNavigate).toHaveBeenCalledWith('/Map');
  });

  it('navega a /Binnacle al hacer clic en la tarjeta "Check Last Messages"', () => {
    render(<Home />, { wrapper: MemoryRouter });

    const chatCard = screen.getByText(/Check Last Messages/i).closest('.card');
    expect(chatCard).toBeInTheDocument();

    fireEvent.click(chatCard!);
    expect(mockNavigate).toHaveBeenCalledWith('/Binnacle');
  });
});
import React from "react";
import { createContext, useCallback, useState } from "react";

export enum SnackSeverity {
  ERROR = 'error',
  WARNING = 'warning',
  INFO = 'info',
  SUCCESS = 'success'
}

type Props = {
  children: React.ReactNode,
};

export const SnackContext = createContext<Function>(() => { });

/**
 * Este Handler, al igual que el del dialogo, sirve para proveer a toda la aplicación de una funcionalidad básica: mostrar snacks de resultado
 * al hacer operaciones. Además, se consume desde el hook "useApi" para mostrar el resultado de una llamada al back en caso de ser necesario.
 * De la misma forma, esta completamente aislado del resto de arbol de componentes y no causa mutaciones en el.
 */
export default function SnackHandler({ children }: Props) {
  const [snack, setSnack] = useState({ open: false, severity: SnackSeverity.WARNING, message: 'Esto es un mensaje de alerta por defecto.' });

  //Memorizamos la funcion con useCallback, para que cuando cambie el estado de la modal no se regenere la funcion
  //y notifique a los componentes subscritos
  const showSnack = useCallback(
    (message: string, severity: SnackSeverity) => {
      setSnack({ open: true, message, severity });
    },
    []
  );

  function handleClose() {
    setSnack(prev => ({ ...prev, open: false }));
  }

  function getSeverityColor(severity: SnackSeverity): string {
    switch (severity) {
      case SnackSeverity.ERROR:
        return '#f44336'; // Rojo
      case SnackSeverity.WARNING:
        return '#ff9800'; // Naranja
      case SnackSeverity.INFO:
        return '#2196f3'; // Azul
      case SnackSeverity.SUCCESS:
        return '#4caf50'; // Verde
      default:
        return '#333'; // Default (gris oscuro)
    }
  }

  return (
    <SnackContext.Provider value={showSnack}>
      {children}
      {snack.open && (
        <div
          style={{
            position: 'fixed',
            top: '20px',
            right: '20px',
            padding: '16px 24px',
            borderRadius: '8px',
            color: '#fff',
            display: 'flex',
            alignItems: 'center',
            boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)',
            zIndex: 1000,
            minWidth: '250px',
            maxWidth: '400px',
            backgroundColor: getSeverityColor(snack.severity),
          }}
        >
          <span style={{ flex: 1, fontSize: '16px', }}>{snack.message}</span>
          <button style={{
            background: 'transparent',
            border: 'none',
            color: '#fff',
            fontSize: '16px',
            cursor: 'pointer',
          }} onClick={handleClose}>
            ✖
          </button>
        </div>
      )}
    </SnackContext.Provider>
  );
}
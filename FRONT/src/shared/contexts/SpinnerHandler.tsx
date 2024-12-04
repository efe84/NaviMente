import React, { useCallback, useMemo, useState } from "react";
import { createContext } from "react";

type Props = {
    children: React.ReactNode,
};

type SpinnerController = {
    increaseLoader: Function,
    decreaseLoader: Function
};

export const SpinnerContext = createContext<SpinnerController>({} as SpinnerController);

/**
 * Este Handler nos exponse un spinner que se puede usar desde cualquier lugar en la aplicación y se cerciora de que los incrementos/decrementos
 * sean consistentes exponiendo dos funciones para aumentar o disminuir el contador en lugar de exponer el setter.
 */
export default function SpinnerHandler({ children }: Props) {
    const [counter, setCounter] = useState(0);

    //Utilizamos useCallback en ambas funciones para evitar que se regenere la función, causando mutaciones innecesarias en los subscriptores
    const increase = useCallback(() => {
        setCounter(prev => prev+1);
    }, []);
    
    const decrease = useCallback(() => {
        setCounter(prev => prev-1);
    }, []);

    //Como el value que devuelve el proveedor del contexto es un objeto, tambien tenemos que memoizarlo para que no se regenere un objeto exactamente
    //igual pero con diferente referencia causando mutaciones en los subscriptores. Técnicamente, en el array de dependencias deberiamos incluir
    //las dos funciones que se exponen en el objeto, pero al ser ambas referencias fijas memoizadas sin array de dependencias podemos omitirlo.
    const value = useMemo(() => { return { increaseLoader: increase, decreaseLoader: decrease };}, []);

    return (
        <SpinnerContext.Provider value={value}>
            {counter > 0 && 
                <div style={{ position:'absolute', width:'100%', height: '100%', display:'flex', backgroundColor:'rgba(0,0,0,0.5)', zIndex: 1000 }}>
                    <div style={{ margin: 'auto', border: '8px solid rgba(255, 255, 255, 0.3)', borderTop: '8px solid white', borderRadius: '50%', width: '50px', height: '50px', animation: 'spin 1s linear infinite', }}></div>
                    <style>
                        {`
                            @keyframes spin {
                                0% {
                                    transform: rotate(0deg);
                                }
                                100% {
                                    transform: rotate(360deg);
                                }
                            }
                        `}
                    </style>
                </div>
            }
            {children}
        </SpinnerContext.Provider>
    );
}
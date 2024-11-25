import React from "react";
import SnackHandler from "./SnackHandler";
import SpinnerHandler from "./SpinnerHandler";

type Props = {
    children: React.ReactNode
};

/**
 * Este componente sirve, simplemente, como wrapper para todos los contextos que utilicemos a nivel de raiz en la aplicación,
 * de forma que evitamos un bloating excesivo en el componente principal.
 */
export default function ContextProvider({ children }: Props) {
    return (
        <SpinnerHandler>
            <SnackHandler>
                {children}
            </SnackHandler>
        </SpinnerHandler>
    );
}
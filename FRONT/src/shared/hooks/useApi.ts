import { useCallback, useContext } from "react";
import { useNavigate, useLocation, matchPath } from "react-router-dom";
import { SnackContext, SnackSeverity } from "../contexts/SnackHandler";
import { SpinnerContext } from "../contexts/SpinnerHandler";

type ApiParams = {
    url: string,
    method: ApiMethod,
    body?: object
};

type ApiError = {
    message: string,
    status: number
} | null;

type ApiResponse = {
    result: object,
    errorMessage: string
} | null;

/**
 * Hook para la gestión centralizada de peticiones a una API.
 * Expone unicamente el metodo "callApi", que recibe una url, un tipo de llamada (POST, GET, PUT...) y un body en caso de necesitarlo para
 * realizar todas las operaciones relativas a la llamada de forma automática y reducir la replicación de código inutil.
 * Es importante incluir algo similar en los proyectos para evitar el control de error en cada una de las llamadas y la logica que pueda
 * estar derivada de ciertos tipos de respuestas, que pueden ser costosos de modificar una vez la aplicación ha crecido.
 * 
 * Ejemplos de uso: ver componente Login,
 */
export const useApi = () => {
    const showSnack = useContext(SnackContext);
    const {increaseLoader, decreaseLoader} = useContext(SpinnerContext);
    const navigate = useNavigate();
    const {pathname} = useLocation();

    /**
     * Funcion que se encarga de gestionar las llamadas erroneas. Por defecto, muestra un snack con un error genérico.
     * @param message: mensaje de error a mostrar
     * @param status: código de estado http de la respuesta. 
     * En caso de recibir un mensaje ya definido (por ejemplo, del back) en el parametro message, lo muestra.
     * Para algunos status codes, se predefinen mensajes y/o comportamientos, como redirigir al login en caso
     * de carecer de autorización.
     */
    const errorHandler = useCallback(function handleErrors(message: string, status: number): ApiError {
        switch(status){
            case 401:
                if(matchPath(pathname, '/login')){
                    showSnack('Las credenciales no son correctas.', SnackSeverity.ERROR);
                } else {
                    showSnack('Necesitas iniciar sesión para ver esta página.', SnackSeverity.ERROR);
                    navigate('/login');    
                }
                break;
            case 403:
                navigate(-1);
                break;
            case 404:
                showSnack('No se ha podido encontrar el recurso que estas buscando :(', SnackSeverity.ERROR);
                break;
            default:
                if (!message)
                    message = 'Ha ocurrido un error al hacer la solicitud.';
                showSnack(message, SnackSeverity.ERROR);
        }
        return {message, status};
    }, [showSnack, navigate, pathname]);

    /**
     * Función que lleva toda el flujo de la llamada a la API
     * @param url: url a la que debe llamar
     * @param method: metodo de la llamada. Se usa ApiMethod.
     * @param body: objeto que se envia como cuerpo de la llamada en caso de POST o PUT
     * @param handleErrors: si es false, evita el control de errores y devuelve la excepción en el catch.
     */
    const callApi = useCallback(async function callApi({url, method, body}: ApiParams, globalSpinner = true, handleErrors = true) {
        let call;
        if(globalSpinner) {
            increaseLoader();
        }
        const apiUrl = `${(window as any).getConfig("API_URL")}${url}`;
        switch(method){
            case ApiMethod.POST:
                call = post(apiUrl, body);
                break;
            case ApiMethod.GET:
                call = get(apiUrl);
                break;
            case ApiMethod.DELETE:
                call = remove(apiUrl);
                break;
            case ApiMethod.PUT:
                call = put(apiUrl, body);
                break;
            default:
                return;
        }

        let data: ApiResponse = null;
        let resultError: ApiError = null;

        /**
         * Gestion de la petición en si misma. Se utiliza .then y await al mismo tiempo para poder
         * controlar con comodidad el flujo en caso de error.
         */
        data = await call.then(async (response) => {
            if(response.status === 500) {
                return Promise.reject({message:'Error en la petición.', status: response.status});
            }

            if(response.status !== 200) {
                var responseMessage = await response.json();
                return Promise.reject({message: responseMessage?.message, status:response.status});
            }

            // if(response.status !== 200) {
            //     return Promise.reject({message: data?.errorMessage ? data?.errorMessage : data, status:response.status});
            // }

            if(response.headers.get('Content-Type')?.includes('application/json')){
                return await response.json();
            }

        }).catch(error => {
            if (handleErrors) {
                resultError = errorHandler(error.message, error.status);
            } else {
                resultError = error;
            }
        }).finally(() => {
            if(globalSpinner){
                decreaseLoader();
            }
        });

        /**
         * Es importante controlar si ha ocurrido un error y devolver un throw o un Promise.reject de forma que cuando
         * se invoque la función callApi y se haga un .then solo se ejecute si la petición ha tenido exito. De esta forma,
         * no hay que controlar los errores cada vez que se utiliza callApi y se pueden recibir en el .catch del .then.
         */
        if(resultError){
            return Promise.reject(resultError);
        }
        return data;
    }, [increaseLoader, decreaseLoader, errorHandler]);

    

    return callApi;
};

export enum ApiMethod {
    POST = 'POST',
    GET = 'GET',
    PUT = 'PUT',
    DELETE = 'DELETE'
};

//#region Funciones base para las peticiones a la api

async function get(url: string) {
    const response = await fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        },
        credentials: 'include'
    });
    return response;
}

async function post(url: string, body: object = {}) {
    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
        credentials: 'include'
    });
    return response;
}

async function put(url: string, body: object = {}) {
    const response = await fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
        credentials: 'include'
    });
    return response;
}

async function remove(url: string) {
    const response = await fetch(url, {
        method: 'DELETE',
        headers: {},
        credentials: 'include'
    });
    return response;
}

//#endregion
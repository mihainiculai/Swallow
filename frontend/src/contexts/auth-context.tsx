import { createContext, useContext, useEffect, useReducer, useRef } from 'react';
import PropTypes from 'prop-types';
import { axiosInstance } from '@/components/axiosInstance';

const HANDLERS = {
    INITIALIZE: 'INITIALIZE',
    SIGN_IN: 'SIGN_IN',
    SIGN_OUT: 'SIGN_OUT'
};

export type User = {
    email: string;
    firstName: string;
    lastName: string;
    profilePictureURL?: string;
};

interface State {
    isAuthenticated: boolean;
    isLoading: boolean;
    user: User | null;
}

interface AuthProviderProps {
    children: React.ReactNode;
}

type Action = {
    type: string;
    payload?: any;
};

const initialState: State = {
    isAuthenticated: false,
    isLoading: true,
    user: null
};

export interface AuthContextType {
    signIn: (email: string, password: string, reCaptchaToken: string) => Promise<void>;
    signInWithGoogle: (accessToken: string) => Promise<void>;
    signUp: (email: string, passwird:string, firstName: string, lastNmae: string, reCaptchaToken: string) => Promise<void>;
    signOut: () => Promise<void>;
    isAuthenticated: boolean;
    isLoading: boolean;
    user: User | null;
}

const handlers = {
    [HANDLERS.INITIALIZE]: (state: State, action: Action) => {
        const user = action.payload;

        return {
            ...state,
            isAuthenticated: Boolean(user),
            isLoading: false,
            user
        };
    },
    [HANDLERS.SIGN_IN]: (state: State, action: Action) => {
        const user = action.payload;

        return {
            ...state,
            isAuthenticated: true,
            user
        };
    },
    [HANDLERS.SIGN_OUT]: (state: State) => {
        return {
            ...state,
            isAuthenticated: false,
            user: null
        };
    }
};

const reducer = (state: State, action: Action) => (
    handlers[action.type] ? handlers[action.type](state, action) : state
);

export const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: AuthProviderProps) => {
    const [state, dispatch] = useReducer(reducer, initialState);
    const initialized = useRef(false);

    const initialize = async () => {

        if (initialized.current) {
            return;
        }

        initialized.current = true;
        try {
            const { data } = await axiosInstance.get('auth/me');
            dispatch({
                type: HANDLERS.INITIALIZE,
                payload: data
            });
        } catch (error) {
            dispatch({
                type: HANDLERS.INITIALIZE
            });
        }
    };

    useEffect(() => {
        initialize();
        const interval = setInterval(async () => {
            await initialize();
        }, 10 * 60 * 1000);

        return () => clearInterval(interval);
    }, []);

    const signIn = async (email: string, password: string, reCaptchaToken: string) => {
        try {
            const { data } = await axiosInstance.post('auth/login', { email, password, reCaptchaToken });
            dispatch({
                type: HANDLERS.SIGN_IN,
                payload: data
            });
        } catch (error) {
            throw error;
        }
    };

    const signInWithGoogle = async (accessToken: string) => {
        try {
            const { data } = await axiosInstance.post('auth/google', { accessToken });
            dispatch({
                type: HANDLERS.SIGN_IN,
                payload: data
            });
        } catch (error) {
            throw error;
        }
    };

    const signUp = async (email: string, password: string, firstName: string, lastName: string, reCaptchaToken: string) => {
        try {
            const { data } = await axiosInstance.post('auth/register', { email, password, firstName, lastName, reCaptchaToken });
            dispatch({
                type: HANDLERS.SIGN_IN,
                payload: data
            });
        } catch (error) {
            throw error;
        }
    };

    const signOut = async () => {
        try {
            await axiosInstance.post('auth/logout');
            dispatch({
                type: HANDLERS.SIGN_OUT
            });
        } catch (error) {
            console.error(error);
        }
    };

    return (
        <AuthContext.Provider
            value={{
                ...state,
                signIn,
                signInWithGoogle,
                signUp,
                signOut
            }}
        >
            {children}
        </AuthContext.Provider>
    );
};

AuthProvider.propTypes = {
    children: PropTypes.node
};

export const AuthConsumer = AuthContext.Consumer;

export const useAuthContext = () => useContext(AuthContext);

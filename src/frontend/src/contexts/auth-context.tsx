"use client";

import React, { ReactNode, createContext, useContext, useEffect, useReducer } from 'react';

import type { User } from '@/types/user';
import PropTypes from 'prop-types';

import { axiosInstance } from '@/components/utilities/axiosInstance';
import useSWR, { mutate } from 'swr'
import { fetcher } from '@/components/utilities/fetcher';

const HANDLERS = {
    INITIALIZE: 'INITIALIZE',
    SIGN_IN: 'SIGN_IN',
    SIGN_OUT: 'SIGN_OUT'
};

interface State {
    isAuthenticated: boolean;
    isLoading: boolean;
    user: User | null;
}

interface AuthProviderProps {
    children: ReactNode;
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
    signUp: (email: string, password: string, firstName: string, lastName: string, reCaptchaToken: string) => Promise<void>;
    signOut: () => Promise<void>;
    refreshUser: () => void;
    isAuthenticated: boolean;
    isLoading: boolean;
    user: User | null;
}

function User() {
    const { data, error, isValidating, mutate } = useSWR('auth/me', fetcher, {
        refreshInterval: 1000 * 60 * 10,
        shouldRetryOnError: false
    });

    return {
        user: data,
        isLoading: isValidating,
        isSignedIn: !error,
        refresh: mutate
    }
}

const handlers = {
    [HANDLERS.INITIALIZE]: (state: State, action: Action) => {
        const user = action.payload;

        if (user) {
            user.fullName = `${user.firstName} ${user.lastName}`;
        }

        return {
            ...state,
            isAuthenticated: Boolean(user),
            isLoading: false,
            user
        };
    },
    [HANDLERS.SIGN_IN]: (state: State) => {
        mutate('auth/me');
        return {
            ...state,
            isAuthenticated: false,
            isLoading: true,
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
    const { user, isSignedIn, isLoading, refresh } = User();

    useEffect(() => {
        if (isLoading) {
            return;
        }

        dispatch({
            type: HANDLERS.INITIALIZE,
            payload: isSignedIn ? user : null
        });
    }, [user, isSignedIn, isLoading]);

    const signIn = async (email: string, password: string, reCaptchaToken: string) => {
        await axiosInstance.post('auth/login', { email, password, reCaptchaToken });
        dispatch({
            type: HANDLERS.SIGN_IN,
        });
    };

    const signInWithGoogle = async (accessToken: string) => {
        await axiosInstance.post('auth/google', { accessToken });
        dispatch({
            type: HANDLERS.SIGN_IN,
        });
    };

    const signUp = async (email: string, password: string, firstName: string, lastName: string, reCaptchaToken: string) => {
        await axiosInstance.post('auth/register', { email, password, firstName, lastName, reCaptchaToken });
        dispatch({
            type: HANDLERS.SIGN_IN,
        });

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

    const refreshUser = () => {
        refresh();
    }

    return (
        <AuthContext.Provider value={{ ...state, signIn, signInWithGoogle, signUp, signOut, refreshUser }} >
            {children}
        </AuthContext.Provider>
    );
};

AuthProvider.propTypes = {
    children: PropTypes.node
};

export const AuthConsumer = AuthContext.Consumer;
export const useAuthContext = () => useContext(AuthContext);

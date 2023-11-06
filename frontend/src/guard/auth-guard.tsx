"use client";

import { ReactNode, useEffect, useRef, useState, FC } from 'react';
import { useRouter } from 'next/navigation';

import { useAuthContext } from '@/contexts/auth-context';

interface AuthContextType {
    isAuthenticated: boolean;
    isLoading: boolean;
}

interface AuthGuardProps {
    children: ReactNode;
}

export const AuthGuard: FC<AuthGuardProps> = (props) => {
    const { children } = props;
    const router = useRouter();
    const { isAuthenticated, isLoading } = useAuthContext() as AuthContextType;
    const ignore = useRef(false);
    const [checked, setChecked] = useState(false);

    useEffect(
        () => {
            if (ignore.current) {
                return;
            }

            if (isLoading) {
                return;
            }

            ignore.current = true;

            if (!isAuthenticated) {
                console.log('Not authenticated, redirecting');
                router.push('/auth/login');
            } else {
                setChecked(true);
            }
        },
        [isAuthenticated, isLoading, router]
    );

    if (!checked) {
        return null;
    }

    return <>{children}</>;
};

"use client";

import { ReactNode, useEffect, useRef, useState, FC } from 'react';
import { useRouter } from 'next/navigation';

import { useAuthContext, AuthContextType } from '@/contexts/auth-context';

interface AuthGuardProps {
    children: ReactNode;
}

export const AuthGuard: FC<AuthGuardProps> = (props) => {
    const { children } = props;
    const router = useRouter();
    const { isAuthenticated, isLoading } = useAuthContext() as AuthContextType;
    const ignore = useRef(false);
    const [checked, setChecked] = useState(false);

    useEffect(() => {


        if (isLoading) {
            return;
        }


        if (!isAuthenticated) {
            setChecked(false);
            console.log('Not authenticated, redirecting');
            router.push('/auth/login');
        } else {
            setChecked(true);
        }
    }, [isAuthenticated, isLoading, router]);

    if (!checked) {
        return null;
    }

    return <>{children}</>;
};

"use client";

import React, { ReactNode, JSX, useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';

import { useAuthContext, AuthContextType } from '@/contexts/auth-context';

export interface LoginGuardProps {
    children: ReactNode;
}

export function LoginGuard({ children }: LoginGuardProps): JSX.Element | null {
    const router = useRouter();
    const { isAuthenticated, isLoading } = useAuthContext() as AuthContextType;
    const [checked, setChecked] = useState<boolean>(false);

    useEffect(() => {
            if (isLoading) {
                return;
            }

            if (isAuthenticated) {
                setChecked(false);
                router.push('/dashboard');
                return;
            }

            setChecked(true);
        }, [isAuthenticated, isLoading, router]
    );

    if (!checked) {
        return null;
    }

    return <>{children}</>;
}

"use client";

import React, { ReactNode, JSX, useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';

import { useAuthContext, AuthContextType } from '@/contexts/auth-context';

export interface AuthGuardProps {
    children: ReactNode;
}

export function AuthGuard({ children }: AuthGuardProps): JSX.Element | null {
    const router = useRouter();
    const { isAuthenticated, isLoading } = useAuthContext() as AuthContextType;
    const [checked, setChecked] = useState<boolean>(false);

    useEffect(() => {
        if (isLoading) {
            return;
        }

        if (!isAuthenticated) {
            setChecked(false);
            router.push('/auth/login');
            return;
        }

        setChecked(true);
    }, [isAuthenticated, isLoading, router]);

    if (!checked) {
        return null;
    }

    return <>{children}</>;
}

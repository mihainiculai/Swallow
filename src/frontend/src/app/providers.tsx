'use client'

import { useRouter } from 'next/navigation'
import { NextUIProvider } from '@nextui-org/react'
import { ThemeProvider } from "next-themes";
import { AuthProvider } from '@/contexts/auth-context';

export function Providers({ children }: { children: React.ReactNode }) {
    const router = useRouter();

    return (
        <AuthProvider>
            <NextUIProvider navigate={router.push}>
                <ThemeProvider attribute="class" defaultTheme="system" enableSystem={true}>
                    {children}
                </ThemeProvider>
            </NextUIProvider>
        </AuthProvider>
    )
}
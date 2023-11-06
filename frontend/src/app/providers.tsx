'use client'

import { useRouter } from 'next/navigation'
import { NextUIProvider } from '@nextui-org/react'
import { ThemeProvider } from "next-themes";
import { GoogleOAuthProvider } from '@react-oauth/google';
import { AuthProvider } from '@/contexts/auth-context';

const googleClientId = process.env.NEXT_PUBLIC_GOOGLE_CLIENT_ID

export function Providers({ children }: { children: React.ReactNode }) {
    const router = useRouter();

    return (
        <AuthProvider>
            <GoogleOAuthProvider clientId={googleClientId as string}>
                <NextUIProvider navigate={router.push}>
                    <ThemeProvider attribute="class" defaultTheme="system" enableSystem={true}>
                        {children}
                    </ThemeProvider>
                </NextUIProvider>
            </GoogleOAuthProvider>
        </AuthProvider>
    )
}
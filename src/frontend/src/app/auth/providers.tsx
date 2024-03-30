'use client'

import React, { ReactNode } from "react";
import { GoogleOAuthProvider } from '@react-oauth/google';

export function Providers({ children }: { children: ReactNode }) {

    return (
        <GoogleOAuthProvider clientId={process.env.NEXT_PUBLIC_GOOGLE_CLIENT_ID as string}>
            {children}
        </GoogleOAuthProvider>
    )
}
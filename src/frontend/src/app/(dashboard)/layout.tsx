import React, { ReactNode } from "react";
import { AuthGuard } from "@/guard/auth-guard"
import { NavigationBar } from "./_components/Navbar/navbar"

export default function DashboardLayout({ children }: { children: ReactNode }) {
    return (
        <AuthGuard>
            <NavigationBar />
            <main>
                {children}
            </main>
        </AuthGuard>
    )
}

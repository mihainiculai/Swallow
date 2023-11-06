"use client";

import { useAuthContext } from "@/contexts/auth-context"

export default function Dashboard() {
    const { user } = useAuthContext();
    return (
        <div>
            <h1>Dashboard</h1>
            <p>Welcome, {user?.email}</p>
        </div>
    )
}
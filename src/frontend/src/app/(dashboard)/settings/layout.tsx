"use client";

import React from 'react'
import Typography from "@/components/Typography";
import { Menubar } from './_components/menu-bar';

export default function SettingsLayout({ children }: { children: React.ReactNode }) {
    return (
        <div className="flex flex-col p-6 max-w-7xl mx-auto">
            <Typography size="sm" variant="title" fullWidth className="mb-4">
                Settings
            </Typography>
            <Menubar />
            {children}
        </div>
    )
}

"use client";

import React from 'react'
import Typography from "@/components/ui-elements/typography";
import { Menubar } from './_components/menu-bar';

export default function SettingsLayout({ children }: { children: React.ReactNode }) {
    return (
        <div className="flex flex-col p-6 max-w-7xl mx-auto">
            <header className="flex items-center justify-between mt-6">
                <Typography size="sm" variant="title" fullWidth className="mb-4">
                    Administration
                </Typography>
            </header>
            <Menubar />
            {children}
        </div>
    )
}

"use client";

import React, { useState, useEffect } from 'react'
import { usePathname } from 'next/navigation'
import { useRouter } from 'next/navigation'
import { Tabs, Tab } from "@nextui-org/react";
import { MdAccountCircle, MdWallet, MdLock, MdSettings } from "react-icons/md";

export const Menubar = () => {
    const pathname: string = usePathname();
    const router = useRouter();

    const [selected, setSelected] = useState('account');

    const getSelectionFromPath = (path: string) => {
        const match = path.match(/\/settings\/(.+)/);
        return match ? match[1] : 'account';
    };

    useEffect(() => {
        setSelected(getSelectionFromPath(pathname));
    }, [pathname]);

    const handleSelectionChange = (key: any) => {
        router.push(`/settings/${key}`);
    }

    return (
        <div className="overflow-x-auto whitespace-nowrap hide-scrollbar">
            <Tabs
                aria-label="Options"
                color="primary"
                variant="light"
                selectedKey={selected}
                onSelectionChange={handleSelectionChange}
                className="my-4"
            >
                <Tab
                    key="account"
                    title={
                        <div className="flex items-center space-x-2">
                            <MdAccountCircle />
                            <span>Account</span>
                        </div>
                    }
                />
                <Tab
                    key="security"
                    title={
                        <div className="flex items-center space-x-2">
                            <MdLock />
                            <span>Security</span>
                        </div>
                    }
                />
                <Tab
                    key="preferences"
                    title={
                        <div className="flex items-center space-x-2">
                            <MdSettings />
                            <span>Preferences</span>
                        </div>
                    }
                />
                <Tab
                    key="membership"
                    title={
                        <div className="flex items-center space-x-2">
                            <MdWallet />
                            <span>Membership</span>
                        </div>
                    }
                />
            </Tabs>
        </div>
    )
}
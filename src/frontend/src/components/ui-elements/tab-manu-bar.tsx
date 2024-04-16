"use client";

import React, { useState, useEffect } from 'react'
import { usePathname } from 'next/navigation'
import { useRouter } from 'next/navigation'
import { Tabs, Tab } from "@nextui-org/react";

interface Item {
    key: string;
    title: string;
    icon: any;
}

interface TabMenubarProps {
    items: Item[];
    basePath: string;
}

export const TabMenubar = ({ items, basePath }: TabMenubarProps) => {
    const pathname: string = usePathname();
    const router = useRouter();

    const [selected, setSelected] = useState(items[0].key);

    const getSelectionFromPath = (path: string) => {
        const match = path.match(new RegExp(`${basePath}/(.+)`));
        return match ? match[1] : 'account';
    };

    useEffect(() => {
        setSelected(getSelectionFromPath(pathname));
        //eslint-disable-next-line
    }, [pathname]);

    const handleSelectionChange = (key: any) => {
        router.push(`${basePath}/${key}`);
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
                {items.map((item) => (
                    <Tab
                        key={item.key}
                        title={
                            <div className="flex items-center space-x-2">
                                {item.icon}
                                <span>{item.title}</span>
                            </div>
                        }
                    />
                ))}
            </Tabs>
        </div>
    )
}
import React from "react";
import { Link } from "@nextui-org/react";
import { NavbarItem, NavbarMenuItem } from "@nextui-org/react";


export const MenuItem: React.FC<{ name: string; path: string }> = ({ name, path }) => {
    return (
        <NavbarMenuItem>
            <Link href={path} color="foreground" className="w-full py-2" >
                {name}
            </Link>
        </NavbarMenuItem>
    )
}

export const BarItem: React.FC<{ name: string; path: string; isActive: boolean; className?: string }> = ({ name, path, isActive, className }) => {
    return (
        <NavbarItem isActive={isActive} className={className}>
            <Link href={path} className={`${isActive ? "text-primary" : "text-inherit"} hover:text-primary`}>
                {name}
            </Link>
        </NavbarItem>
    );
}
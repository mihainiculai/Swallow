"use client";

import React, { useState } from "react";

import { usePathname } from "next/navigation";

import { Navbar, NavbarContent, Button, NavbarMenuToggle, NavbarMenu, Link } from "@nextui-org/react";

import { Logo } from "@/components/logo";
import { CiSearch } from "react-icons/ci";
import { RiSparklingLine } from "react-icons/ri";
import { NavbarConfig, NavbarItemType } from "@/config/dashboard/navbar/navbar";
import { MenuItem, BarItem } from "@/components/ui-elements/navbar";
import { DropdownAvatar } from "./dropdown-avatar";

import { useAuthContext, AuthContextType } from "@/contexts/auth-context";

export const NavigationBar: React.FC = () => {
    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false);
    const location = usePathname();

    const isActive = (item: string): boolean => location === item;

    const { user } = useAuthContext() as AuthContextType

    return (
        <Navbar maxWidth="xl" onMenuOpenChange={setIsMenuOpen}>
            <NavbarContent>
                <NavbarMenuToggle
                    aria-label={isMenuOpen ? "Close menu" : "Open menu"}
                    className="sm:hidden"
                />

                <Link href="/dashboard" className="flex items-center gap-2 mr-6">
                    <Logo width={32} height={32} className="mr-2" />
                    <p className="hidden sm:block font-bold text-inherit">Swallow</p>
                </Link>

                <div className="hidden sm:flex gap-4">
                    {NavbarConfig.items.map((item: NavbarItemType) => (
                        <BarItem key={item.name} className="hidden sm:block" name={item.name} path={item.path} isActive={isActive(item.path)} />
                    ))}
                </div>
            </NavbarContent>

            <NavbarContent as="div" className="items-center" justify="end">
                {user?.planId === 1 && (
                    <Button
                        className="bg-gradient-to-tr from-[#F05121] to-[#FD8524] text-white"
                        startContent={<RiSparklingLine size={18} />}
                        as={Link}
                        href="/settings/membership"
                    >
                        Go Premium
                    </Button>
                )}
                <Button
                    variant="flat"
                    startContent={<CiSearch size={18} />}
                    className="text-default-500 w-40 lg:w-52 hidden md:inline-flex"
                >
                    Tap to search...
                </Button>
                <Button isIconOnly variant="flat" className="md:hidden">
                    <CiSearch size={18} />
                </Button>
                <DropdownAvatar />
            </NavbarContent>

            <NavbarMenu className="px-4 py-8">
                {NavbarConfig.items.map((item: NavbarItemType) => (
                    <MenuItem key={item.name} name={item.name} path={item.path} />
                ))}
            </NavbarMenu>
        </Navbar>
    )
}
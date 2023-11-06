"use client";

import React, { useState, useEffect } from "react";
import { usePathname } from "next/navigation";
import Link from 'next/link'
import { Navbar, NavbarBrand, NavbarContent, NavbarItem, NavbarMenu, NavbarMenuItem, NavbarMenuToggle, Button } from "@nextui-org/react";
import { Logo } from "@/components/Logo";
import { NavbarConfig } from "@/config/landing/navbar";

const MenuItem: React.FC<{ name: string; path: string }> = ({ name, path }) => {
    return (
        <NavbarMenuItem>
            <Link href={path} color="foreground" className="w-full py-2" >
                {name}
            </Link>
        </NavbarMenuItem>
    )
}

const BarItem: React.FC<{ name: string; path: string; isActive: boolean; className?: string }> = ({ name, path, isActive, className }) => {
    return (
        <NavbarItem isActive={isActive} className={className}>
            <Link href={path} className={`${isActive ? "text-primary" : "text-inherit"}`}>
                {name}
            </Link>
        </NavbarItem>
    );
}

export const NavigationBar: React.FC = () => {
    const [isMenuOpen, setIsMenuOpen] = useState(false);
    const location = usePathname();

    const isHome = location === '/home';
    const isActive = (item: string): boolean => location === item;

    const [isNavbarBlurred, setIsNavbarBlurred] = useState(false);

    useEffect(() => {
        window.addEventListener("scroll", () => {
            const scrollY = window.scrollY;
            if (scrollY > 1) {
                setIsNavbarBlurred(true);
            } else {
                setIsNavbarBlurred(false);
            }
        });
    }, []);

    return (
        <Navbar
            shouldHideOnScroll
            onMenuOpenChange={setIsMenuOpen}
            maxWidth="xl"
            position={isHome ? "static" : "sticky"}
            isBlurred={!isHome}
            className={`${isHome ? "sm:fixed sm:top-0 sm:z-50 sm:bg-transparent transition-backdrop-filter duration-300" + (isNavbarBlurred ? ' backdrop-blur-md' : '') : ""}`}
        >
            <NavbarContent>
                <NavbarMenuToggle
                    aria-label={isMenuOpen ? "Close menu" : "Open menu"}
                    className="sm:hidden"
                />

                <NavbarBrand>
                    <Link href="/home" className="flex items-center gap-2">
                        <Logo width={32} height={32} theme={location === '/home' ? "dark" : undefined} />
                        <p className="font-bold text-inherit">Swallow</p>
                    </Link>
                </NavbarBrand>
            </NavbarContent>

            <NavbarContent className="sm:flex gap-6" justify="end">
                {NavbarConfig.items.map((item, index) => (
                    <BarItem key={index} className="hidden sm:block" name={item.name} path={item.path} isActive={isActive(item.path)} />
                ))}

                <NavbarItem className="block sm:inline-block">
                    <Button as={Link} color="primary" href="/auth/login" variant="flat">
                        Get Started
                    </Button>
                </NavbarItem>
            </NavbarContent>

            <NavbarMenu className="px-4 py-8">
                {NavbarConfig.items.map((item, index) => (
                    <MenuItem key={index} name={item.name} path={item.path} />
                ))}
            </NavbarMenu>
        </Navbar>
    );
}
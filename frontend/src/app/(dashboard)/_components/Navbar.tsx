"use client";

import React from "react";
import Link from 'next/link'
import { useRouter } from 'next/navigation'
import { useAuthContext, AuthContextType } from "@/contexts/auth-context";
import { Navbar, NavbarContent, NavbarItem, Input, DropdownItem, DropdownTrigger, Dropdown, DropdownMenu, Button, Avatar, DropdownSection } from "@nextui-org/react";
import { Logo } from "@/components/Logo";
import { MdSearch } from "react-icons/md";
import { RiSparklingLine } from "react-icons/ri";
import { DropdownConfig, DropdownItemType } from "@/config/dashboard/navbar/dropdown";

export const NavigationBar: React.FC = () => {
    const { user, signOut } = useAuthContext() as AuthContextType
    const router = useRouter()

    const handleSignOut = () => {
        try {
            signOut()
            router.push('/home')
        } catch (error) {
            // TODO: Handle error
        }
    }

    return (
        <Navbar maxWidth="xl">
            <NavbarContent justify="start">
                <Link href="/dashboard" className="flex items-center gap-2 mr-6">
                    <Logo width={32} height={32} className="mr-2" />
                    <p className="hidden sm:block font-bold text-inherit">Swallow</p>
                </Link>
                <NavbarContent className="hidden sm:flex gap-3">
                    <NavbarItem>
                        <Link color="foreground" href="#">
                            Dashboard
                        </Link>
                    </NavbarItem>
                    <NavbarItem isActive>
                        <Link href="#" aria-current="page" color="secondary">
                            Discover
                        </Link>
                    </NavbarItem>
                    <NavbarItem>
                        <Link color="foreground" href="#">
                            Integrations
                        </Link>
                    </NavbarItem>
                </NavbarContent>
            </NavbarContent>

            <NavbarContent as="div" className="items-center" justify="end">
                <Button radius="full" className="bg-gradient-to-tr from-[#F05121] to-[#FD8524] text-white shadow-lg" startContent={<RiSparklingLine size={18} />}>
                    Go Premium
                </Button>
                <Input
                    classNames={{
                        base: "max-w-full sm:max-w-[15rem] h-10",
                        mainWrapper: "h-full",
                        input: "text-small",
                        inputWrapper: "h-full font-normal text-default-500 bg-default-400/20 dark:bg-default-500/20",
                    }}
                    placeholder="Type to search..."
                    size="sm"
                    startContent={<MdSearch size={18} />}
                    type="search"
                />
                <Dropdown placement="bottom-end">
                    <DropdownTrigger>
                        <Avatar
                            isBordered
                            as="button"
                            className="transition-transform"
                            color="primary"
                            name={user?.fullName}
                            size="sm"
                            src={user?.profilePictureURL}
                        />
                    </DropdownTrigger>
                    <DropdownMenu aria-label="Profile Actions" variant="flat">
                        <DropdownItem key="profile" className="h-14 gap-2">
                            <p className="font-semibold">Signed in as</p>
                            <p className="font-semibold">{user?.fullName}</p>
                        </DropdownItem>
                        <DropdownItem key="upgrade" className="bg-gradient-to-tr from-[#F05121] to-[#FD8524] text-white shadow-lg text-center">
                            Upgrade your plan
                        </DropdownItem>

                        <DropdownSection showDivider className="mt-2">
                            {DropdownConfig.items.map((item: DropdownItemType) => (
                                <DropdownItem key={item.name} as={Link} href={item.path}>
                                    {item.name}
                                </DropdownItem>
                            ))}
                        </DropdownSection>

                        <DropdownSection showDivider>
                            <DropdownItem key="settings" as={Link} href="/dashboard/settings">
                                Administration
                            </DropdownItem>
                        </DropdownSection>

                        <DropdownItem key="logout" className="text-danger" color="danger" onClick={handleSignOut}>
                            Log Out
                        </DropdownItem>
                    </DropdownMenu>
                </Dropdown>
            </NavbarContent>
        </Navbar>
    )
}
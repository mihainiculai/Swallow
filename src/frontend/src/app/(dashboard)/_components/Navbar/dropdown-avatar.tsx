import React from 'react'

import Link from 'next/link';
import { useRouter } from 'next/navigation'

import { Avatar, Dropdown, DropdownTrigger, DropdownMenu, DropdownItem, DropdownSection } from "@nextui-org/react";

import { useAuthContext, AuthContextType } from "@/contexts/auth-context";
import { DropdownConfig, DropdownItemType } from "@/config/dashboard/navbar/dropdown";

export const DropdownAvatar: React.FC = () => {
    const router = useRouter()
    const { user, signOut } = useAuthContext() as AuthContextType

    const handleSignOut = () => {
        try {
            signOut()
            router.push('/home')
        } catch (error) {
            // TODO: Handle error
        }
    }

    return (
        <Dropdown placement="bottom-end">
            <DropdownTrigger>
                <Avatar
                    isBordered
                    as="button"
                    className="transition-transform"
                    color="primary"
                    name={user?.fullName}
                    size="sm"
                    src={user?.profilePictureUrl}
                />
            </DropdownTrigger>
            <DropdownMenu aria-label="Profile Actions" variant="flat">
                <DropdownItem key="profile" textValue="Signed in as" className="h-14 gap-2">
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
                    <DropdownItem key="settings" as={Link} href="/administration">
                        Administration
                    </DropdownItem>
                </DropdownSection>

                <DropdownItem key="logout" className="text-danger" color="danger" onClick={handleSignOut}>
                    Log Out
                </DropdownItem>
            </DropdownMenu>
        </Dropdown>
    )
}

"use client"

import { Logo } from '@/components/logo';
import { Button } from '@nextui-org/react';
import { RiSparklingLine } from 'react-icons/ri';
import Link from 'next/link';
import { useAuthContext, AuthContextType } from "@/contexts/auth-context";

export const Navbar = () => {
    const { user } = useAuthContext() as AuthContextType
    
    return (
        <div className='flex flex-row gap-6 w-full justify-between'>
            <div className='flex items-center gap-4'>
                <Link href="/dashboard">
                    <Logo width={32} height={32} className="mr-2" />
                </Link>
                <p className="font-bold text-inherit">Swallow Itinerary</p>
            </div>

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
        </div>
    )
}
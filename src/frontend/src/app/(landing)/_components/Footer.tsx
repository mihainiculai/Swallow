import React from "react";
import { Logo } from "@/components/Logo";
import Link from 'next/link'
import { Divider } from "@nextui-org/react";
import { FooterConfig } from "@/config/landing/footer";

const FooterItem: React.FC<{ name: string; path: string }> = ({ name, path }) => {
    return (
        <li>
            <Link href={path} color="foreground" className='mr-4 md:mr-6 hover:text-primary' >
                {name}
            </Link>
        </li>
    );
}

const currentYear = new Date().getFullYear();

export const Footer = () => {
    return (
        <footer className='rounded-lg shadow'>
            <div className='w-full max-w-screen-xl mx-auto p-4 md:py-8'>
                <div className='sm:flex sm:items-center sm:justify-between'>
                    <Link href="/home" className="flex items-center mb-4 sm:mb-0">
                        <Logo width={32} height={32} className="mr-3" />
                        <span className='self-center text-2xl font-semibold whitespace-nowrap dark:text-white'>
                            Swallow
                        </span>
                    </Link>
                    <ul className='flex flex-wrap items-center mb-6 text-sm font-medium text-gray-500 sm:mb-0 dark:text-gray-400'>
                        {FooterConfig.items.map((item, index) => (
                            <FooterItem key={index} name={item.name} path={item.path} />
                        ))}
                    </ul>
                </div>
                <Divider className="my-8" />
                <span className='block text-center text-sm text-gray-500 dark:text-gray-400 p-4'>
                    © {currentYear}{' '}
                    <Link href='/home' className='hover:underline hover:text-primary'>
                        Swallow™
                    </Link>
                    . All Rights Reserved.
                </span>
            </div>
        </footer>
    );
};

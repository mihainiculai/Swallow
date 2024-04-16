import React from "react"

import Link from 'next/link'
import Image from "next/image"

import { Logo } from "@/components/logo"

export default function AuthLayout({ children }: { children: React.ReactNode }) {
    return (
        <div className="flex flex-col md:flex-row h-screen">
            <div className="hidden md:block flex-1 h-full">
                <div className="w-full h-full relative ">
                    <Image
                        fill
                        sizes="100%"
                        src="/auth/auth-cover.webp"
                        alt="Hero Cover"
                        className="object-cover object-center"
                        priority
                    />
                </div>
            </div>

            <div className="md:w-1/2 lg:w-1/3 overflow-auto">
                <div className="flex flex-col justify-start items-start p-8">
                    <Link href="/home">
                        <div className="flex items-center space-x-2 mb-16" >
                            <Logo width={32} height={32} />
                            <h1 className="text-xl font-bold">Swallow</h1>
                        </div>
                    </Link>
                    <div className="w-full max-w-md mx-auto space-y-10">
                        {children}
                    </div>
                </div>
            </div>
        </div>
    )
}

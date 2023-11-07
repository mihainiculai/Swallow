import React from "react"
import { LoginGuard } from "@/guard/login-guard"
import Image from "next/image"
import { Logo } from "@/components/Logo"
import { Providers } from "./providers"

export default function AuthLayout({ children }: { children: React.ReactNode }) {
    return (
        <Providers>
            <LoginGuard>
                <div className="flex flex-col md:flex-row md:h-screen">
                    <div className="flex-1 flex justify-center items-center">
                        <div className="w-full h-full relative">
                            <Image
                                fill
                                sizes="100%"
                                priority
                                src="/auth/auth-cover.webp"
                                alt="Hero Cover"
                                className="object-cover object-center"
                            />
                        </div>
                    </div>

                    <div className="md:w-1/2 lg:w-1/3 overflow-auto">
                        <div className="flex flex-col justify-start items-start p-8">
                            <div className="flex items-center space-x-2 mb-16">
                                <Logo width={32} height={32} />
                                <h1 className="text-xl font-bold">Swallow</h1>
                            </div>
                            <div className="w-full max-w-md mx-auto space-y-10">
                                {children}
                            </div>
                        </div>
                    </div>
                </div>
            </LoginGuard>
        </Providers>
    )
}

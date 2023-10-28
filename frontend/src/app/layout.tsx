import type { Metadata } from 'next'
import { Providers } from "./providers";
import { siteConfig } from "@/config/site";
import './globals.css'

export const metadata: Metadata = {
    title: siteConfig.name,
    description: siteConfig.description,
    keywords: siteConfig.keywords,
    authors: siteConfig.author,
}

export default function RootLayout({ children }: { children: React.ReactNode }) {
    return (
        <html lang="en" suppressHydrationWarning>
            <body>
                <Providers>
                    {children}
                </Providers>
            </body>
        </html>
    )
}

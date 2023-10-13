import type { Metadata } from 'next'

export const metadata: Metadata = {
    title: 'Swallow',
    description: 'A intelligent trip planner',
}

export default function RootLayout({
    children,
}: {
    children: React.ReactNode
}) {
    return (
        <html lang="en">
            <body>{children}</body>
        </html>
    )
}

import { AuthGuard } from "@/guard/auth-guard"

export default function DashboardLayout({ children }: { children: React.ReactNode }) {
    return (
        <AuthGuard>
            {children}
        </AuthGuard>
    )
}

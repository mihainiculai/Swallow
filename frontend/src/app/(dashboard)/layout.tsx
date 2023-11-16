import { AuthGuard } from "@/guard/auth-guard"
import { NavigationBar } from "./_components/Navbar"

export default function DashboardLayout({ children }: { children: React.ReactNode }) {
    return (
        <AuthGuard>
            <NavigationBar />
            {children}
        </AuthGuard>
    )
}

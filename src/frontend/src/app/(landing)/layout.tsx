import { NavigationBar } from "./_components/Navbar"
import { Footer } from "./_components/Footer"

export default function LandingLayout({ children }: { children: React.ReactNode }) {
    return (
        <div>
            <NavigationBar />
            {children}
            <Footer />
        </div>
    )
}

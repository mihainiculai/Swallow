"use client";

import { useTheme } from "next-themes";
import Image from "next/image";

interface LogoProps {
    width: number;
    height: number;
    theme?: "light" | "dark";
    className?: string;
}

export const Logo: React.FC<LogoProps> = ({ width, height, theme, className }) => {
    const { resolvedTheme } = useTheme()

    const selectedTheme = theme || resolvedTheme;
    const logoPath = `logo/${selectedTheme === "dark" ? "dark-logo.svg" : "light-logo.svg"}`;

    return (
        <Image
            src={logoPath}
            width={width}
            height={height}
            className={className}
            alt="Swallow Logo"
        />
    )
}
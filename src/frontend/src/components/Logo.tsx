"use client";

import React, { FC, useState, useEffect } from "react";
import { useTheme } from "next-themes";
import Image from "next/image";

interface LogoProps {
    width: number;
    height: number;
    theme?: "light" | "dark";
    className?: string;
}

export const Logo: FC<LogoProps> = ({ width, height, theme, className }) => {
    const { resolvedTheme } = useTheme();
    const [logoPath, setLogoPath] = useState<string | null>(null);

    useEffect(() => {
        const selectedTheme = theme || resolvedTheme;
        setLogoPath(`/logo/${selectedTheme === "dark" ? "dark-logo.svg" : "light-logo.svg"}`);
    }, [resolvedTheme, theme]);

    if (!logoPath) return null;

    return (
        <Image
            src={logoPath}
            width={width}
            height={height}
            className={className}
            alt="Swallow Logo"
        />
    );
}
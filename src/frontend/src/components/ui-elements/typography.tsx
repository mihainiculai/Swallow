import React, { FC } from 'react';
import { tv } from "tailwind-variants";

export const titleClasses = tv({
    base: "tracking-tight inline font-bold",
    variants: {
        color: {
            violet: "from-[#FF1CF7] to-[#b249f8]",
            yellow: "from-[#FF705B] to-[#FFB457]",
            blue: "from-[#5EA2EF] to-[#0072F5]",
            cyan: "from-[#00b7fa] to-[#01cfea]",
            green: "from-[#6FEE8D] to-[#17c964]",
            pink: "from-[#FF72E1] to-[#F54C7A]",
            primary: "from-[#FD8524] to-[#F05121]",
            foreground: "dark:from-[#FFFFFF] dark:to-[#4B4B4B]",
        },
        size: {
            sm: "text-3xl lg:text-4xl",
            md: "text-[2.3rem] lg:text-5xl leading-9",
            lg: "text-4xl lg:text-6xl",
        },
        fullWidth: {
            true: "w-full block",
        },
    },
    defaultVariants: {
        size: "md",
    },
    compoundVariants: [
        {
            color: [
                "violet",
                "yellow",
                "blue",
                "cyan",
                "green",
                "pink",
                "primary",
                "foreground",
            ],
            class: "bg-clip-text text-transparent bg-gradient-to-b",
        },
    ],
});

export const subtitleClasses = tv({
    base: "w-full md:w-1/2 my-2 text-lg lg:text-xl text-default-600 block max-w-full",
    variants: {
        fullWidth: {
            true: "!w-full",
        },
    },
    defaultVariants: {
        fullWidth: true
    }
});

interface TypographyProps {
    variant?: 'title' | 'subtitle';
    color?: 'violet' | 'yellow' | 'blue' | 'cyan' | 'green' | 'pink' | 'primary' | 'foreground';
    size?: 'sm' | 'md' | 'lg';
    fullWidth?: boolean;
    className?: string;
    children: React.ReactNode;
}

const Typography: FC<TypographyProps> = ({
    variant = 'title',
    color,
    size = 'md',
    fullWidth = false,
    className = '',
    children,
}) => {
    const classes = variant === 'title'
        ? titleClasses({ color, size, fullWidth })
        : subtitleClasses({ fullWidth });

    return <span className={`${classes} ${className}`}>{children}</span>;
};

export default Typography;
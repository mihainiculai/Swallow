import React from "react";

export const CellWrapper = React.forwardRef<HTMLDivElement, React.HTMLAttributes<HTMLDivElement>>(
    ({ children, className }, ref) => (
        <div
            ref={ref}
            className="flex items-center justify-between gap-2 rounded-medium bg-content2 p-4"
        >
            {children}
        </div>
    ),
);

CellWrapper.displayName = "CellWrapper";
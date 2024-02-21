"use client";

import type { SwitchProps } from "@nextui-org/react";

import React from "react";
import { Switch } from "@nextui-org/react";

export type SwitchCellProps = SwitchProps & {
    label: string;
    description: string;
};

export const SwitchCell = React.forwardRef<HTMLInputElement, SwitchCellProps>(
    ({ label, description }, ref) => (
        <Switch
            ref={ref}
            className="inline-flex bg-content2 flex-row-reverse w-full max-w-full items-center justify-between cursor-pointer rounded-medium gap-2 p-4"
        >
            <div className="flex flex-col">
                <p className="text-medium">{label}</p>
                <p className="text-small text-default-500">{description}</p>
            </div>
        </Switch>
    ),
);

SwitchCell.displayName = "SwitchCell";
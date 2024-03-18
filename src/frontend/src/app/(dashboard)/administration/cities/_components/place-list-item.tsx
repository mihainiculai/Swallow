"use client";

import React, { useState } from "react";
import { Button, Image, Skeleton, Chip } from "@nextui-org/react";

import { FaStar } from "react-icons/fa";

export type PlaceListItemColor = {
    name: string;
    hex: string;
};

export type PlaceItem = {
    attractionId?: number;
    name?: string;
    priority?: number;
    price?: number;
    isNew?: boolean;
    rating?: number;
    ratingCount?: number;
    description?: string;
    pictureUrl?: string;
    categories?: string[];
    schedules?: any[];
};

export type PlaceListItemProps = Omit<React.HTMLAttributes<HTMLDivElement>, "id"> & {
    isPopular?: boolean;
    isLoading?: boolean;
    removeWrapper?: boolean;
} & PlaceItem;

const PlaceListItem = React.forwardRef<HTMLDivElement, PlaceListItemProps>(
    (
        { name, priority, price, rating, isLoading, description, pictureUrl: imageSrc, categories, schedules, removeWrapper, className },
        ref,

    ) => {
        const [showFullDescription, setShowFullDescription] = useState(false);
        const DESCRIPTION_MAX_LENGTH = 135;

        const renderDescription = () => {
            if (description && description.length > DESCRIPTION_MAX_LENGTH && !showFullDescription) {
                return (
                    <>
                        <p className="text-small text-default-500">
                            {`${description.substring(0, DESCRIPTION_MAX_LENGTH)}... `}
                            <Button size="sm" variant="light" onClick={() => setShowFullDescription(true)}>More</Button>
                        </p>
                    </>
                );
            } else if (description) {
                return (
                    <>
                        <p className="text-small text-default-500">{description} </p>
                        {description.length > DESCRIPTION_MAX_LENGTH ? (
                            <Button size="sm" variant="light" onClick={() => setShowFullDescription(false)}>Less</Button>
                        ) : null}
                    </>
                );
            }
            return null;
        };

        return (
            <div
                ref={ref}
                className={
                    "relative flex w-full flex-none flex-col gap-3" +
                    (removeWrapper ? " rounded-none bg-background shadow-none" : "") +
                    (className ? ` ${className}` : "")
                }
            >
                <Image
                    isBlurred
                    isZoomed
                    alt={name}
                    className="aspect-square w-full hover:scale-110"
                    isLoading={isLoading}
                    src={imageSrc || (isLoading ? "" : "/placeholder.svg")}
                />

                <div className="mt-1 flex flex-col gap-2 px-1">
                    {isLoading ? (
                        <div className="my-1 flex flex-col gap-3">
                            <Skeleton className="w-3/5 rounded-lg">
                                <div className="h-3 w-3/5 rounded-lg bg-default-200" />
                            </Skeleton>
                            <Skeleton className="mt-3 w-4/5 rounded-lg">
                                <div className="h-3 w-4/5 rounded-lg bg-default-200" />
                            </Skeleton>
                            <Skeleton className="mt-4 w-2/5 rounded-lg">
                                <div className="h-3 w-2/5 rounded-lg bg-default-300" />
                            </Skeleton>
                        </div>
                    ) : (
                        <>
                            <div className="flex items-start justify-between gap-1">
                                <h3 className="text-small font-medium text-default-700">{priority}. {name}</h3>
                                {rating !== undefined ? (
                                    <div className="flex items-center gap-1">
                                        <FaStar className="text-default-500" />
                                        <span className="text-small text-default-500">{rating}</span>
                                    </div>
                                ) : null}
                            </div>
                            <div className="flex gap-2 flex-wrap">
                                {categories?.map((category, index) => (
                                    <Chip variant="flat" key={index} color="primary" size="sm">
                                        {category}
                                    </Chip>
                                ))}
                            </div>
                            {renderDescription()}
                            {price ? <p className="text-small font-medium text-default-500">${price}</p> : null}
                            {schedules?.length ? (
                                <div className="flex flex-col gap-1">
                                    <h4 className="text-small font-medium text-default-700">Schedule</h4>
                                    <div className="flex flex-col gap-1">
                                        {schedules.map((schedule, index) => (
                                            <p key={index} className="text-small text-default-500">
                                                {
                                                    schedule.weekdayName === "Monday" && schedule.openTime === "00:00:00" ? "Monday - Sunday: 00:00 - 24:00" :
                                                        `${schedule.weekdayName}: ${schedule.openTime.substr(0, 5)} - ${schedule?.closeTime?.substr(0, 5)}`
                                                }
                                            </p>
                                        ))}
                                    </div>
                                </div>
                            ) : null}
                        </>
                    )}
                </div>
            </div>
        );
    },
);

PlaceListItem.displayName = "PlaceListItem";

export default PlaceListItem;
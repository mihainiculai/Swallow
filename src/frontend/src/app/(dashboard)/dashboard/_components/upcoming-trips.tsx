"use client"

import React from 'react';

import { Button, Card, Image, Link } from '@nextui-org/react';

import useSWR from 'swr';
import { fetcher } from '@/components/utilities/fetcher';

import { FaArrowAltCircleRight } from "react-icons/fa";

interface UpcomingTripsProps {
    tripId: string;
    destination: string;
    pictureUrl?: string;
    startDate: string;
    endDate: string;
}

const formatDate = (dateString: string): string => {
    const date = new Date(dateString);
    const day = date.getDate();
    const month = date.toLocaleString('default', { month: 'short' });
    const year = date.getFullYear();
    return `${day} ${month}${day === 1 ? '' : 'e'}`;
};

const formatDateRange = (startDate: string, endDate: string): string => {
    const start = formatDate(startDate);
    const end = formatDate(endDate);
    const endYear = new Date(endDate).getFullYear();
    return `${start} - ${end} ${endYear}`;
};

export const UpcomingTrips = () => {
    const { data, error } = useSWR<UpcomingTripsProps[]>('trips/upcoming', fetcher);

    return (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {data && data.map((trip) => (
                <Card key={trip.tripId}>
                    <div className="relative h-[10rem]">
                        <Image
                            removeWrapper
                            alt={trip.destination}
                            className="z-0 w-full h-full object-cover"
                            src={trip.pictureUrl}
                        />
                    </div>
                    <div className="p-4 flex flex-row justify-between items-center">
                        <div>
                        <h5 className="text-lg font-semibold">{trip.destination}</h5>
                        <p className="text-default-500">
                            {formatDateRange(trip.startDate, trip.endDate)}
                        </p>
                        </div>
                        <Button isIconOnly variant='flat' color='primary' as={Link} href={`/itinerary/${trip.tripId}`}>
                            <FaArrowAltCircleRight />
                        </Button>
                    </div>
                </Card>
            ))}
        </div>
    )
}
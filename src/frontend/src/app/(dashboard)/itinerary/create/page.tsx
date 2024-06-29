"use client";

import React, { useEffect, useState } from "react";

import { redirect } from 'next/navigation'
import { useSearchParams } from 'next/navigation'

import { useAuthContext, AuthContextType } from "@/contexts/auth-context";

import {
    Button,
    Card,
    CardFooter,
    Chip,
    Image,
    Spacer
} from "@nextui-org/react";

import useSWRImmutable from 'swr/immutable';
import { fetcher } from "@/components/utilities/fetcher";

import { RiSparklingLine } from "react-icons/ri";

import { parseDate } from '@internationalized/date';
import { getLocalTimeZone } from '@internationalized/date';
import Link from "next/link";
import { AutoItineraryCard } from "./_components/itinerary-card";


export default function ItineraryCreate() {
    const { user } = useAuthContext() as AuthContextType;

    const searchParams = useSearchParams()

    const [cityId, setCityId] = useState<number | null>(null)
    const [startDate, setStartDate] = useState<Date | null>(null)
    const [endDate, setEndDate] = useState<Date | null>(null)
    const [itineraryType, setItineraryType] = useState<number>(0)

    useEffect(() => {
        try {
            const sDate = parseDate(searchParams.get('startDate') || '');
            const eDate = parseDate(searchParams.get('endDate') || '');

            setCityId(parseInt(searchParams.get('cityId') || ''));
            setStartDate(sDate.toDate(getLocalTimeZone()));
            setEndDate(eDate.toDate(getLocalTimeZone()));
        } catch (error) {
            console.error('Invalid search parameters');
            redirect('/dashboard');
        }
    }, [searchParams]);

    const { data: destination } = useSWRImmutable(`destinations/${cityId}`, fetcher)

    if (!destination) {
        return null;
    }

    return (
        <div className="flex flex-col p-6 max-w-7xl mx-auto gap-6">
            <Card className="w-full relative h-[10rem]">
                <Image
                    removeWrapper
                    alt="City Picture"
                    className="z-0 w-full h-full object-cover"
                    src={destination?.pictureUrl}
                />
                <CardFooter className="absolute z-10 bottom-0 bg-black/20 p-4">
                    <div>
                        <p className="text-tiny text-white/60 uppercase font-bold">{destination.countryName}</p>
                        <h4 className="text-white font-medium text-large">{destination.name}</h4>
                        <Spacer y={2} />
                        <p className="text-white/60">Period: {startDate?.toString().split(' ').slice(0, 4).join(' ')} - {endDate?.toString().split(' ').slice(0, 4).join(' ')}</p>
                    </div>
                </CardFooter>
            </Card>
            {itineraryType === 0 && (
                <>
                    <div className="flex flex-col text-center mt-8 mb-6">
                        <h2 className="font-medium leading-7 text-primary">
                            Create your itinerary
                        </h2>
                        <h1 className="text-4xl font-medium tracking-tight">
                            You are one step away
                        </h1>
                        <Spacer y={4} />
                        <h2 className="text-large text-default-500">
                            Choose how you want to create your itinerary
                        </h2>
                    </div>
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                        <Card className="w-full grid grid-cols-1 lg:grid-cols-3 gap-8 py-12 px-4 md:p-8">
                            <Chip
                                classNames={{
                                    base: "absolute top-4 right-4",
                                }}
                                color="primary"
                                variant="flat"
                            >
                                Recommended
                            </Chip>
                            <Image
                                src="/itinerary/create/auto.svg"
                                alt="Auto"
                                className="aspect-square h-[10rem]"
                            />
                            <div className="flex flex-col gap-2 h-full col-span-2">
                                <h3 className="text-default-700 text-medium">One-Click Itinerary</h3>
                                <p className="text-default-500">
                                    Our Auto Itinerary option is perfect for a hassle-free experience.
                                    This feature automatically generates an itinerary based on your preferences.
                                </p>
                                <Spacer y={2} />
                                <div>
                                    {user?.planId === 1 ? (
                                        <Button
                                            className="bg-gradient-to-tr from-[#F05121] to-[#FD8524] text-white"
                                            startContent={<RiSparklingLine size={18} />}
                                            as={Link}
                                            href="/settings/membership"
                                        >
                                            Go Premium
                                        </Button>
                                    ) : (
                                        <Button color="primary" onClick={() => setItineraryType(1)}>
                                            Create itinerary
                                        </Button>
                                    )}
                                </div>
                            </div>
                        </Card>
                        <Card className="w-full grid ggrid-cols-1 lg:grid-cols-3 gap-8 py-12 px-4 md:p-8">
                            <Image
                                src="/itinerary/create/manual.svg"
                                alt="Manual"
                                className="aspect-square h-[10rem]"
                            />
                            <div className="flex flex-col gap-2 h-full col-span-2">
                                <h3 className="text-default-700 text-medium">Manual Itinerary</h3>
                                <p className="text-default-500">
                                    Our Manual Itinerary option puts you in control.
                                    This feature allows you to meticulously craft your travel experience, step-by-step and place-by-place.
                                </p>
                                <Spacer y={2} />
                                <div>
                                    <Button color="primary" onClick={() => setItineraryType(2)}>
                                        Create itinerary
                                    </Button>
                                </div>
                            </div>
                        </Card>
                    </div>
                </>
            )}
            {itineraryType !== 0 && (
                <AutoItineraryCard
                    cityId={cityId}
                    startDate={startDate?.toISOString() || ''}
                    endDate={endDate?.toISOString() || ''}
                    itineraryType={itineraryType}    
                />
            )}
        </div>
    )
}
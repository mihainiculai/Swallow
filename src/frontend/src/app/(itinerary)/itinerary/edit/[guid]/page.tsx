"use client";

import * as React from 'react';
import {
    Button,
    Image,
    Link,
    Card,
    CardFooter,
    Spacer,
    Autocomplete,
    AutocompleteItem,
} from '@nextui-org/react';

import { useAuthContext, AuthContextType } from "@/contexts/auth-context";

import useSWR from 'swr';
import { fetcher } from "@/components/utilities/fetcher";

import Map, { Marker } from 'react-map-gl';
import 'mapbox-gl/dist/mapbox-gl.css';
import { Logo } from '@/components/logo';

import { FaStar, FaTripadvisor, FaPhoneAlt, FaLink } from "react-icons/fa";
import { MdOutlineFlightTakeoff, MdHotel, MdMoreHoriz } from "react-icons/md";
import { MdAdd } from "react-icons/md";
import { BiSolidMagicWand } from "react-icons/bi";
import { GrAttachment } from "react-icons/gr";

import { RiSparklingLine } from "react-icons/ri";
import { RiHotelBedFill } from "react-icons/ri";
import { IoMdPin } from "react-icons/io";
import { UUID } from 'crypto';

export interface ItineraryDto {
    tripId: string;
    startDate: Date;
    endDate: Date;
    city: ItineraryCityDto;
    tripToHotel?: ItineraryTripToHotelDto;
    transportModeId: number;
    itineraryDays: ItineraryDayDto[];
    expenses: ItineraryExpenseDto[];
}

export interface ItineraryCityDto {
    name: string;
    description?: string;
    latitude: number;
    longitude: number;
    pictureUrl?: string;
}

export interface ItineraryTripToHotelDto {
    place: ItineraryPlaceDto;
    expense?: ItineraryExpenseDto;
}

export interface ItineraryPlaceDto {
    name: string;
    description?: string;
    address?: string;
    phone?: string;
    website?: string;
    rating?: number;
    userRatingsTotal?: number;
    latitude?: number;
    longitude?: number;
    pictureUrl?: string;
    googlePlaceId?: string;
    googleMapsUrl?: string;
    tripAdvisorUrl?: string;
}

export interface ItineraryExpenseDto {
    expenseId: string;
    expenseCategoryId: number;
    name: string;
    description?: string;
    attachmentUrl?: string;
    price?: number;
    currencyId?: number;
}

export interface ItineraryDayDto {
    itineraryDayId: number;
    date?: Date;
    itineraryAttractions: ItineraryAttractionDto[];
}

export interface ItineraryAttractionDto {
    itineraryAttractionId: number;
    index: number;
    attraction: ItineraryPlaceDto;
    ticketsUrl?: string;
    expense?: ItineraryExpenseDto;
}

interface RecommendationDto {
    AttractionId: string;
    name: string;
    rating?: number;
    description?: string;
    pictureUrl?: string;
}

const dayColors = [
    "#FD8524", // Orange
    "#039be5", // Blue
    "#8e24aa", // Purple
    "#e53935", // Red
    "#43a047", // Green
];

export default function ItineraryEdit({ params }: { params: { guid: UUID } }) {
    const { user } = useAuthContext() as AuthContextType

    const { data: itinerary, error } = useSWR<ItineraryDto>(`/itineraries/${params.guid}`, fetcher)
    const { data: recommendations, error: recommendationsError } = useSWR<RecommendationDto>(`/itineraries/recommend-attractions/${params.guid}`, fetcher)

    return (
        <div className="w-screen h-screen grid grid-cols-5 gap-6 p-6 overflow-hidden">
            <div className="col-span-3 flex flex-col gap-6 h-full overflow-x-hidden overflow-y-hidden relative">
                <div className='flex flex-row gap-6 w-full justify-between'>
                    <div className='flex items-center gap-4'>
                        <Link href="/dashboard">
                            <Logo width={32} height={32} className="mr-2" />
                        </Link>
                        <p className="font-bold text-inherit">Swallow Itinerary</p>
                    </div>

                    {user?.planId === 1 && (
                        <Button
                            className="bg-gradient-to-tr from-[#F05121] to-[#FD8524] text-white"
                            startContent={<RiSparklingLine size={18} />}
                            as={Link}
                            href="/settings/membership"
                        >
                            Go Premium
                        </Button>
                    )}
                </div>

                <div className="content-wrapper overflow-x-hidden hide-scrollbar flex flex-col gap-8">
                    <Card className="w-full relative min-h-[14rem] h-[14rem]">
                        <Image
                            removeWrapper
                            alt="City Picture"
                            className="z-0 w-full h-full object-cover"
                            src={itinerary?.city?.pictureUrl}
                        />
                        <CardFooter className="absolute z-10 bottom-0 bg-black/20 p-4">
                            <div>
                                <h4 className="text-white font-medium text-large">Trip to {itinerary?.city?.name}</h4>
                                <Spacer y={2} />
                                <p className="text-white/60">{itinerary?.startDate?.toString().split(' ').slice(0, 4).join(' ')} - {itinerary?.endDate?.toString().split(' ').slice(0, 4).join(' ')}</p>
                            </div>
                        </CardFooter>
                    </Card>

                    <Card className="w-full grid grid-cols-5 relative min-h-[8rem] h-[8rem] items-center p-4 gap-4">
                        <div className="col-span-2 flex flex-col gap-2">
                            <h3 className="font-bold text-large">Budget</h3>
                            <p className="text-gray-500 text-2xl">0.00 USD</p>
                            <Link size='sm' href="#">View details</Link>
                        </div>
                        <div className="col-span-3 flex flex-row gap-4 justify-end">
                            <Button isIconOnly size='lg' variant='light' className='w-20 h-20'>
                                <div className="flex flex-col items-center gap-2">
                                    <MdOutlineFlightTakeoff fontSize={24} />
                                    <p className="text-[0.65rem]">Flights</p>
                                </div>
                            </Button>
                            <Button isIconOnly size='lg' variant='light' className='w-20 h-20'>
                                <div className="flex flex-col items-center gap-2">
                                    <MdHotel fontSize={24} />
                                    <p className="text-[0.65rem]">Lodging</p>
                                </div>
                            </Button>
                            <Button isIconOnly size='lg' variant='light' className='w-20 h-20'>
                                <div className="flex flex-col items-center gap-2">
                                    <GrAttachment fontSize={24} />
                                    <p className="text-[0.65rem]">Attachments</p>
                                </div>
                            </Button>
                            <Button isIconOnly size='lg' variant='light' className='w-20 h-20'>
                                <div className="flex flex-col items-center gap-2">
                                    <MdMoreHoriz fontSize={24} />
                                    <p className="text-[0.65rem]">More</p>
                                </div>
                            </Button>
                        </div>
                    </Card>

                    <div className="flex flex-col gap-8">
                        {itinerary?.itineraryDays.map((day, dayIndex) => (
                            <div key={day.itineraryDayId} className="flex flex-col gap-4 ml-4">
                                <div className="flex flex-row gap-4 items-center">
                                    <div className="w-4 h-4 rounded-full" style={{ backgroundColor: dayColors[dayIndex % dayColors.length] }} />
                                    <h4 className="font-bold text-large">{dayIndex > 0 ? `Day ${dayIndex}` : 'Places to add'}</h4>
                                </div>

                                {dayIndex === 0 && (
                                    <div className="flex flex-col gap-4">
                                        <div className="flex flex-row gap-4 items-center">
                                            <Autocomplete placeholder="Search for a place" size='lg'> 
                                                <AutocompleteItem key="1" value="1">Item 1</AutocompleteItem>
                                                <AutocompleteItem key="2" value="2">Item 2</AutocompleteItem>
                                                <AutocompleteItem key="3" value="3">Item 3</AutocompleteItem>
                                            </Autocomplete>
                                            <Button
                                                color='primary'
                                                className='px-8'
                                            >
                                                Add to Itinerary
                                            </Button>
                                        </div>

                                        <div className="flex flex-row gap-4 items-center">
                                            {recommendations && (
                                                <Card className="flex flex-col gap-4 p-4">
                                                    <div className="flex flex-row gap-4 items-center">
                                                        <BiSolidMagicWand className="text-primary" size={24} />
                                                        <h4 className="font-bold text-large">Recommendations</h4>
                                                    </div>
                                                    <div className="flex flex-col gap-4">
                                                        {recommendations.map(recommendation => (
                                                            <Card key={recommendation.AttractionId} className="flex flex-row gap-4 p-4">
                                                                <Image
                                                                    removeWrapper
                                                                    alt="Attraction Picture"
                                                                    className="w-24 h-24 object-cover"
                                                                    src={recommendation.pictureUrl}
                                                                />
                                                                <div className="flex flex-col gap-2 w-full">
                                                                    <div className="flex flex-row gap-4 items-center justify-between w-full">
                                                                        <h4 className="font-bold text-large grow">{recommendation.name}</h4>
                                                                        <div className="flex gap-2 items-center min-w-[8rem] justify-end">
                                                                            <p className="text-sm text-gray-500 inline">{recommendation.rating}</p>
                                                                            <FaStar className="text-primary" />
                                                                        </div>
                                                                    </div>
                                                                    <p className="text-gray-500 text-sm">{recommendation.description}</p>
                                                                </div>
                                                                <Button
                                                                    isIconOnly
                                                                    variant='light'
                                                                    className='m-auto'
                                                                >
                                                                    <MdAdd className='text-primary' size={24} />
                                                                </Button>
                                                            </Card>
                                                        ))}
                                                    </div>
                                                </Card>
                                            )}
                                        </div>

                                        <pre>{JSON.stringify(recommendations, null, 2)}</pre>
                                    </div>
                                )}

                                {day.itineraryAttractions.map(attraction => (
                                    <Card key={attraction.itineraryAttractionId} className="flex flex-row gap-4 p-4">
                                        <Image
                                            removeWrapper
                                            alt="Attraction Picture"
                                            className="w-24 h-24 object-cover"
                                            src={attraction.attraction.pictureUrl}
                                        />
                                        <div className="flex flex-col gap-2 w-full">
                                            <div className="flex flex-row gap-4 items-center justify-between w-full">
                                                <h4 className="font-bold text-large grow">{attraction.attraction.name}</h4>
                                                <div className="flex gap-2 items-center min-w-[8rem] justify-end">
                                                    <p className="text-sm text-gray-500 inline">{attraction.attraction.rating} ({attraction.attraction.userRatingsTotal})</p>
                                                    <FaStar className="text-primary" />
                                                </div>
                                            </div>
                                            <p className="text-gray-500 text-sm">{attraction.attraction.description}</p>
                                        </div>
                                    </Card>
                                ))}
                            </div>
                        ))}
                    </div>
                </div>
            </div>
            <div className="col-span-2 flex flex-col h-full">
                {itinerary?.city && (
                    <Map
                        mapboxAccessToken={process.env.NEXT_PUBLIC_MAPBOX_ACCESS_TOKEN}
                        initialViewState={{
                            longitude: itinerary?.city?.longitude,
                            latitude: itinerary?.city?.latitude,
                            zoom: 11
                        }}
                        style={{ borderRadius: '1rem' }}
                        mapStyle="mapbox://styles/mapbox/streets-v9"
                    >
                        {itinerary?.tripToHotel && (
                            <Marker longitude={itinerary?.tripToHotel.place.longitude} latitude={itinerary?.tripToHotel.place.latitude} anchor="bottom">
                                <RiHotelBedFill size={32} color="#FD8524" />
                            </Marker>
                        )}
                        {itinerary?.itineraryDays.map((day, dayIndex) => (
                            day.itineraryAttractions.map(attraction => (
                                <Marker key={attraction.itineraryAttractionId} longitude={attraction.attraction.longitude} latitude={attraction.attraction.latitude} anchor="bottom">
                                    <IoMdPin size={32} color={dayColors[dayIndex % dayColors.length]} />
                                </Marker>
                            ))
                        ))}
                    </Map>
                )}
            </div>
        </div>
    )
}
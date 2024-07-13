"use client";

import { Card, Image, Button } from '@nextui-org/react';

import { FaStar, FaLink, FaPhoneAlt, FaTripadvisor } from 'react-icons/fa';
import { AiFillLike, AiFillDislike, AiOutlineLike, AiOutlineDislike, AiOutlineDelete } from "react-icons/ai";
import { SiGooglemaps } from 'react-icons/si';

import useSWR, { mutate } from 'swr';
import { fetcher } from '@/components/utilities/fetcher';
import { axiosInstance } from '@/components/utilities/axiosInstance';

export const AttractionnCard = ({ attraction, tripId, day, index }: { attraction: any, tripId: string, day: number, index: number }) => {
    const { data: preference } = useSWR(`itineraries/preferences?attractionId=${attraction.attractionId}`, fetcher);

    const like = async () => {
        await axiosInstance.post(`itineraries/preferences?attractionId=${attraction.attractionId}&preference=1`);
        mutate(`itineraries/preferences?attractionId=${attraction.attractionId}`);
        mutate(`itineraries/recommend-attractions/${tripId}`);

    }

    const dislike = async () => {
        await axiosInstance.post(`itineraries/preferences?attractionId=${attraction.attractionId}&preference=4`);
        mutate(`itineraries/preferences?attractionId=${attraction.attractionId}`);
        mutate(`itineraries/recommend-attractions/${tripId}`);
    }

    const remove = async () => {
        await axiosInstance
            .delete(`itineraries/${tripId}/attractions`, {
                data: {
                    day: day,
                    index: index
                }
            }).finally(() => {
                mutate(`/itineraries/${tripId}`);
                mutate(`/itineraries/recommend-attractions/${tripId}`);
            });
    }

    return (
        <Card key={attraction.itineraryAttractionId} className="flex flex-row gap-4 p-4">
            <Image
                removeWrapper
                alt="Attraction Picture"
                className="w-24 h-24 object-cover"
                src={attraction.attraction.pictureUrl || '/placeholder.png'}
            />
            <div className="flex flex-col gap-2 w-full">
                <div className="flex flex-row gap-4 items-start justify-between w-full">
                    <h4 className="font-bold text-large grow">{attraction.attraction.name}</h4>
                    <div className="flex gap-2 items-center min-w-[8rem] justify-end">
                        <p className="text-sm text-default-400 inline">{attraction.attraction.rating} ({attraction.attraction.userRatingsTotal})</p>
                        <FaStar className="text-primary" />
                    </div>
                </div>
                <p className="text-default-500 text-sm">{attraction.attraction.description}</p>
                <div className="flex flex-row gap-2 items-center justify-between">
                    <div className="flex flex-row gap-1 items-center">
                        {attraction.attraction.website && (
                            <Button
                                variant='light'
                                size='sm'
                                isIconOnly
                                onClick={() => window.open(attraction.attraction.website, '_blank')}
                            >
                                <FaLink className="text-primary" />
                            </Button>
                        )}
                        {attraction.attraction.phone && (
                            <Button
                                variant='light'
                                size='sm'
                                isIconOnly
                                onClick={() => window.open(`tel:${attraction.attraction.phone}`)}
                            >
                                <FaPhoneAlt className="text-primary" />
                            </Button>
                        )}
                        {attraction.attraction.tripAdvisorUrl && (
                            <Button
                                variant='light'
                                size='sm'
                                isIconOnly
                                onClick={() => window.open(attraction.attraction.tripAdvisorUrl, '_blank')}
                            >
                                <FaTripadvisor className="text-primary" />
                            </Button>
                        )}
                        {attraction.attraction.googleMapsUrl && (
                            <Button
                                variant='light'
                                size='sm'
                                isIconOnly
                                onClick={() => window.open(attraction.attraction.googleMapsUrl, '_blank')}
                            >
                                <SiGooglemaps className="text-primary" />
                            </Button>
                        )}
                    </div>
                    <div className="flex flex-row gap-1 items-center">
                        <Button
                            variant='light'
                            size='sm'
                            isIconOnly
                            onClick={like}
                        >
                            {preference == 1 ? (
                                <AiFillLike className="text-primary" />
                            ) : (
                                <AiOutlineLike className="text-primary" />
                            )}
                        </Button>
                        <Button
                            variant='light'
                            size='sm'
                            isIconOnly
                            onClick={dislike}
                        >
                            {preference == 4 ? (
                                <AiFillDislike className="text-primary" />
                            ) : (
                                <AiOutlineDislike className="text-primary" />
                            )}
                        </Button>
                        <Button
                            variant='light'
                            size='sm'
                            isIconOnly
                            onClick={remove}
                        >
                            <AiOutlineDelete className="text-primary" />
                        </Button>
                    </div>
                </div>

            </div>
        </Card>
    )
}
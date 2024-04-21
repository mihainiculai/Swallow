"use client";

import {
    Avatar,
    Card,
    CardHeader,
    CardFooter,
    Image,
    Spacer,
    Button
} from "@nextui-org/react";
import { DateRangePicker } from "@nextui-org/date-picker";

import useSWRImmutable from 'swr/immutable'
import { fetcher } from "@/components/utilities/fetcher";
import { CountryFlagUrl } from "@/components/country-flag";
import PlaceListItem from "../../administration/cities/_components/place-list-item";

interface Destination {
    cityId: number;
    name: string;
    description?: string;
    pictureUrl?: string;
    countryName: string;
    countryCode: string;
}

interface Attraction {
    name: string;
    description?: string;
    rating?: number;
    categories: string[];
    visitDuration?: string;
    tripAdvisorUrl?: string;
    phone?: string;
    website?: string;
    googleMapsUrl?: string;
    pictureUrl?: string;
}

export default function Destionation({ params }: { params: { id: number } }) {
    const { data: destination } = useSWRImmutable<Destination>(`destinations/${params.id}`, fetcher)
    const { data: attractions, isLoading: isLoadingAttractions } = useSWRImmutable<Attraction[]>(`destinations/${params.id}/top-attractions`, fetcher)

    if (!destination) {
        return null;
    }

    return (
        <div className="flex flex-col p-6 max-w-7xl mx-auto gap-4">
            <Card className="w-full relative h-[40rem] md:h-[30rem] lg:h-[25rem] xl:h-[20rem]">
                <CardHeader className="absolute z-10 top-1 flex-col items-end">
                    <Avatar
                        radius="full"
                        size="sm"
                        src={CountryFlagUrl(destination.countryCode)}
                    />
                </CardHeader>
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
                        <Spacer y={4} />
                        <p className="text-white/60">{destination?.description}</p>
                    </div>
                </CardFooter>
            </Card>
            <Card className="p-6 w-full flex-col md:flex-row gap-4 items-center align-middle gap-4">
                <div className="w-full">
                    <DateRangePicker
                        label="Select dates"
                    />
                </div>
                <Button
                    color="primary"
                    size="lg"
                    className="w-full md:w-auto"
                >
                    Start planning
                </Button>
            </Card>
            <div className="mt-6 mb-3">
                <h2 className="font-medium leading-7 text-primary">TOP 10</h2>
                <h2 className="text-2xl font-bold">Popular places</h2>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {isLoadingAttractions && (
                    <>
                        <PlaceListItem isLoading />
                        <PlaceListItem isLoading />
                        <PlaceListItem isLoading />
                    </>
                )}
                {attractions?.map((attraction, index) => (
                    <PlaceListItem
                        key={index}
                        priority={index + 1}
                        name={attraction.name}
                        description={attraction.description}
                        rating={attraction.rating}
                        categories={attraction.categories}
                        pictureUrl={attraction.pictureUrl}
                        phone={attraction.phone}
                        website={attraction.website}
                        tripAdvisorUrl={attraction.tripAdvisorUrl}
                        googleMapsUrl={attraction.googleMapsUrl}
                        descriptionLength={300}
                    />
                ))}
            </div>
        </div>
    )
}
"use client"

import { useRouter } from 'next/navigation'

import useSWR from "swr"
import { fetcher } from "@/components/utilities/fetcher"
import { Button, Card, CardFooter, Image } from '@nextui-org/react'
import { FaCircleArrowRight } from "react-icons/fa6";

interface City {
    cityId: number
    name: string
    pictureUrl: string
}

export default function DiscoverPage() {
    const router = useRouter()
    const { data } = useSWR<City[]>("discover/top-cities", fetcher)

    return (
        <div className="max-w-7xl mx-auto grid grid-cols-1 md:grid-cols-3 gap-4 py-6">
            {data && data.map(city => (
                <>
                    <Card className="col-span-12 sm:col-span-4 h-[300px]" key={city.cityId}>
                        <Image
                            removeWrapper
                            alt="Card background"
                            className="z-0 w-full h-full object-cover"
                            src={city.pictureUrl}
                        />
                        <CardFooter className="absolute z-10 bottom-1 flex justify-between w-full">
                            <h4 className="text-white font-bold text-xl">{city.name}</h4>
                            <Button
                                color="primary"
                                onClick={() => router.push(`/destination/${city.cityId}`)}
                                endContent={<FaCircleArrowRight />}
                            >
                                Discover
                            </Button>
                        </CardFooter>
                    </Card>
                </>
            ))}
        </div>
    )
}
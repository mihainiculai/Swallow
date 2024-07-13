"use client";

import { Card, Image, CardFooter, Spacer } from '@nextui-org/react';

export const HeadCard = ({ itinerary }: { itinerary: any }) => {
    return (
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
    )
}
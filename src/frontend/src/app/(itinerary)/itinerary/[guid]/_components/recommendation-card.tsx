"use cliennt"

import { Card, Image, Button } from '@nextui-org/react';
import { UUID } from 'crypto';
import { MdAdd } from 'react-icons/md';
import { mutate } from 'swr';

interface RecommendationCardProps {
    tripId: UUID;
    recommendation: any;
    addAttraction: (attractionId: number) => Promise<void>;
}

export const RecommendationCard = ({ tripId, recommendation, addAttraction }: RecommendationCardProps) => {
    const handleAddAttraction = async () => {
        await addAttraction(recommendation.attractionId);
    }

    return (
        <Card key={recommendation.AttractionId} className="flex-shrink-0 p-2 w-72 h-16">
            <div className='flex flex-row gap-2'>
            <Image
                removeWrapper
                alt="Attraction Picture"
                className="w-12 h-12 object-cover"
                src={recommendation.pictureUrl}
            />
            <h4 className="font-bold grow">{recommendation.name.substring(0, 41)}</h4>
            <Button
                isIconOnly
                variant='light'
                className='m-auto'
            >
                <MdAdd className='text-primary' size={24} onClick={handleAddAttraction} />
            </Button>
            </div>
        </Card>
    )
}
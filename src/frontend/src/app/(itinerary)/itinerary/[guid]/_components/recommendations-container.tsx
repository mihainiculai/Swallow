import React, { useRef, useState, useEffect } from 'react';

import { Button } from '@nextui-org/react';
import { MdChevronLeft, MdChevronRight } from 'react-icons/md';

import useSWR from 'swr';
import { fetcher } from "@/components/utilities/fetcher";

import { RecommendationDto } from './types';

import { RecommendationCard } from './recommendation-card';

import { UUID } from 'crypto';

interface RecommendationsContainerProps {
    tripId: UUID;
    addAttraction: (attractionId: number) => Promise<void>;
}

export const RecommendationsContainer: React.FC<RecommendationsContainerProps> = ({ tripId, addAttraction }) => {
    const { data: recommendations } = useSWR<RecommendationDto[]>(`/itineraries/recommend-attractions/${tripId}`, fetcher)

    const scrollContainerRef = useRef<HTMLDivElement>(null);
    const [canScrollLeft, setCanScrollLeft] = useState<boolean>(false);
    const [canScrollRight, setCanScrollRight] = useState<boolean>(false);

    const cardWidth = 288;

    const checkScroll = () => {
        if (scrollContainerRef.current) {
            const { scrollLeft, scrollWidth, clientWidth } = scrollContainerRef.current;
            setCanScrollLeft(scrollLeft > 10);
            setCanScrollRight(scrollLeft < scrollWidth - clientWidth);
        }
    };

    useEffect(() => {
        const container = scrollContainerRef.current;
        if (container) {
            container.addEventListener('scroll', checkScroll);
            window.addEventListener('resize', checkScroll);
            checkScroll();
        }

        return () => {
            if (container) {
                container.removeEventListener('scroll', checkScroll);
                window.removeEventListener('resize', checkScroll);
            }
        };
    }, []);

    const scroll = (direction: 'left' | 'right') => {
        const container = scrollContainerRef.current;
        if (container) {
            const currentScroll = container.scrollLeft;
            const maxScroll = container.scrollWidth - container.clientWidth;
            const targetScroll = direction === 'left'
                ? Math.max(0, currentScroll - cardWidth)
                : Math.min(maxScroll, currentScroll + cardWidth);

            container.scrollTo({
                left: targetScroll,
                behavior: 'smooth'
            });
        }
    };

    return (
        <div className="relative p-8">
            <Button
                isDisabled={!canScrollLeft}
                isIconOnly
                size="sm"
                variant='flat'
                color='primary'
                className="absolute left-[-10px] top-1/2 transform -translate-y-1/2 z-20 shadow-md rounded-full"
                onClick={() => scroll('left')}
            >
                <MdChevronLeft size={20} />
            </Button>
            <div
                ref={scrollContainerRef}
                className="flex flex-row gap-4 overflow-x-auto hide-scrollbar px-2 snap-x snap-mandatory"
            >
                {recommendations?.map(recommendation => (
                    <div key={recommendation.AttractionId} className="snap-start">
                        <RecommendationCard recommendation={recommendation} addAttraction={addAttraction} tripId={tripId} />
                    </div>
                ))}
            </div>
            <Button
                isDisabled={!canScrollRight}
                isIconOnly
                variant='flat'
                color='primary'
                size="sm"
                className="absolute right-[-10px] top-1/2 transform -translate-y-1/2 z-20 shadow-md rounded-full"
                onClick={() => scroll('right')}
            >
                <MdChevronRight size={20} />
            </Button>
        </div>
    );
};

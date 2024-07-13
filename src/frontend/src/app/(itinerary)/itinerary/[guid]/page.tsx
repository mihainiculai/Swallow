"use client";

import * as React from 'react';
import { useRouter } from 'next/navigation'

import { DragDropContext, Droppable, Draggable } from 'react-beautiful-dnd';
import { UUID } from 'crypto';

import useSWR, { mutate } from 'swr';
import { fetcher } from "@/components/utilities/fetcher";
import { axiosInstance } from '@/components/utilities/axiosInstance';

import { ItineraryDto, RecommendationDto } from './_components/types';
import { Navbar } from './_components/navbar';
import { HeadCard } from './_components/head-card';
import { SearchBar } from './_components/search-bar';
import { ManageCard } from './_components/manage-card';
import { AttractionnCard } from './_components/attraction-card';
import { MapCard } from './_components/map-card';
import { DayColors } from './_components/colors';
import { RecommendationsContainer } from './_components/recommendations-container';
import { ExpensesCard } from './_components/expenses-card';

export default function ItineraryEdit({ params }: { params: { guid: UUID } }) {
    const router = useRouter()
    const expensesRef = React.useRef<HTMLDivElement>(null);

    const scrollToExpenses = () => {
        expensesRef.current?.scrollIntoView({ behavior: 'smooth' });
    };

    const { data: itinerary, error } = useSWR<ItineraryDto>(`/itineraries/${params.guid}`, fetcher)
    const { data: recommendations } = useSWR<RecommendationDto[]>(`/itineraries/recommend-attractions/${params.guid}`, fetcher)

    const [itineraryData, setItineraryData] = React.useState<ItineraryDto | null>(null);
    const [totalCost, setTotalCost] = React.useState<number>(0);

    React.useEffect(() => {
        if (itinerary) {
            setItineraryData(itinerary);
        }
    }, [itinerary]);

    const onDragEnd = async (result: any) => {
        if (!result.destination || !itineraryData) {
            return;
        }

        const sourceDay = parseInt(result.source.droppableId);
        const destinationDay = parseInt(result.destination.droppableId);

        const newItineraryData = { ...itineraryData };
        const [reorderedItem] = newItineraryData.itineraryDays[sourceDay].itineraryAttractions.splice(result.source.index, 1);
        newItineraryData.itineraryDays[destinationDay].itineraryAttractions.splice(result.destination.index, 0, reorderedItem);

        setItineraryData(newItineraryData);

        await axiosInstance.post(`itineraries/reorder-attraction/${itineraryData.tripId}`, {
            sourceDay: sourceDay,
            destinationDay: destinationDay,
            sourceIndex: result.source.index,
            destinationIndex: result.destination.index,
        });
        mutate(`/itineraries/${params.guid}`);
    };

    React.useEffect(() => {
        if (error) {
            router.push('/dashboard')
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [error])

    const handleAddAttraction = async (attractionId: number) => {
        await axiosInstance.post(`itineraries/add-attraction/${itineraryData?.tripId}/${attractionId}`);
        mutate(`/itineraries/${params.guid}`);
        mutate(`/itineraries/recommend-attractions/${params.guid}`);
    }

    return (
        <div className="w-screen h-screen grid grid-cols-5 gap-6 p-6">
            <div className="col-span-3 flex flex-col gap-6 overflow-x-hidden overflow-y-hidden">
                <Navbar />
                <div className="overflow-x-hidden hide-scrollbar flex flex-col gap-8">
                    <HeadCard itinerary={itineraryData} />
                    <ManageCard tripId={itineraryData?.tripId ?? ''} totalCost={totalCost} onViewMoreClick={scrollToExpenses} cityId={itineraryData?.cityId ?? 0} lodging={itineraryData?.tripToHotel?.place} />
                    <DragDropContext onDragEnd={onDragEnd}>
                        <div className="flex flex-col gap-8">
                            {itineraryData?.itineraryDays.map((day, dayIndex) => (
                                <Droppable key={day.itineraryDayId} droppableId={dayIndex.toString()}>
                                    {(provided) => (
                                        <div {...provided.droppableProps} ref={provided.innerRef} className={`flex flex-col gap-4 p-4`} >
                                            <div className="flex flex-row gap-4 items-center">
                                                <div className="w-4 h-4 rounded-full" style={{ backgroundColor: DayColors[dayIndex % DayColors.length] }} />
                                                <h4 className="font-bold text-large">{dayIndex > 0 ? `Day ${dayIndex}` : 'Places to add'}</h4>
                                            </div>

                                            {dayIndex === 0 && <SearchBar tripId={itineraryData?.tripId} addAttraction={handleAddAttraction} />}
                                            {dayIndex === 0 && recommendations && <RecommendationsContainer addAttraction={handleAddAttraction} tripId={itineraryData?.tripId} />}

                                            {day.itineraryAttractions.map((attraction, index) => (
                                                <Draggable key={attraction.itineraryAttractionId} draggableId={attraction.itineraryAttractionId.toString()} index={index}>
                                                    {(provided) => (
                                                        <div ref={provided.innerRef} {...provided.draggableProps} {...provided.dragHandleProps}>
                                                            <AttractionnCard attraction={attraction} tripId={params.guid} day={dayIndex} index={index} />
                                                        </div>
                                                    )}
                                                </Draggable>
                                            ))}
                                            {provided.placeholder}
                                        </div>
                                    )}
                                </Droppable>
                            ))}
                        </div>
                    </DragDropContext>
                    <ExpensesCard
                        ref={expensesRef}
                        expenses={itineraryData?.expenses}
                        itineraryId={params.guid}
                        totalCost={totalCost}
                        setTotalCost={setTotalCost}
                    />
                </div>
            </div>
            <div className="col-span-2 flex flex-col h-full">
                {itinerary?.city && <MapCard itinerary={itinerary} />}
            </div>
        </div>
    );
}
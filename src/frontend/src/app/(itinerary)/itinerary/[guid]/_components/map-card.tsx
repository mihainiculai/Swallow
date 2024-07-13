"use client";

import { Map, Marker } from 'react-map-gl';
import 'mapbox-gl/dist/mapbox-gl.css';
import { RiHotelBedFill } from "react-icons/ri";
import { IoMdPin } from "react-icons/io";

import { ItineraryDto } from './types';

const dayColors = ["#FD8524", "#039be5", "#8e24aa", "#e53935", "#43a047"];

export const MapCard = ({ itinerary }: { itinerary: ItineraryDto }) => {
    return (
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
    )
}
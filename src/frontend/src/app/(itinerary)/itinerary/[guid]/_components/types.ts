export interface ItineraryDto {
    tripId: string;
    startDate: Date;
    endDate: Date;
    cityId: number;
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
    price: number;
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

export interface RecommendationDto {
    AttractionId: string;
    name: string;
    rating?: number;
    description?: string;
    pictureUrl?: string;
}
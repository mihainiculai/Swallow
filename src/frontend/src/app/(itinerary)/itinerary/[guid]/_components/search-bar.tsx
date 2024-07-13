
import React, { useState } from "react";
import { Autocomplete, AutocompleteItem, Image } from "@nextui-org/react";
import { Key } from '@react-types/shared';

import { axiosInstance } from "@/components/utilities/axiosInstance";

import { FaSearch } from "react-icons/fa";

interface Prediction {
    attractionId: number;
    name: string;
    pictureUrl: string;
}

interface SearchPlaceAutoCompleteProps {
    tripId: string;
    addAttraction: (attractionId: number) => Promise<void>;
}

export const SearchBar: React.FC<SearchPlaceAutoCompleteProps> = ({ tripId, addAttraction }) => {
    const [searchQuery, setSearchQuery] = useState<string>("");
    const [predictions, setPredictions] = useState<Prediction[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(false);

    const handleSearch = async (searchQuery: string) => {
        if (searchQuery.length < 2) {
            setPredictions([]);
            setIsLoading(false);
            return;
        }

        const result = await axiosInstance.get<Prediction[]>("itineraries/search", {
            params: {
                tripId: tripId,
                query: searchQuery
            },
        });

        const data = result.data;
        data ? setPredictions(data) : setPredictions([]);
        setIsLoading(false);
    }

    const onInputChange = (searchQuery: string) => {
        setSearchQuery(searchQuery);
        setIsLoading(true);
        handleSearch(searchQuery);
    }

    const onSelectionChange = (key: Key) => {
        addAttraction(parseInt(key as string));
    }

    return (
        <Autocomplete
            inputValue={searchQuery}
            isLoading={isLoading}
            items={predictions}
            onInputChange={onInputChange}
            placeholder="Type to search..."
            size='lg'
            fullWidth
            startContent={<FaSearch className='text-default-400' />}
            selectorIcon={null}
            onSelectionChange={onSelectionChange}
            aria-label="Search Place"
        >
            {(item) => (
                <AutocompleteItem key={item.attractionId} value={item.attractionId.toString()}>
                    <div className="flex flex-row gap-2 items-center">
                        <div>
                            <Image width={48} height={48} src={item.pictureUrl || "/placeholder.png"} alt="Attraction Picture" />
                        </div>
                        <div>
                            <p>{item.name}</p>
                        </div>
                    </div>
                </AutocompleteItem>
            )}
        </Autocomplete>
    );
}
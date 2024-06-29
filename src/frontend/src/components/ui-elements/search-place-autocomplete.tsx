
import React, { useState } from "react";
import { Autocomplete, AutocompleteItem } from "@nextui-org/react";
import { Key } from '@react-types/shared';

import { axiosInstance } from "../utilities/axiosInstance";

import { FaSearch } from "react-icons/fa";
import { FaLocationDot } from "react-icons/fa6";

interface Prediction {
    structured_formatting: {
        main_text: string;
        secondary_text: string;
    };
    place_id: string;
}

interface PredictionResult {
    predictions: Prediction[];
    sessionToken: string;
}

interface SearchPlaceAutoCompleteProps {
    cityId: number | null;
    value: Key;
    setValue: (key: Key) => void;
    setSessionToken?: (token: string) => void;
    isInvalid?: boolean | undefined;
    errorMessage?: string | false | undefined;
}

export const SearchPlaceAutocomplete: React.FC<SearchPlaceAutoCompleteProps> = ({ cityId, value, setValue, setSessionToken, isInvalid = false, errorMessage = "" }) => {
    const [searchQuery, setSearchQuery] = useState<string>("");
    const [predictions, setPredictions] = useState<Prediction[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [token, setToken] = useState<string>("");

    const handleSearch = async (searchQuery: string) => {
        if (searchQuery.length < 2) {
            setPredictions([]);
            setIsLoading(false);
            return;
        }

        const result = await axiosInstance.get<PredictionResult>("search/places", {
            params: {
                cityId: cityId,
                query: searchQuery,
                ...(token ? { sessionToken: token } : {}),
            },
        });

        const data = result.data;

        if (data) {
            setPredictions(data.predictions);
            setToken(data.sessionToken);
            if (setSessionToken) {
                setSessionToken(data.sessionToken);
            }
        }
        else {
            setPredictions([]);
        }

        setIsLoading(false);
    }

    const onInputChange = (searchQuery: string) => {
        setSearchQuery(searchQuery);
        setValue("");
        setIsLoading(true);
        handleSearch(searchQuery)
    }

    const onSelectionChange = (key: Key) => {
        const selectedItem = predictions.find((item) => item.place_id === key);
        setSearchQuery(selectedItem?.structured_formatting.main_text || "");
        setValue(key);
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
            selectedKey={value}
            onSelectionChange={onSelectionChange}
            aria-label="Search Place"
            isInvalid={isInvalid}
            errorMessage={errorMessage}
        >
            {(item) => (
                <AutocompleteItem key={item.place_id}>
                    <div className="flex flex-row gap-2 items-center">
                        <div>
                            <FaLocationDot />
                        </div>
                        <div>
                            <p>{item.structured_formatting.main_text}</p>
                            <p className="text-default-500">{item.structured_formatting.secondary_text}</p>
                        </div>
                    </div>
                </AutocompleteItem>
            )}
        </Autocomplete>
    );
}
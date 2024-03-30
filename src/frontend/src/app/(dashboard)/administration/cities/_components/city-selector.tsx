import {
    Avatar,
    Autocomplete,
    AutocompleteItem,
} from "@nextui-org/react";

import useSWR from "swr";
import { fetcher } from "@/components/utilities/fetcher";

import { CountryFlagUrl } from "@/components/country-flag";

export const CitySelector = ({ selectedCountry, onCountrySelectionChange, onCitySelectionChange }: { selectedCountry: any, onCountrySelectionChange: any, onCitySelectionChange: any }) => {
    const { data: countries } = useSWR('countries', fetcher);
    const { data: cities } = useSWR(selectedCountry?.id ? `countries/${selectedCountry?.id}/cities` : null, fetcher);
    
    const onCountrySelection = (id: any) => {
        const selectedCountry = countries.find((country: any) => country.countryId == id);

        if (!selectedCountry) {
            onCountrySelectionChange(null);
            return;
        }

        const country = {
            id: selectedCountry.countryId,
            name: selectedCountry.name,
            iso2: selectedCountry.iso2
        };

        onCountrySelectionChange(country);
    };

    const onCitySelection = (id: any) => {
        const selectedCity = cities.find((city: any) => city.cityId == id);

        if (!selectedCity) {
            onCitySelectionChange(null);
            return;
        }

        const city = {
            id: selectedCity.cityId,
            name: selectedCity.name
        };

        onCitySelectionChange(city);
    }

    return (
        <div className="flex flex-col w-full md:flex-nowrap gap-4 basis-1/4">
            <Autocomplete label="Country" onSelectionChange={onCountrySelection}>
                {countries && countries.map((country: any) => (
                    <AutocompleteItem
                        key={country.countryId}
                        value={country.countryId}
                        startContent={
                            <Avatar
                                alt={country.name}
                                className="w-6 h-6"
                                src={CountryFlagUrl(country.iso2.toLowerCase())}
                            />
                        }
                    >
                        {country.name}
                    </AutocompleteItem>
                ))}
            </Autocomplete>

            <Autocomplete label="City" onSelectionChange={onCitySelection}>
                {cities && cities.map((city: any) => (
                    <AutocompleteItem key={city.cityId} value={city.cityId}>
                        {city.name}
                    </AutocompleteItem>
                ))}
            </Autocomplete>
        </div>
    )
}
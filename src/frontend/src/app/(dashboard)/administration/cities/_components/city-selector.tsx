import {
    Avatar,
    Autocomplete,
    AutocompleteItem,
} from "@nextui-org/react";
import { CountryFlagUrl } from "@/components/country-flag";

export const CitySelector = ({ countries, cities, onCountrySelectionChange, onCitySelectionChange }: { countries: any, cities: any, onCountrySelectionChange: any, onCitySelectionChange: any }) => {
    return (
        <div className="flex w-full flex-wrap md:flex-nowrap gap-4">
            <Autocomplete label="Country" onSelectionChange={onCountrySelectionChange}>
                {countries && countries.map((country: any) => (
                    <AutocompleteItem key={country.countryId} value={country.countryId} startContent={<Avatar alt={country.name} className="w-6 h-6" src={CountryFlagUrl(country.iso2.toLowerCase())} />}>
                        {country.name}
                    </AutocompleteItem>
                ))}
            </Autocomplete>

            <Autocomplete label="City" onSelectionChange={onCitySelectionChange}>
                {cities && cities.map((city: any) => (
                    <AutocompleteItem key={city.cityId} value={city.cityId}>
                        {city.name}
                    </AutocompleteItem>
                ))}
            </Autocomplete>
        </div>
    )
}
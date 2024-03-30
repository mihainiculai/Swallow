"use client";

import { useState } from "react";

import { Card, CardBody } from "@nextui-org/react";

import { CitySelector } from "./_components/city-selector";
import { CountryCard } from "./_components/country-card";
import { CityCard } from "./_components/city-card";

interface Country {
    id: number;
    name: string;
    iso2: string;
}

interface City {
    id: number;
    name: string;
}

export default function AdministrativeCitiesPage() {
    const [selectedCountry, setSelectedCountry] = useState<Country | null>(null);
    const [selectedCity, setSelectedCity] = useState<City | null>(null);

    const onCountrySelectionChange = (country: Country) => {
        setSelectedCountry(country);
        setSelectedCity(null);
    }

    const onCitySelectionChange = (city: City) => {
        setSelectedCity(city);
    }

    return (
        <>
            <div className="flex flex-col md:flex-row w-full gap-4">
                <CitySelector
                    selectedCountry={selectedCountry}
                    onCountrySelectionChange={onCountrySelectionChange}
                    onCitySelectionChange={onCitySelectionChange}
                />

                <div className="flex flex-col gap-4 basis-3/4">
                    {!selectedCountry && (
                        <Card className="full-width">
                            <CardBody className="flex justify-center text-center py-32 text-default-500">
                                Select a country and a city to view their details
                            </CardBody>
                        </Card>
                    )}

                    {selectedCountry && !selectedCity && (
                        <CountryCard
                            selectedCountry={selectedCountry}
                        />
                    )}

                    {selectedCity && (
                        <CityCard
                            selectedCity={selectedCity}
                            selectedCountry={selectedCountry}
                        />
                    )}
                </div>
            </div>
        </>
    );
}
"use client";

import { useState, useEffect } from "react";
import {
    Input,
    Textarea,
    Avatar,
    Autocomplete,
    AutocompleteItem,
    Card,
    CardHeader,
    CardBody,
    CardFooter,
} from "@nextui-org/react";
import useSWR from "swr";
import { fetcher } from "@/components/fetcher";
import * as yup from "yup";
import { useFormik } from "formik";
import { axiosInstance } from "@/components/axiosInstance";

const validationSchema = yup.object({
    cityId: yup.number().required("City is required"),
    description: yup.string(),
    pictureURL: yup.string().url("Invalid URL format"),
});

export default function AdministrativeCitiesPage() {
    const { data: countries } = useSWR('countries', fetcher);
    const [selectedCountry, setSelectedCountry] = useState(null);
    const { data: cities } = useSWR(`countries/${selectedCountry}/cities`, fetcher)
    const [selectedCity, setSelectedCity] = useState(null);
    const { data: city } = useSWR(`cities/${selectedCity}`, fetcher);

    const onCountrySelectionChange = (id: any) => {
        setSelectedCountry(id);
        setSelectedCity(null);
    };

    const onCitySelectionChange = (id: any) => {
        setSelectedCity(id);
    }

    const formik = useFormik({
        initialValues: {
            cityId: city?.cityId || "",
            description: city?.description || "",
            pictureURL: city?.pictureURL || "",
        },
        validationSchema: validationSchema,
        onSubmit: (values) => {

        },
    });

    useEffect(() => {
        formik.setValues({
            cityId: city?.cityId || "",
            description: city?.description || "",
            pictureURL: city?.pictureURL || "",
        });
    }, [city]);

    return (
        <>
            <div className="flex w-full flex-wrap md:flex-nowrap gap-4">
                <Autocomplete label="Country" onSelectionChange={onCountrySelectionChange}>
                    {countries && countries.map((country: any) => (
                        <AutocompleteItem key={country.countryId} value={country.countryId} startContent={<Avatar alt={country.name} className="w-6 h-6" src={'https://flagcdn.com/' + country.iso2.toLowerCase() + '.svg'} />}>
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
            {city && (
                <Card className="full-width p-4 mt-4">
                    <CardBody className="gap-8">
                        <Textarea
                            label="Description"
                            isInvalid={formik.touched.description && !!formik.errors.description}
                            {...formik.getFieldProps("description")}
                        />
                        <Input
                            label="Picture URL"
                            isInvalid={formik.touched.pictureURL && !!formik.errors.pictureURL}
                            errorMessage={formik.touched.pictureURL && formik.errors.pictureURL}
                            {...formik.getFieldProps("pictureURL")}
                        />
                    </CardBody>
                </Card>
            )}
        </>
    );
}
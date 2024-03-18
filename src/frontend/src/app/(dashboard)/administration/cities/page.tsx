"use client";

import { useState, useEffect } from "react";

import {
    Input,
    Textarea,
    Avatar,
    Card,
    CardHeader,
    CardBody,
    CardFooter,
    Button,
    useDisclosure,
} from "@nextui-org/react";

import useSWR, { mutate } from "swr";
import { fetcher } from "@/components/fetcher";
import { axiosInstance } from "@/components/axiosInstance";

import * as yup from "yup";
import { useFormik } from "formik";

import { RiAiGenerate } from "react-icons/ri";

import { CountryFlagUrl } from "@/components/country-flag";
import { CitySelector } from "./_components/city-selector";
import { AttractionsModal } from "./_components/attractions-modal";

const validationSchema = yup.object({
    cityId: yup.number().required("City is required"),
    description: yup.string(),
    pictureURL: yup.string().url("Invalid URL format"),
    tripAdvisorURL: yup.string().url("Invalid URL format"),
});

export default function AdministrativeCitiesPage() {
    const { data: countries } = useSWR('countries', fetcher);
    const [selectedCountry, setSelectedCountry] = useState(null);
    const { data: cities } = useSWR(`countries/${selectedCountry}/cities`, fetcher)
    const [selectedCity, setSelectedCity] = useState(null);
    const { data: city } = useSWR(`cities/${selectedCity}`, fetcher);

    const [isGeneratingDescription, setIsGeneratingDescription] = useState(false);
    const [isSubmitting, setIsSubmitting] = useState(false);

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
            tripAdvisorURL: city?.tripAdvisorURL || "",
        },
        validationSchema: validationSchema,
        onSubmit: (values) => {
            setIsSubmitting(true);

            axiosInstance.put(`cities`, values)
                .then(() => {
                    mutate(`cities/${selectedCity}`);
                })
                .finally(() => {
                    setIsSubmitting(false);
                });
        },
    });

    useEffect(() => {
        formik.setValues({
            cityId: city?.cityId || "",
            description: city?.description || "",
            pictureURL: city?.pictureURL || "",
            tripAdvisorURL: city?.tripAdvisorURL || "",
        });
        // eslint-disable-next-line
    }, [city]);

    const handleGenerateDescription = () => {
        setIsGeneratingDescription(true);
        axiosInstance.get(`cities/${selectedCity}/generate-description`, { timeout: 30000 })
            .then((response) => {
                formik.setFieldValue("description", response.data);
            })
            .finally(() => {
                setIsGeneratingDescription(false);
            });
    }

    const { isOpen, onOpen, onOpenChange } = useDisclosure();

    return (
        <>
            <div className="flex flex-col md:flex-row w-full gap-4">
                <CitySelector
                    countries={countries}
                    cities={cities}
                    onCountrySelectionChange={onCountrySelectionChange}
                    onCitySelectionChange={onCitySelectionChange}
                />

                {city ? (
                    <>
                        <Card className="full-width p-4 basis-3/4">
                            <CardHeader className="flex justify-between items-center px-10 py-8">
                                <div className="flex items-center">
                                    <Avatar
                                        size="md"
                                        alt={city.name}
                                        className="mr-8 inline-block"
                                        src={CountryFlagUrl(countries.find((c: any) => c.countryId == selectedCountry)?.iso2.toLowerCase())}
                                    />
                                    <h2 className="text-xl font-semibold">
                                        {cities.find((c: any) => c.cityId === city.cityId)?.name}, {countries.find((c: any) => c.countryId == selectedCountry)?.name}
                                    </h2>
                                </div>
                            </CardHeader>

                            <CardBody className="gap-8">
                                <form className="flex w-full flex-col items-start rounded-medium bg-default-100 transition-colors hover:bg-default-200">
                                    <Textarea
                                        classNames={{
                                            inputWrapper: "!bg-transparent shadow-none",
                                            innerWrapper: "relative",
                                            input: "pt-1 pl-2 pb-6 !pr-10",
                                        }}
                                        label="Description"
                                        isInvalid={formik.touched.description && !!formik.errors.description}
                                        {...formik.getFieldProps("description")}
                                    />
                                    <div className="flex w-full items-center px-4 pb-4">
                                        <div className="flex w-full justify-end">
                                            <Button
                                                size="sm"
                                                startContent={
                                                    <RiAiGenerate className="text-defau lt-500" />
                                                }
                                                variant="flat"
                                                onClick={handleGenerateDescription}
                                                isLoading={isGeneratingDescription}
                                            >
                                                Generate
                                            </Button>
                                        </div>
                                    </div>
                                </form>
                                <Input
                                    label="Picture URL"
                                    isInvalid={formik.touched.pictureURL && !!formik.errors.pictureURL}
                                    errorMessage={formik.touched.pictureURL && typeof formik.errors.pictureURL === "string" ? formik.errors.pictureURL : ""}
                                    {...formik.getFieldProps("pictureURL")}
                                />
                                <Input
                                    label="TripAdvisor URL"
                                    isInvalid={formik.touched.tripAdvisorURL && !!formik.errors.tripAdvisorURL}
                                    errorMessage={formik.touched.tripAdvisorURL && typeof formik.errors.tripAdvisorURL === "string" ? formik.errors.tripAdvisorURL : ""}
                                    {...formik.getFieldProps("tripAdvisorURL")}
                                />
                            </CardBody>

                            <CardFooter className="my-4 justify-end gap-2">
                                <Button variant="light" onPress={onOpen}>
                                    Show attractions
                                </Button>
                                <Button color="primary" onClick={formik.submitForm} isLoading={isSubmitting}>
                                    Save Changes
                                </Button>
                            </CardFooter>
                        </Card>

                        <AttractionsModal isOpen={isOpen} onOpenChange={onOpenChange} cityId={selectedCity} cityName={cities.find((c: any) => c.cityId === city.cityId)?.name + ", " + countries.find((c: any) => c.countryId == selectedCountry)?.name} />
                    </>
                ) : (
                    <Card className="full-width basis-3/4">
                        <CardBody className="flex justify-center text-center py-32 text-default-500">
                            Select a city to view its details
                        </CardBody>
                    </Card>
                )}
            </div>
        </>
    );
}
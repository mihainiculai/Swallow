import React, { useState, useEffect } from "react";

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
import { fetcher } from "@/components/utilities/fetcher";
import { axiosInstance } from "@/components/utilities/axiosInstance";

import * as yup from "yup";
import { useFormik } from "formik";

import { RiAiGenerate } from "react-icons/ri";

import { CountryFlagUrl } from "@/components/country-flag";
import { AttractionsModal } from "./attractions-modal";

const validationSchema = yup.object({
    cityId: yup.number().required("City is required"),
    description: yup.string(),
    pictureUrl: yup.string().url("Invalid URL format"),
    tripAdvisorUrl: yup.string().url("Invalid URL format"),
});

export const CityCard = ({ selectedCity, selectedCountry }: { selectedCity: any, selectedCountry: any }) => {
    const { data: city } = useSWR(selectedCity?.id ? `cities/${selectedCity?.id}` : null, fetcher);

    const [isGeneratingDescription, setIsGeneratingDescription] = useState(false);
    const [isSubmitting, setIsSubmitting] = useState(false);

    const formik = useFormik({
        initialValues: {
            cityId: city?.cityId || "",
            description: city?.description || "",
            pictureUrl: city?.pictureUrl || "",
            tripAdvisorUrl: city?.tripAdvisorUrl || "",
        },
        validationSchema: validationSchema,
        onSubmit: (values) => {
            setIsSubmitting(true);

            axiosInstance.put(`cities`, values)
                .then(() => {
                    mutate(`cities/${selectedCity.id}`);
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
            pictureUrl: city?.pictureUrl || "",
            tripAdvisorUrl: city?.tripAdvisorUrl || "",
        });
        // eslint-disable-next-line
    }, [city]);

    const handleGenerateDescription = () => {
        setIsGeneratingDescription(true);
        axiosInstance.get(`cities/${selectedCity.id}/generate-description`, { timeout: 30000 })
            .then((response) => {
                formik.setFieldValue("description", response.data);
            })
            .finally(() => {
                setIsGeneratingDescription(false);
            });
    }

    const { isOpen, onOpen, onOpenChange } = useDisclosure();

    if (!city) return null;

    return (
        <>
            <Card className="full-width p-4">
                <CardHeader className="flex justify-between items-center px-10 py-8">
                    <div className="flex items-center">
                        <Avatar
                            size="md"
                            alt={city.name}
                            className="mr-8 inline-block"
                            src={CountryFlagUrl(selectedCountry?.iso2.toLowerCase() || "")}
                        />
                        <h2 className="text-xl font-semibold">
                            {selectedCity?.name}, {selectedCountry?.name}
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
                        isInvalid={formik.touched.pictureUrl && !!formik.errors.pictureUrl}
                        errorMessage={formik.touched.pictureUrl && typeof formik.errors.pictureUrl === "string" ? formik.errors.pictureUrl : ""}
                        {...formik.getFieldProps("pictureUrl")}
                    />
                    <Input
                        label="TripAdvisor URL"
                        isInvalid={formik.touched.tripAdvisorUrl && !!formik.errors.tripAdvisorUrl}
                        errorMessage={formik.touched.tripAdvisorUrl && typeof formik.errors.tripAdvisorUrl === "string" ? formik.errors.tripAdvisorUrl : ""}
                        {...formik.getFieldProps("tripAdvisorUrl")}
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

            <AttractionsModal isOpen={isOpen} onOpenChange={onOpenChange} cityId={selectedCity?.id} cityName={selectedCity?.name + ", " + selectedCountry?.name} />
        </>
    );
}
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
} from "@nextui-org/react";

import useSWR, { mutate } from "swr";
import { fetcher } from "@/components/utilities/fetcher";
import { axiosInstance } from "@/components/utilities/axiosInstance";

import * as yup from "yup";
import { useFormik } from "formik";

import { RiAiGenerate } from "react-icons/ri";
import { CountryFlagUrl } from "@/components/country-flag";

const validationSchema = yup.object({
    countryId: yup.number().required("Country is required"),
    description: yup.string(),
    pictureUrl: yup.string().url("Invalid URL format"),
});

export const CountryCard = ({ selectedCountry }: { selectedCountry: any }) => {
    const { data: country } = useSWR(selectedCountry?.id ? `countries/${selectedCountry.id}` : null, fetcher);

    const [isGeneratingDescription, setIsGeneratingDescription] = useState(false);
    const [isSubmitting, setIsSubmitting] = useState(false);

    const formik = useFormik({
        initialValues: {
            countryId: country?.countryId || "",
            description: country?.description || "",
            pictureUrl: country?.pictureUrl || "",
        },
        validationSchema: validationSchema,
        onSubmit: (values) => {
            setIsSubmitting(true);

            axiosInstance.put(`countries`, values)
                .then(() => {
                    mutate(`countries/${selectedCountry?.id}`);
                })
                .finally(() => {
                    setIsSubmitting(false);
                });
        },
    });

    useEffect(() => {
        formik.setValues({
            countryId: country?.countryId || "",
            description: country?.description || "",
            pictureUrl: country?.pictureUrl || "",
        });
        // eslint-disable-next-line
    }, [country]);

    const handleGenerateDescription = () => {
        setIsGeneratingDescription(true);
        axiosInstance.get(`countries/${selectedCountry.id}/generate-description`, { timeout: 30000 })
            .then((response) => {
                formik.setFieldValue("description", response.data);
            })
            .finally(() => {
                setIsGeneratingDescription(false);
            });
    }

    if (!country) return null;

    return (
        <>
            <Card className="full-width p-4">
                <CardHeader className="flex justify-between items-center px-10 py-8">
                    <div className="flex items-center">
                        <Avatar
                            size="md"
                            alt={selectedCountry?.name}
                            className="mr-8 inline-block"
                            src={CountryFlagUrl(selectedCountry?.iso2.toLowerCase() || "")}
                        />
                        <h2 className="text-xl font-semibold">
                            {selectedCountry?.name}
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
                </CardBody>

                <CardFooter className="my-4 justify-end gap-2">
                    <Button color="primary" onClick={formik.submitForm} isLoading={isSubmitting}>
                        Save Changes
                    </Button>
                </CardFooter>
            </Card>
        </>
    );
}
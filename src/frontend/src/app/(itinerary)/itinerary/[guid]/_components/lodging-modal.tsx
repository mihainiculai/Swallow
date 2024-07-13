"use client"

import React, { useState, useEffect } from "react";
import { SearchPlaceAutocomplete } from "@/components/ui-elements/search-place-autocomplete";
import { Modal, ModalContent, ModalHeader, ModalBody, ModalFooter, Button, useDisclosure } from "@nextui-org/react";
import { useFormik } from "formik";
import * as Yup from "yup";
import { Key } from '@react-types/shared';

import { mutate } from "swr";
import { axiosInstance } from "@/components/utilities/axiosInstance";

const validationSchema = Yup.object({
    lodgingPlaceId: Yup.string(),
    lodgingSessionToken: Yup.string(),
});

interface LodgingModalProps {
    tripId: string;
    cityId: number;
    lodging: any;
    isOpen: boolean;
    onOpen: () => void;
    onOpenChange: () => void;
}

export const LodgingModal = ({ tripId, cityId, lodging, isOpen, onOpen, onOpenChange }: LodgingModalProps) => {
    const formik = useFormik({
        initialValues: {
            lodgingPlaceId: lodging?.googlePlaceId,
            lodgingSessionToken: "",
        },
        validationSchema,
        onSubmit: async (values) => {
            await axiosInstance.post(`/itineraries/${tripId}/lodging`, {
                placeId: values.lodgingPlaceId,
                sessionToken: values.lodgingSessionToken,
            }).then(() => {
                mutate(`/itineraries/${tripId}`);
                onOpenChange();
            });
        },
    });

    useEffect(() => {
        formik.resetForm();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [isOpen]);

    return (
        <Modal isOpen={isOpen} onOpenChange={onOpenChange}>
            <ModalContent>
                {(onClose) => (
                    <>
                        <form onSubmit={formik.handleSubmit}>
                        <ModalHeader className="flex flex-col gap-1">Lodging details</ModalHeader>
                        <ModalBody>
                            <SearchPlaceAutocomplete
                                cityId={cityId}
                                value={formik.values.lodgingPlaceId}
                                setValue={(value: Key) => formik.setFieldValue('lodgingPlaceId', value)}
                                setSessionToken={(token: string) => formik.setFieldValue('lodgingSessionToken', token)}
                                isInvalid={formik.touched.lodgingPlaceId && !!formik.errors.lodgingPlaceId}
                                errorMessage={formik.touched.lodgingPlaceId && formik.errors.lodgingPlaceId}
                                defaultValue={lodging?.name ?? ''}
                            />
                        </ModalBody>
                        <ModalFooter>
                            <Button color="danger" variant="light" onPress={onClose}>
                                Close
                            </Button>
                            <Button color="primary" type="submit">
                                Save
                            </Button>
                        </ModalFooter>
                        </form>
                    </>
                )}
            </ModalContent>
        </Modal>
    )
}
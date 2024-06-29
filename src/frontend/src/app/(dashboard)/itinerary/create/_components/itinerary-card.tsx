import React, { useEffect } from 'react';
import { Key } from '@react-types/shared';

import {
    Autocomplete,
    AutocompleteItem,
    Button,
    Card,
    CardBody,
    CardFooter,
    CardHeader,
    TimeInput
} from '@nextui-org/react';

import * as yup from "yup";
import { useFormik } from "formik";

import useSWRImmutable from 'swr/immutable';
import { fetcher } from '@/components/utilities/fetcher';

import { axiosInstance } from '@/components/utilities/axiosInstance';

import { FaBusAlt, FaCarAlt } from "react-icons/fa";
import { FaTrainTram } from "react-icons/fa6";
import { IoAirplane } from "react-icons/io5";

import { SearchPlaceAutocomplete } from '@/components/ui-elements/search-place-autocomplete';
import { Time } from "@internationalized/date";
import { Required } from './required';

interface AutoItineraryCardProps {
    cityId: number | null;
    startDate: string;
    endDate: string;
    itineraryType: number;
}

const validationSchema = yup.object({
    cityId: yup.number().required(),
    itineraryType: yup.number().required(),
    startDate: yup.string().required(),
    endDate: yup.string().required(),
    transportModeId: yup.number().min(1, "Transportation mode is required").required("Transportation mode is required"),
    lodgingPlaceId: yup.string().when('itineraryType', (itineraryType, schema) => {
        return parseInt(itineraryType.toString()) === 1 ? schema.required('Lodging is required') : schema;
    }),
    lodgingSessionToken: yup.string(),
    arrivingTransportModeId: yup.number().when('itineraryType', (itineraryType, schema) => {
        return parseInt(itineraryType.toString()) === 1 ? schema.min(1, 'Arriving transportation mode is required') : schema;
    }),
    arrivingPlaceId: yup.string().when('itineraryType', (itineraryType, schema) => {
        return parseInt(itineraryType.toString()) === 1 ? schema.required('Arriving location is required') : schema;
    }),
    arrivingSessionToken: yup.string(),
    arrivingTime: yup.object({
        hour: yup.number(),
        minute: yup.number(),
    }),
    departingTransportModeId: yup.number().when('itineraryType', (itineraryType, schema) => {
        return parseInt(itineraryType.toString()) === 1 ? schema.min(1, 'Departing transportation mode is required') : schema;
    }),
    departingPlaceId: yup.string().when('itineraryType', (itineraryType, schema) => {
        return parseInt(itineraryType.toString()) === 1 ? schema.required('Departing location is required') : schema;
    }),
    departingSessionToken: yup.string(),
    departingTime: yup.object({
        hour: yup.number(),
        minute: yup.number(),
    }),
});

export const AutoItineraryCard: React.FC<AutoItineraryCardProps> = ({ cityId, startDate, endDate, itineraryType }) => {
    const { data: transportModes } = useSWRImmutable("trips/transport-modes", fetcher);
    const { data: transportTypes } = useSWRImmutable("trips/transport-types", fetcher);

    const formik = useFormik({
        initialValues: {
            cityId: cityId || 0,
            itineraryType: itineraryType || 0,
            startDate: startDate || "",
            endDate: endDate || "",
            transportModeId: 0,
            lodgingPlaceId: "",
            lodgingSessionToken: "",
            arrivingTransportModeId: 0,
            arrivingPlaceId: "",
            arrivingSessionToken: "",
            arrivingTime: new Time(12, 0),
            departingTransportModeId: 0,
            departingPlaceId: "",
            departingSessionToken: "",
            departingTime: new Time(12, 0),
        },
        validationSchema: validationSchema,
        onSubmit: (values) => {
            axiosInstance.post("/itineraries", values)
        }
    })

    useEffect(() => {
        formik.setFieldValue('cityId', cityId)
        formik.setFieldValue('startDate', startDate)
        formik.setFieldValue('endDate', endDate)
        formik.setFieldValue('itineraryType', itineraryType)
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [cityId, startDate, endDate, itineraryType])

    return (
        <Card className="w-full p-6">
            <form onSubmit={formik.handleSubmit}>
                <CardHeader className="flex flex-col items-start px-4 pb-0 pt-4">
                    <p className="text-large">Itinerary Details</p>
                    <p className="text-small text-default-500">Fill in the details to generate an itinerary</p>
                    <p className="text-small text-default-400"><Required />indicates required fields</p>
                </CardHeader>
                <CardBody className="flex flex-col gap-4">
                    <h3 className="text-large mt-6">Itinerary</h3>
                    <div className='flex flex-col md:flex-row gap-4 justify-between md:items-center'>
                        <div className='basis-3/5'>
                            <p className="text-default-500">Transportation mode<Required /></p>
                        </div>
                        <div className='basis-2/5'>
                            <Autocomplete
                                placeholder='Select a transport mode'
                                aria-label='Select a transport mode'
                                size='lg'
                                fullWidth
                                defaultItems={transportModes || []}
                                selectedKey={formik.values.transportModeId}
                                onSelectionChange={(key: Key) => formik.setFieldValue('transportModeId', key)}
                                isInvalid={formik.touched.transportModeId && !!formik.errors.transportModeId}
                                errorMessage={formik.touched.transportModeId && formik.errors.transportModeId}
                            >
                                {(item: any) => (
                                    <AutocompleteItem key={item.transportModeId} textValue={item.name}>
                                        <div className="flex gap-4 items-center">
                                            {item.transportModeId === 1 && <FaBusAlt />}
                                            {item.transportModeId === 2 && <FaCarAlt />}
                                            <span>{item.name}</span>
                                        </div>
                                    </AutocompleteItem>
                                )}
                            </Autocomplete>
                        </div>
                    </div>
                    <div className='flex flex-col md:flex-row gap-4 justify-between md:items-center'>
                        <div className='basis-3/5'>
                            <p className="text-default-500">Lodging<Required required={itineraryType === 1} /></p>
                        </div>
                        <div className='basis-2/5'>
                            <SearchPlaceAutocomplete
                                cityId={cityId}
                                value={formik.values.lodgingPlaceId}
                                setValue={(value: Key) => formik.setFieldValue('lodgingPlaceId', value)}
                                setSessionToken={(token: string) => formik.setFieldValue('lodgingSessionToken', token)}
                                isInvalid={formik.touched.lodgingPlaceId && !!formik.errors.lodgingPlaceId}
                                errorMessage={formik.touched.lodgingPlaceId && formik.errors.lodgingPlaceId}
                            />
                        </div>
                    </div>

                    <h3 className="text-large mt-6">Arriving</h3>
                    <div className='flex flex-col md:flex-row gap-4 justify-between md:items-center'>
                        <div className='basis-3/5'>
                            <p className="text-default-500">Transportation mode<Required required={itineraryType === 1} /></p>
                        </div>
                        <div className='basis-2/5'>
                            <Autocomplete
                                placeholder='Select a transport mode'
                                aria-label='Select a transport mode'
                                size='lg'
                                fullWidth
                                defaultItems={transportTypes || []}
                                selectedKey={formik.values.arrivingTransportModeId}
                                onSelectionChange={(key: Key) => formik.setFieldValue('arrivingTransportModeId', key)}
                                isInvalid={formik.touched.arrivingTransportModeId && !!formik.errors.arrivingTransportModeId}
                                errorMessage={formik.touched.arrivingTransportModeId && formik.errors.arrivingTransportModeId}
                            >
                                {(item: any) => (
                                    <AutocompleteItem key={item.transportTypeId} textValue={item.name}>
                                        <div className="flex gap-4 items-center">
                                            {item.transportTypeId === 1 && <IoAirplane />}
                                            {item.transportTypeId === 2 && <FaTrainTram />}
                                            {item.transportTypeId === 3 && <FaBusAlt />}
                                            {item.transportTypeId === 4 && <FaCarAlt />}
                                            <span>{item.name}</span>
                                        </div>
                                    </AutocompleteItem>
                                )}
                            </Autocomplete>
                        </div>
                    </div>
                    <div className='flex flex-col md:flex-row gap-4 justify-between md:items-center'>
                        <div className='basis-3/5'>
                            <p className="text-default-500">Arriving time<Required required={itineraryType === 1} /></p>
                        </div>
                        <div className='basis-2/5'>
                            <TimeInput
                                fullWidth
                                aria-label='Arriving time'
                                value={formik.values.arrivingTime}
                                onChange={(value) => formik.setFieldValue('arrivingTime', value)} />
                        </div>
                    </div>
                    <div className='flex flex-col md:flex-row gap-4 justify-between md:items-center'>
                        <div className='basis-3/5'>
                            <p className="text-default-500">Location<Required required={itineraryType === 1} /></p>
                        </div>
                        <div className='basis-2/5'>
                            <SearchPlaceAutocomplete
                                cityId={cityId}
                                value={formik.values.arrivingPlaceId}
                                setValue={(value: Key) => formik.setFieldValue('arrivingPlaceId', value)}
                                setSessionToken={(token: string) => formik.setFieldValue('arrivingSessionToken', token)}
                                isInvalid={formik.touched.arrivingPlaceId && !!formik.errors.arrivingPlaceId}
                                errorMessage={formik.touched.arrivingPlaceId && formik.errors.arrivingPlaceId}
                            />
                        </div>
                    </div>

                    <h3 className="text-large mt-6">Departing</h3>
                    <div className='flex flex-col md:flex-row gap-4 justify-between md:items-center'>
                        <div className='basis-3/5'>
                            <p className="text-default-500">Transportation mode<Required required={itineraryType === 1} /></p>
                        </div>
                        <div className='basis-2/5'>
                            <Autocomplete
                                placeholder='Select a transport mode'
                                aria-label='Select a transport mode'
                                size='lg'
                                fullWidth
                                defaultItems={transportTypes || []}
                                selectedKey={formik.values.departingTransportModeId}
                                onSelectionChange={(key: Key) => formik.setFieldValue('departingTransportModeId', key)}
                                isInvalid={formik.touched.departingTransportModeId && !!formik.errors.departingTransportModeId}
                                errorMessage={formik.touched.departingTransportModeId && formik.errors.departingTransportModeId}
                            >
                                {(item: any) => (
                                    <AutocompleteItem key={item.transportTypeId} textValue={item.name}>
                                        <div className="flex gap-4 items-center">
                                            {item.transportTypeId === 1 && <IoAirplane />}
                                            {item.transportTypeId === 2 && <FaTrainTram />}
                                            {item.transportTypeId === 3 && <FaBusAlt />}
                                            {item.transportTypeId === 4 && <FaCarAlt />}
                                            <span>{item.name}</span>
                                        </div>
                                    </AutocompleteItem>
                                )}
                            </Autocomplete>
                        </div>
                    </div>
                    <div className='flex flex-col md:flex-row gap-4 justify-between md:items-center'>
                        <div className='basis-3/5'>
                            <p className="text-default-500">Departing time<Required required={itineraryType === 1} /></p>
                        </div>
                        <div className='basis-2/5'>
                            <TimeInput
                                fullWidth
                                aria-label='Departing time'
                                value={formik.values.departingTime}
                                onChange={(value) => formik.setFieldValue('departingTime', value)}
                            />
                        </div>
                    </div>
                    <div className='flex flex-col md:flex-row gap-4 justify-between md:items-center'>
                        <div className='basis-3/5'>
                            <p className="text-default-500">Location<Required required={itineraryType === 1} /></p>
                        </div>
                        <div className='basis-2/5'>
                            <SearchPlaceAutocomplete
                                cityId={cityId}
                                value={formik.values.departingPlaceId}
                                setValue={(value: Key) => formik.setFieldValue('departingPlaceId', value)}
                                setSessionToken={(token: string) => formik.setFieldValue('departingSessionToken', token)}
                                isInvalid={formik.touched.departingPlaceId && !!formik.errors.departingPlaceId}
                                errorMessage={formik.touched.departingPlaceId && formik.errors.departingPlaceId}
                            />
                        </div>
                    </div>
                </CardBody>
                <CardFooter className="flex justify-end pb-2">
                    <Button color='primary' type='submit'>
                        Generate
                    </Button>
                </CardFooter>
            </form>
        </Card>
    )
}
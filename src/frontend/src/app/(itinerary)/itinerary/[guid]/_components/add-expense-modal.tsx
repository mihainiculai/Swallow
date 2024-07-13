"use client";

import React, { useEffect } from 'react';

import { Modal, ModalContent, ModalHeader, ModalBody, ModalFooter, Button, Input, Select, SelectItem, Spacer } from "@nextui-org/react";
import * as yup from "yup";
import { useFormik } from "formik";
import { mutate } from 'swr';
import useSWRImmutable from "swr/immutable";
import { axiosInstance } from '@/components/utilities/axiosInstance';
import ExpenseIcon from "./expense-icons";
import { fetcher } from "@/components/utilities/fetcher";

interface AddExpenseModalProps {
    isOpen: boolean;
    onOpen: () => void;
    onOpenChange: () => void;
    itineraryId: string;
    currencies: { currencyId: number; name: string; code: string; symbol: string }[];
}

interface ExpenseCategory {
    expenseCategoryId: number;
    name: string;
}

const validationSchema = yup.object({
    name: yup.string().required("Name is required"),
    description: yup.string().required("Description is required"),
    price: yup
        .number()
        .typeError("Price must be a number")
        .test(
            "is-decimal",
            "Price must have 2 decimal places",
            (value) => value === undefined || /^\d+(\.\d{1,2})?$/.test(value.toString())
        )
        .required("Price is required")
        .positive("Price must be a positive number"),
    expenseCategoryId: yup.number().positive("Category is required"),
    currencyId: yup.number().required("Currency is required"),
});

export default function AddExpenseModal({ isOpen, onOpen, onOpenChange, itineraryId, currencies }: AddExpenseModalProps) {
    const { data: expenseCategories } = useSWRImmutable<ExpenseCategory[]>("expenses/categories", fetcher);

    const formik = useFormik({
        initialValues: {
            name: "",
            description: "",
            price: 0,
            expenseCategoryId: 0,
            currencyId: 146,
        },
        validationSchema: validationSchema,
        onSubmit: (values) => {
            axiosInstance.post("/expenses", {
                ...values,
                tripId: itineraryId,
            })

            mutate(`/itineraries/${itineraryId}`);
            onOpenChange();
        }
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
                        <ModalHeader>Add Expense</ModalHeader>
                        <ModalBody>
                            <form onSubmit={formik.handleSubmit} className="flex flex-col gap-4">
                                <Input
                                    label="Name"
                                    name="name"
                                    value={formik.values.name}
                                    onChange={formik.handleChange}
                                    onBlur={formik.handleBlur}
                                    isInvalid={formik.touched.name && Boolean(formik.errors.name)}
                                    errorMessage={formik.touched.name && formik.errors.name}
                                />
                                <Input
                                    label="Description"
                                    name="description"
                                    value={formik.values.description}
                                    onChange={formik.handleChange}
                                    onBlur={formik.handleBlur}
                                    isInvalid={formik.touched.description && Boolean(formik.errors.description)}
                                    errorMessage={formik.touched.description && formik.errors.description}
                                />
                                <Select
                                    label="Category"
                                    name="expenseCategoryId"
                                    selectedKeys={[formik.values.expenseCategoryId.toString()]}
                                    onChange={formik.handleChange}
                                    onBlur={formik.handleBlur}
                                    isInvalid={formik.touched.expenseCategoryId && Boolean(formik.errors.expenseCategoryId)}
                                    errorMessage={formik.touched.expenseCategoryId && formik.errors.expenseCategoryId}
                                >
                                    {expenseCategories?.map((category) => (
                                        <SelectItem
                                            key={category.expenseCategoryId.toString()}
                                            value={category.expenseCategoryId}
                                            startContent={<ExpenseIcon id={category.expenseCategoryId} />}
                                        >
                                            {category.name}
                                        </SelectItem>
                                    ))}
                                </Select>
                                <Spacer y={1} />
                                <Select
                                    label="Currency"
                                    name="currencyId"
                                    selectedKeys={[formik.values.currencyId.toString()]}
                                    onChange={formik.handleChange}
                                    onBlur={formik.handleBlur}
                                    isInvalid={formik.touched.currencyId && Boolean(formik.errors.currencyId)}
                                    errorMessage={formik.touched.currencyId && formik.errors.currencyId}
                                >
                                    {currencies?.map((currency) => (
                                        <SelectItem
                                            key={currency.currencyId.toString()}
                                            value={currency.currencyId}
                                            startContent={
                                                <span className="text-default-500 w-8">
                                                    {currency.symbol}
                                                </span>
                                            }
                                        >
                                            {currency.name}
                                        </SelectItem>
                                    ))}
                                </Select>
                                <Input
                                    label="Price"
                                    name="price"
                                    type="text"
                                    value={formik.values.price.toString()}
                                    onChange={(e) => {
                                        const value = e.target.value;
                                        if (/^\d*\.?\d{0,2}$/.test(value) || value === '') {
                                            formik.setFieldValue('price', value);
                                        }
                                    }}
                                    onBlur={formik.handleBlur}
                                    isInvalid={formik.touched.price && Boolean(formik.errors.price)}
                                    errorMessage={formik.touched.price && formik.errors.price}
                                    endContent={
                                        <div className="pointer-events-none flex items-center">
                                            <span className="text-default-400 text-small">
                                                {formik.values.currencyId === 146 ? "$" : currencies.find(currency => currency.currencyId === formik.values.currencyId)?.symbol}
                                            </span>
                                        </div>
                                    }
                                />
                            </form>
                        </ModalBody>
                        <ModalFooter>
                            <Button onClick={onClose} variant="light">Cancel</Button>
                            <Button onClick={() => formik.handleSubmit()} color="primary">Save</Button>
                        </ModalFooter>
                    </>
                )}
            </ModalContent>
        </Modal>
    )
}
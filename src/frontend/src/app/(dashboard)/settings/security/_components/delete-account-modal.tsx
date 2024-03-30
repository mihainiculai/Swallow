"use client";
import React, { useEffect, useState } from 'react';
import {
    Button,
    Modal,
    ModalContent,
    ModalHeader,
    ModalBody,
    ModalFooter,
    Input
} from '@nextui-org/react';
import { axiosInstance } from '@/components/utilities/axiosInstance';
import * as yup from "yup";
import { useFormik } from "formik";

interface DeleteAccountModalProps {
    isOpen: boolean;
    onOpenChange: (isOpen: boolean) => void;
}

const validationSchema = yup.object({
    password: yup.string().required("Current password is required"),
});

export const DeleteAccountModal: React.FC<DeleteAccountModalProps> = ({ isOpen, onOpenChange }) => {
    const [error, setError] = useState(null);
    const [isEmailSent, setIsEmailSent] = useState(false);

    const formik = useFormik({
        initialValues: {
            password: "",
        },
        validationSchema,
        onSubmit: async (values) => {
            console.log(values);
            axiosInstance.post("users/request-delete-account", values)
                .then(() => {
                    formik.resetForm();
                    setError(null);
                    setIsEmailSent(true);
                })
                .catch((error) => {
                    if (error.response.data == "Invalid password.") {
                        formik.setFieldError("password", "Invalid password.");
                    } else {
                        setError(error.response.data);
                    }
                });
        }
    });

    useEffect(() => {
        formik.resetForm();
        setError(null);
        setIsEmailSent(false);
        // eslint-disable-next-line
    }, [isOpen]);

    return (
        <Modal
            placement="center"
            size='xl'
            isOpen={isOpen}
            onOpenChange={onOpenChange}
        >
            <ModalContent>
                {!isEmailSent && (
                    <>
                        <ModalHeader>Delete Account</ModalHeader>
                        <ModalBody className="flex flex-col gap-6">
                            <p className="text-sm">
                                Please note that deleting your account is irreversible. Upon deletion:
                            </p>
                            <ul className="text-sm list-disc list-inside">
                                <li>All your data will be permanently lost</li>
                                <li>Any active subscriptions will be terminated without a refund</li>
                                <li>You will lose access to all services and features associated with your account</li>
                                <li>This action cannot be undone</li>
                            </ul>
                            <Input
                                type="password"
                                label="Your Current Password"
                                isRequired
                                isInvalid={formik.touched.password && !!formik.errors.password}
                                errorMessage={formik.touched.password && formik.errors.password}
                                {...formik.getFieldProps("password")}
                            />
                            {error && <p className="text-danger">{error}</p>}
                        </ModalBody>

                        <ModalFooter className="mt-4">
                            <Button
                                variant='light'
                                onClick={() => {
                                    formik.resetForm();
                                    onOpenChange(false);
                                }}
                            >
                                Cancel
                            </Button>
                            <Button color="danger" onClick={formik.submitForm}>
                                Delete Account
                            </Button>
                        </ModalFooter>
                    </>
                )}
                {isEmailSent && (
                    <>
                        <ModalHeader>Email Confirmation</ModalHeader>
                        <ModalBody>
                            <p>An email has been sent to your email address.<br />Please check your email to confirm the change.</p>
                        </ModalBody>
                        <ModalFooter>
                            <Button
                                color="primary"
                                onClick={() => {
                                    formik.resetForm();
                                    onOpenChange(false);
                                }}
                            >
                                Close
                            </Button>
                        </ModalFooter>
                    </>
                )}
            </ModalContent>
        </Modal>
    );
}
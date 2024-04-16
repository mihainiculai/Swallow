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
import useSWR, { mutate } from 'swr';
import { fetcher } from '@/components/utilities/fetcher';
import * as yup from "yup";
import { useFormik } from "formik";

interface ChangePasswordModalProps {
    isOpen: boolean;
    onOpenChange: (isOpen: boolean) => void;
}

const validationSchema = yup.object({
    haveOldPassword: yup.boolean().required(),
    oldPassword: yup.string().when("haveOldPassword", {
        is: true,
        then: (schema) => schema.required("Old password is required"),
    }),
    newPassword: yup.string().required("New password is required")
        .min(6, 'Password should be at least 6 characters long')
        .matches(/[a-z]/, 'Password must contain at least one lowercase letter')
        .matches(/[A-Z]/, 'Password must contain at least one uppercase letter')
        .matches(/[0-9]/, 'Password must contain at least one number')
        .matches(/[@$!%*#?&+\-\[\]]/, 'Password must contain at least one special character')
        .matches(/^[a-zA-Z0-9@$!%*#?&+\-\[\]]+$/, 'Password can only contain letters, numbers, and special characters'),
    confirmPassword: yup.string().required("Confirm password is required").oneOf([yup.ref('newPassword')], 'Passwords must match')
});

export const ChangePasswordModal: React.FC<ChangePasswordModalProps> = ({ isOpen, onOpenChange }) => {
    const { data, isLoading } = useSWR('users/change-password', fetcher);
    const [error, setError] = useState<string | null>(null);

    const formik = useFormik({
        initialValues: {
            haveOldPassword: true,
            oldPassword: "",
            newPassword: "",
            confirmPassword: ""
        },
        validationSchema,
        onSubmit: async (values) => {
            axiosInstance.post("users/change-password", {
                currentPassword: values.oldPassword || null,
                newPassword: values.newPassword
            })
                .then(() => {
                    mutate('users/change-password');
                    formik.resetForm();
                    setError(null);
                    onOpenChange(false);
                })
                .catch((error) => {
                    if (error.response.data == "Invalid current password.") {
                        formik.setFieldError("oldPassword", "Invalid current password.");
                    } else {
                        setError(error.response.data);
                    }
                });
        }
    });

    useEffect(() => {
        if (!data) return;
        formik.setFieldValue("haveOldPassword", data.hasPassword);
        // eslint-disable-next-line
    }, [data]);

    useEffect(() => {
        formik.resetForm();
        setError(null);
        if (data) formik.setFieldValue("haveOldPassword", data.hasPassword);
        // eslint-disable-next-line
    }, [isOpen]);

    if (isLoading) return null;

    return (
        <Modal
            placement="center"
            isOpen={isOpen}
            onOpenChange={onOpenChange}
        >
            <ModalContent>
                <>
                    <ModalHeader>{formik.values.haveOldPassword ? "Change Password" : "Set Password"}</ModalHeader>
                    <ModalBody className="flex flex-col items-center gap-6">
                        {formik.values.haveOldPassword && (
                            <Input
                                type="password"
                                label="Current Password"
                                isRequired
                                isInvalid={formik.touched.oldPassword && !!formik.errors.oldPassword}
                                errorMessage={formik.touched.oldPassword && formik.errors.oldPassword}
                                {...formik.getFieldProps("oldPassword")}
                            />
                        )}
                        <Input
                            type="password"
                            label="New Password"
                            isRequired
                            isInvalid={formik.touched.newPassword && !!formik.errors.newPassword}
                            errorMessage={formik.touched.newPassword && formik.errors.newPassword}
                            {...formik.getFieldProps("newPassword")}
                        />
                        <Input
                            type="password"
                            label="Confirm Password"
                            isRequired
                            isInvalid={formik.touched.confirmPassword && !!formik.errors.confirmPassword}
                            errorMessage={formik.touched.confirmPassword && formik.errors.confirmPassword}
                            {...formik.getFieldProps("confirmPassword")}
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
                        <Button color="primary" onClick={formik.submitForm}>
                            {formik.values.haveOldPassword ? "Change Password" : "Set Password"}
                        </Button>
                    </ModalFooter>
                </>
            </ModalContent>
        </Modal >
    );
}
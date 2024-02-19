"use client";
import { useEffect, useState } from "react";
import Link from "next/link";
import {
    Card,
    CardHeader,
    CardBody,
    Button,
    Avatar,
    Badge,
    Input,
    Dropdown,
    DropdownTrigger,
    DropdownMenu,
    DropdownItem,
    CardFooter,
    Divider,
    useDisclosure,
} from "@nextui-org/react";
import { MdModeEdit } from "react-icons/md";
import { useAuthContext, AuthContextType } from "@/contexts/auth-context";
import * as yup from "yup";
import { useFormik } from "formik";
import { axiosInstance } from '@/components/axiosInstance';
import { mutate } from 'swr';
import { FaEye, FaEyeSlash } from "react-icons/fa6";
import { ProfilePictureModal } from "./_components/ProfilePictureModal";

const validationSchema = yup.object({
    publicProfile: yup.boolean().required(),
    username: yup.string().when("publicProfile", {
        is: true,
        then: (schema) => schema.required("Username is required"),
    }),
    email: yup.string().email("Invalid email format").required("Email is required"),
    firstName: yup.string().required("First name is required"),
    lastName: yup.string().required("Last name is required"),
    password: yup.string(),
});

export default function AccountSettingsPage() {
    const { user } = useAuthContext() as AuthContextType;

    const formik = useFormik({
        initialValues: {
            publicProfile: user?.public || false,
            username: user?.username || "",
            email: user?.email || "",
            firstName: user?.firstName || "",
            lastName: user?.lastName || "",
            password: "",
        },
        validationSchema: validationSchema,
        onSubmit: (values) => {
            axiosInstance.put("/users/update-profile", values)
                .then(() => {
                    mutate("auth/me");
                })
                .catch((error) => {
                    const response = error.response;

                    if (response) {
                        if (response.status === 400) {
                            formik.setFieldError("currentPassword", response.data);
                        }

                        if (response.status === 409) {
                            if (response.data === "Email already exists.") {
                                formik.setFieldError("email", response.data);
                            }
                            if (response.data === "Username already exists.") {
                                console.log(response.data);
                                formik.setFieldError("username", response.data);
                            }
                        }
                    }
                });
        }
    });

    const handleAccountVisibility = (value: boolean) => {
        formik.setFieldValue("publicProfile", value);

        if (!value) {
            formik.setFieldValue("username", "");
            formik.setFieldTouched("username", false);
        }
    }

    const { isOpen, onOpen, onOpenChange } = useDisclosure();

    return (
        <>
            <Card className="full-width p-2">
                <CardHeader className="flex justify-between items-center px-10 py-4">
                    <div className="flex gap-4 py-4">
                        <Badge
                            classNames={{
                                badge: "w-6 h-6",
                            }}
                            color="primary"
                            content={
                                <Button
                                    isIconOnly
                                    className="p-0 text-primary-foreground"
                                    radius="full"
                                    size="sm"
                                    variant="light"
                                    onClick={onOpen}
                                >
                                    <MdModeEdit />
                                </Button>
                            }
                            placement="bottom-right"
                            shape="circle"
                        >
                            <Avatar className="h-14 w-14" src={user?.profilePictureURL} />
                        </Badge>
                        <div className="flex flex-col items-start justify-center">
                            <p className="font-medium">{user?.fullName}</p>
                            <span className="text-small text-default-500">[PLAN NAME]</span>
                        </div>
                    </div>

                    <div>
                        <Dropdown placement="bottom-end">
                            <DropdownTrigger>
                                <Button
                                    variant="flat"
                                    startContent={formik.values.publicProfile ? <FaEye /> : <FaEyeSlash />}
                                >
                                    {formik.values.publicProfile ? "Public" : "Private"}
                                </Button>
                            </DropdownTrigger>
                            <DropdownMenu aria-label="Account Visibility" variant="flat">
                                <DropdownItem
                                    startContent={<FaEyeSlash />}
                                    onClick={() => handleAccountVisibility(false)}
                                >
                                    Private
                                </DropdownItem>
                                <DropdownItem
                                    startContent={<FaEye />}
                                    onClick={() => handleAccountVisibility(true)}
                                >
                                    Public
                                </DropdownItem>
                            </DropdownMenu>
                        </Dropdown>
                    </div>
                </CardHeader>

                <div className="px-4 pb-6">
                    <Divider />
                </div>

                <CardBody className="grid grid-cols-1 gap-8 md:grid-cols-2">
                    <Input
                        label="Username"
                        placeholder={!formik.values.publicProfile ? "Required just for public accounts" : ""}
                        isDisabled={!formik.values.publicProfile}
                        isRequired={formik.values.publicProfile}
                        isInvalid={formik.touched.username && !!formik.errors.username}
                        errorMessage={formik.touched.username && formik.errors.username}
                        {...formik.getFieldProps("username")}
                    />

                    <Input
                        type="email"
                        label="Email"
                        isRequired
                        isInvalid={formik.touched.email && !!formik.errors.email}
                        errorMessage={formik.touched.email && formik.errors.email}
                        {...formik.getFieldProps("email")}
                    />
                    <Input
                        label="First Name"
                        isRequired
                        isInvalid={formik.touched.firstName && !!formik.errors.firstName}
                        errorMessage={formik.touched.firstName && formik.errors.firstName}
                        {...formik.getFieldProps("firstName")}
                    />
                    <Input
                        label="Last Name"
                        isRequired
                        isInvalid={formik.touched.lastName && !!formik.errors.lastName}
                        errorMessage={formik.touched.lastName && formik.errors.lastName}
                        {...formik.getFieldProps("lastName")}
                    />

                    {user?.email !== formik.values.email && (
                        <>
                            <Input
                                type="password"
                                label="Current Password"
                                isRequired
                                isInvalid={formik.touched.password && !!formik.errors.password}
                                errorMessage={formik.touched.password && formik.errors.password}
                                {...formik.getFieldProps("password")}
                            />
                            <div className="text-default-500 text-small">
                                Password is required to update your email.
                                <br />
                                If you don&apos;t have a password, create one <Link href="/settings/security" className="text-primary">here</Link>.
                            </div>
                        </>
                    )}
                </CardBody>

                <CardFooter className="my-4 justify-end gap-2">
                    <Button variant="light">
                        Cancel
                    </Button>
                    <Button color="primary" onClick={formik.submitForm}>
                        Save Changes
                    </Button>
                </CardFooter>
            </Card>

            <ProfilePictureModal
                isOpen={isOpen}
                onOpenChange={onOpenChange}
                user={user}
            />
        </>
    );
}
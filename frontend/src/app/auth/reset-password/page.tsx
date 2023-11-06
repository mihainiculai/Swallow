"use client";

import React, { useState, useEffect } from "react";
import Link from "next/link";
import { useSearchParams } from 'next/navigation'
import { Button, Input, Spinner } from "@nextui-org/react";
import { axiosInstance } from "@/components/axiosInstance";
import { useFormik } from "formik";
import { FaEye, FaEyeSlash } from "react-icons/fa";
import { resetPasswordSchema } from "../_components/validationSchemas";

export default function ForgotPasswordPage() {
    const searchParams = useSearchParams()
    const email = searchParams.get("email")
    const token = searchParams.get("token")

    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [isVisible, setIsVisible] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);
    const [tokenValid, setTokenValid] = useState<boolean>(false);

    useEffect(() => {
        const verifyToken = async () => {
            if (!email || !token) return;

            const decodedToken = decodeURIComponent(token);
            try {
                await axiosInstance.post("auth/verify-reset-token", { email, token: decodedToken });
                setTokenValid(true);
            } catch (error) {
                setTokenValid(false);
            }

            setIsLoading(false);
        };

        verifyToken();
    }, [email, token]);

    const formik = useFormik({
        initialValues: {
            password: "",
            confirmPassword: "",
        },
        validationSchema: resetPasswordSchema,
        onSubmit: async (values) => {
            if (!email || !token) return;

            try {
                const decodedToken = decodeURIComponent(token);
                await axiosInstance.post("auth/reset-password", { email, token: decodedToken, password: values.password });
                setSuccess("Your password has been reset successfully.");
                setError(null);
            } catch (error) {
                setError("There was a problem resetting your password. Please try again.");
                setSuccess(null);
            }
        }
    });

    if (isLoading) return <Spinner />

    return (
        <>
            <div>
                <h1 className="text-2xl font-bold">Reset Password</h1>
                {!tokenValid && <p className="mt-2">Your reset password link is invalid or has expired.</p>}
                {tokenValid && <p className="mt-2">Enter your new password below.</p>}
            </div>

            {!tokenValid &&
                <div className="space-y-6">
                    <Button as={Link} href="/auth/forgot-password" color="primary" fullWidth>
                        Reset Password
                    </Button>
                    <Button as={Link} href="/auth/login" color="primary" variant="flat" fullWidth>
                        Back to Login
                    </Button>
                </div>
            }
            {tokenValid &&
                <form onSubmit={formik.handleSubmit} className="space-y-6">
                    <Input
                        type={isVisible ? "text" : "password"}
                        placeholder="Password"
                        errorMessage={formik.touched.password && formik.errors.password}
                        isInvalid={formik.touched.password && !!formik.errors.password}
                        {...formik.getFieldProps("password")}
                        endContent={
                            <button className="focus:outline-none" type="button" onClick={() => setIsVisible(!isVisible)}>
                                {isVisible ? <FaEyeSlash className="text-2xl text-default-400 pointer-events-none" /> : <FaEye className="text-2xl text-default-400 pointer-events-none" />}
                            </button>
                        }
                    />
                    <Input
                        type="password"
                        placeholder="Confirm Password"
                        errorMessage={formik.touched.confirmPassword && formik.errors.confirmPassword}
                        isInvalid={formik.touched.confirmPassword && !!formik.errors.confirmPassword}
                        {...formik.getFieldProps("confirmPassword")}
                    />

                    <div>
                        {error && <span className="text-sm text-danger">{error}</span>}
                        {success && <span className="text-sm text-success">{success}</span>}
                    </div>

                    <div className="space-y-6">
                        {!success &&
                            <Button color="primary" type="submit" fullWidth>
                                Reset Password
                            </Button>
                        }

                        {success &&
                            <Button as={Link} href="/auth/login" color="primary" fullWidth>
                                Back to Login
                            </Button>
                        }
                    </div>
                </form>
            }
        </>
    );
}

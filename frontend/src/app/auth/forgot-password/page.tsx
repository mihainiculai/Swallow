"use client";

import React, { useState } from "react";
import Link from "next/link";
import { Button, Input } from "@nextui-org/react";
import { axiosInstance } from "@/components/axiosInstance";
import { useFormik } from "formik";
import { forgotPasswordSchema } from "../_components/validationSchemas";

export default function ForgotPasswordPage() {
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);

    const formik = useFormik({
        initialValues: {
            email: "",
        },
        validationSchema: forgotPasswordSchema,
        onSubmit: async (values) => {
            try {
                await axiosInstance.post("auth/forgot-password", values.email);
                setSuccess("If you have an account with us, you will receive an email with a link to reset your password shortly.");
                setError(null);
            } catch (error) {
                setError("There was a problem resetting your password.");
                setSuccess(null);
            }
        }
    });

    return (
        <>
            <div>
                <h1 className="text-2xl font-bold">Forgot Password</h1>
                <p className="mt-2">Enter your email address below and we&apos;ll send you a link to reset your password.</p>
            </div>

            <form onSubmit={formik.handleSubmit} className="space-y-6">
                <Input
                    isClearable
                    onClear={() => {
                        formik.setFieldValue("email", "");
                        formik.setFieldTouched("email", false);
                    }}
                    type="email"
                    placeholder="Email"
                    errorMessage={formik.touched.email && formik.errors.email}
                    isInvalid={formik.touched.email && !!formik.errors.email}
                    {...formik.getFieldProps("email")}
                />

                <div>
                    {error && <span className="text-sm text-danger">{error}</span>}
                    {success && <span className="text-sm text-success">{success}</span>}
                </div>

                <div className="space-y-6">
                    {!success &&
                        <Button color="primary" type="submit" fullWidth>
                            Send Reset Link
                        </Button>
                    }
                    <Button as={Link} href="/auth/login" color="primary" variant="flat" fullWidth>
                        Back to Login
                    </Button>
                </div>
            </form>
        </>
    );
}

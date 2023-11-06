"use client";

import React, { useState } from "react"
import { useAuthContext } from "@/contexts/auth-context";
import { useRouter } from 'next/navigation'
import Link from "next/link";
import { Button, Input, Checkbox } from "@nextui-org/react";
import { useFormik } from "formik";
import { FaEye, FaEyeSlash } from "react-icons/fa";
import { registrationSchema } from "../_components/validationSchemas";
import { AuthContextType } from "@/contexts/auth-context";

export default function RegisterPage() {
    const { signUp } = useAuthContext() as AuthContextType
    const router = useRouter()

    const [isVisible, setIsVisible] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null)

    const formik = useFormik({
        initialValues: {
            email: "",
            password: "",
            confirmPassword: "",
            firstName: "",
            lastName: "",
            terms: false,
        },
        validationSchema: registrationSchema,
        onSubmit: async (values) => {
            try {
                await signUp(values.email, values.password, values.firstName, values.lastName)
                router.push("/dashboard");
            } catch (error: any) {
                if (error.response && error.response.status === 409) {
                    formik.setErrors({
                        email: "Email already exists",
                    })
                    return
                }
                setError("An error occurred during registration.")
            }
        }
    });

    return (
        <>
            <div>
                <h1 className="text-2xl font-bold">Create an account</h1>
            </div>

            <form onSubmit={formik.handleSubmit} className="space-y-6">
                <Input
                    type="email"
                    placeholder="Email"
                    errorMessage={formik.touched.email && formik.errors.email}
                    isInvalid={formik.touched.email && !!formik.errors.email}
                    {...formik.getFieldProps("email")}
                />
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
                <Input
                    type="text"
                    placeholder="First Name"
                    errorMessage={formik.touched.firstName && formik.errors.firstName}
                    isInvalid={formik.touched.firstName && !!formik.errors.firstName}
                    {...formik.getFieldProps("firstName")}
                />
                <Input
                    type="text"
                    placeholder="Last Name"
                    errorMessage={formik.touched.lastName && formik.errors.lastName}
                    isInvalid={formik.touched.lastName && !!formik.errors.lastName}
                    {...formik.getFieldProps("lastName")}
                />
                <Checkbox
                    checked={formik.values.terms}
                    onChange={formik.handleChange}
                    name="terms"
                    color="primary"
                >
                    I accept the <Link href="/terms-and-conditions" className="hover:text-primary underline cursor-pointer">terms and conditions</Link>
                    {formik.touched.terms && formik.errors.terms && (
                    <div className="text-sm text-danger">{formik.errors.terms}</div>
                )}
                </Checkbox>
                

                {error && <div className="text-sm text-danger">{error}</div>}

                <div className="space-y-6">
                    <Button color="primary" type="submit" fullWidth>
                        Register
                    </Button>
                    <Button as={Link} href="/auth/login" color="primary" variant="flat" fullWidth>
                        Already have an account? Sign in
                    </Button>
                </div>
            </form>
        </>
    )
}

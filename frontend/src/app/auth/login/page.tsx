"use client";

import React, { useState } from "react"
import { useAuthContext } from "@/contexts/auth-context";
import { useRouter } from 'next/navigation'
import Link from "next/link";
import { Button, Input } from "@nextui-org/react";
import { useGoogleLogin } from '@react-oauth/google';
import DividerWithText from "@/components/DividerWithText";
import { useFormik } from "formik";
import { FaEye, FaEyeSlash } from "react-icons/fa6";
import { FcGoogle } from "react-icons/fc";
import { loginSchema } from "../_components/validationSchemas";
import { AuthContextType } from "@/contexts/auth-context";

export default function LoginPage() {
    const { signIn, signInWithGoogle } = useAuthContext() as AuthContextType
    const router = useRouter()

    const [isVisible, setIsVisible] = useState(false)
    const [error, setError] = useState<string | null>(null)

    const credentials = useFormik({
        initialValues: {
            email: "",
            password: "",
        },
        validationSchema: loginSchema,
        onSubmit: async (values) => {
            try {
                await signIn(values.email, values.password)
                router.push("/dashboard")
            } catch (error) {
                setError("Incorrect email or password")
            }
        }
    })

    const googleLogin = useGoogleLogin({
        onSuccess: async tokenResponse => {
            try {
                await signInWithGoogle(tokenResponse.access_token)
                router.push("/dashboard")
            }
            catch (error) {
                setError("Error signing in with Google")
            }
        },
    });

    return (
        <>
            <div>
                <h1 className="text-2xl font-bold"> Sign in to your account </h1>
            </div>

            <div>
                <Button
                    onClick={() => googleLogin()}
                    size="lg"
                    variant="bordered"
                    fullWidth
                    className="bg-white text-black"
                    startContent={<FcGoogle className="text-xl" />}
                >
                    Continue with Google
                </Button>
            </div>

            <DividerWithText text="OR" />

            <form onSubmit={credentials.handleSubmit} className="space-y-6">
                <Input
                    type="email"
                    placeholder="Email"
                    errorMessage={credentials.touched.email && credentials.errors.email}
                    isInvalid={credentials.touched.email && !!credentials.errors.email}
                    {...credentials.getFieldProps("email")}
                />
                <Input
                    type={isVisible ? "text" : "password"}
                    placeholder="Password"
                    errorMessage={credentials.touched.password && credentials.errors.password}
                    isInvalid={credentials.touched.password && !!credentials.errors.password}
                    {...credentials.getFieldProps("password")}
                    endContent={
                        <button className="focus:outline-none" type="button" onClick={() => setIsVisible(!isVisible)}>
                            {isVisible ? (
                                <FaEyeSlash className="text-2xl text-default-400 pointer-events-none" />
                            ) : (
                                <FaEye className="text-2xl text-default-400 pointer-events-none" />
                            )}
                        </button>
                    }
                />

                {error && <span className="text-sm text-danger">{error}</span>}

                <div className="flex justify-end">
                    <Link
                        href="/auth/forgot-password"
                        className="hover:text-primary hover:underline cursor-pointer mt-2"
                    >
                        Forgot your password?
                    </Link>
                </div>

                <div className="space-y-6">
                    <Button color="primary" type="submit" fullWidth>
                        Sign in
                    </Button>
                    <Button as={Link} href="/auth/register" color="primary" variant="flat" fullWidth>
                        Create an account
                    </Button>
                </div>
            </form>
        </>
    )
}
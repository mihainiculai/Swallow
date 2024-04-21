"use client";

import React, { useState, useEffect, Suspense } from "react";

import Link from "next/link";
import { useSearchParams } from 'next/navigation'

import { Button, Spinner } from "@nextui-org/react";

import { axiosInstance } from "@/components/utilities/axiosInstance";
import { mutate } from "swr";

export default function DeleteAccountPage() {
    const searchParams = useSearchParams()
    const email = searchParams.get("email")
    const token = searchParams.get("token")

    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false)
    const [success, setSuccess] = useState<string | null>(null);
    const [tokenValid, setTokenValid] = useState<boolean>(false);

    useEffect(() => {
        const verifyToken = async () => {
            if (!token || !email) return;

            try {
                const decodedToken = decodeURIComponent(token);
                await axiosInstance.post("users/verify-delete-account", { email: email, token: decodedToken });

                setTokenValid(true);
            } catch (error) {
                setTokenValid(false);
            }

            setIsLoading(false);
        };

        verifyToken();
    }, [email, token]);

    const handleDeleteAccount = async () => {
        setIsSubmitting(true);

        try {
            await axiosInstance.post("users/delete-account", { email: email, token: token });
            setSuccess("Your account has been deleted.");
            setError(null);
        } catch (error) {
            setError("There was a problem deleting your account.");
            setSuccess(null);
        } finally {
            await mutate("auth/me", null);
        }

        setIsSubmitting(false);
    }

    if (isLoading) return <Spinner />;

    return (
        <Suspense>
            <div>
                <h1 className="text-2xl font-bold">Delete Account</h1>
                {!tokenValid && <p className="mt-2">Your delete account link is invalid or has expired.</p>}
            </div>

            {!tokenValid || success &&
                <div className="space-y-6">
                    {success && <div className="text-sm text-success">{success}</div>}

                    <Button as={Link} href="/auth/login" color="primary" variant="flat" fullWidth>
                        Back to Website
                    </Button>
                </div>
            }
            {tokenValid &&
                <>
                    {!success && (
                        <>
                            <p>Please note that deleting your account is irreversible.</p>
                            <ul className="list-disc list-inside">
                                <li>All your data will be permanently lost</li>
                                <li>Any active subscriptions will be terminated without a refund</li>
                                <li>You will lose access to all services and features associated with your account</li>
                                <li>This action cannot be undone</li>
                            </ul>
                            <p><b>Are you sure you want to delete your account?</b></p>
                            {error && <div className="text-danger">{error}</div>}
                            <Button
                                color="danger"
                                onClick={() => handleDeleteAccount()}
                                isLoading={isSubmitting}
                                fullWidth
                            >
                                Delete Account
                            </Button>
                        </>
                    )}
                </>
            }
        </Suspense>
    );
}

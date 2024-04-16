"use client";

import { useState, useEffect } from "react";
import {
    Button,
    Card,
    CardBody,
    CardHeader,
    CardFooter,
    Chip,
    Link,
    Divider,
    Progress,
    Spacer,
    Skeleton
} from "@nextui-org/react";

import { format } from 'date-fns';
import { CellWrapper } from "@/components/cell-wrapper";

import useSWR from "swr";
import { fetcher } from "@/components/utilities/fetcher";
import { axiosInstance } from "@/components/utilities/axiosInstance";
import { getPremiumPrice, tiers } from "@/config/plans";

import { FaCheck, FaTimes } from "react-icons/fa";

interface Plan {
    planName: string;
    startDate: Date;
    endDate: Date;
    remainingTrips: number;
    totalTrips: number;
    hasClientPortalAccess: boolean;
}

export default function MembershipSettingsPage() {
    const { data: plan, isLoading } = useSWR<Plan>("subscriptions/current-subscription", fetcher);

    const [premiumPrice, setPremiumPrice] = useState<number | undefined>(undefined);
    useEffect(() => {
        const fetchPremiumPrice = async () => {
            const price = await getPremiumPrice();
            setPremiumPrice(price);
        };

        fetchPremiumPrice();
    }, []);

    const handleUpgradePlan = () => {
        axiosInstance.post("subscriptions/create-checkout-session")
            .then((response) => {
                window.location.href = response.data;
            })
            .catch((error) => {
                console.error(error);
            });
    }

    const handleClientPortalAccess = () => {
        axiosInstance.post("subscriptions/customer-portal")
            .then((response) => {
                window.location.href = response.data;
            })
            .catch((error) => {
                console.error(error);
            });
    }

    return (
        <div className="flex flex-col gap-6 items-center">
            <Card className="w-full p-4 flex" >
                <CardHeader className="flex flex-col items-start px-4 pb-0 pt-4">
                    <p className="text-large">Membership</p>
                    <p className="text-small text-default-500">Manage your current membership plan.</p>
                </CardHeader>
                <CardBody className="flex flex-col gap-4">
                    <CellWrapper>
                        <div>
                            <p>Current plan</p>
                        </div>
                        <div>
                            {isLoading ? (
                                <Skeleton className="rounded-lg h-6 w-24 bg-default-300" />
                            ) : (
                                <Chip color="primary" variant="flat">
                                    {plan?.planName}
                                </Chip>
                            )}
                        </div>
                    </CellWrapper>
                    <CellWrapper>
                        <div>
                            <p>Start Date</p>
                        </div>
                        <div>
                            {isLoading ? (
                                <Skeleton className="rounded-lg h-6 w-24 bg-default-300" />
                            ) : (
                                <p>{plan?.startDate ? format(plan.startDate, 'dd/MM/yyyy') : 'N/A'}</p>
                            )}
                        </div>
                    </CellWrapper>
                    <CellWrapper>
                        <div>
                            <p>End Date</p>
                        </div>
                        <div>
                            {isLoading ? (
                                <Skeleton className="rounded-lg h-6 w-24 bg-default-300" />
                            ) : (
                                <p>{plan?.endDate ? format(plan.endDate, 'dd/MM/yyyy') : 'N/A'}</p>
                            )}
                        </div>
                    </CellWrapper>
                    <CellWrapper>
                        <div>
                            <p>Remaining Trips</p>
                        </div>
                        <div className="flex flex-col gap-2 items-end">
                            {isLoading ? (
                                <Skeleton className="rounded-lg h-6 w-24 bg-default-300" />
                            ) : (
                                <p>{plan?.remainingTrips} / {plan?.totalTrips}</p>
                            )}
                            <Progress
                                size="sm"
                                aria-label="Remaining trips"
                                className="w-36 md:w-72"
                                value={(plan?.remainingTrips ?? 0) / (plan?.totalTrips ?? 1) * 100}
                            />
                        </div>
                    </CellWrapper>
                    {plan?.hasClientPortalAccess && (
                        <CellWrapper>
                            <div>
                                <p>Client Portal</p>
                                <p className="text-small text-default-500">
                                    View and manage your plan, invoices, and more.
                                </p>
                            </div>
                            <div>
                                <Button
                                    color="primary"
                                    radius="full"
                                    variant="flat"
                                    onClick={handleClientPortalAccess}
                                >
                                    Acces Portal
                                </Button>
                            </div>
                        </CellWrapper>
                    )}
                </CardBody>
            </Card>
            <div className="flex max-w-5xl flex-col items-center py-12">
                <div className="flex max-w-xl flex-col text-center">
                    <h2 className="font-medium text-primary">Plans</h2>
                    <h1 className="text-4xl font-medium tracking-tight">Get unlimited access</h1>
                    <Spacer y={4} />
                    <h2 className="text-large text-default-500">
                        Discover the ideal plan, tailored to your needs
                    </h2>
                </div>
                <Spacer y={8} />
                <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
                    <Card className="relative p-3" shadow="md">
                        <CardHeader className="flex flex-col items-start gap-2 pb-6">
                            <h2 className="text-large font-medium">{tiers[0]?.key}</h2>
                            <p className="text-medium text-default-500">{tiers[0]?.description}</p>
                        </CardHeader>
                        <Divider />
                        <CardBody className="gap-8">
                            <p className="flex items-baseline gap-1 pt-2">
                                <span className="inline bg-gradient-to-br from-foreground to-foreground-600 bg-clip-text text-4xl font-semibold tracking-tight text-transparent">
                                    {tiers[0]?.price}
                                </span>
                            </p>
                            <ul className="flex flex-col gap-2">
                                {tiers[0]?.features?.map((feature, index) => (
                                    <li key={index} className="flex items-center gap-2">
                                        {feature.included ? (
                                            <FaCheck className="text-primary" />
                                        ) : (
                                            <FaTimes className="text-danger" />
                                        )}
                                        <p className="text-default-500">{feature.title}</p>
                                    </li>
                                ))}
                            </ul>
                        </CardBody>
                        <CardFooter>
                            {plan?.planName === "Free" && (
                                <Button
                                    fullWidth
                                    as={Link}
                                    variant="light"
                                    isDisabled
                                >
                                    Current Plan
                                </Button>
                            )}
                        </CardFooter>
                    </Card>
                    <Card className="relative p-3" shadow="md">
                        <Chip
                            classNames={{
                                base: "absolute top-4 right-4",
                            }}
                            color="primary"
                            variant="flat"
                        >
                            Recommended
                        </Chip>
                        <CardHeader className="flex flex-col items-start gap-2 pb-6">
                            <h2 className="text-large font-medium">{tiers[1]?.key}</h2>
                            <p className="text-medium text-default-500">{tiers[1]?.description}</p>
                        </CardHeader>
                        <Divider />
                        <CardBody className="gap-8">
                            <div className="flex items-baseline gap-1 pt-2">
                                <span className="inline bg-gradient-to-br from-foreground to-foreground-600 bg-clip-text text-4xl font-semibold tracking-tight text-transparent">
                                    {premiumPrice ? (
                                        "$" + premiumPrice
                                    ) : (
                                        <Skeleton className="rounded-lg h-8 w-20 bg-default-300" />
                                    )}
                                </span>
                                <span className="text-small font-medium text-default-400">
                                    {tiers[1]?.priceSuffix}
                                </span>
                            </div>
                            <ul className="flex flex-col gap-2">
                                {tiers[1]?.features?.map((feature, index) => (
                                    <li key={index} className="flex items-center gap-2">
                                        {feature.included ? (
                                            <FaCheck className="text-primary" />
                                        ) : (
                                            <FaTimes className="text-danger" />
                                        )}
                                        <p className="text-default-500">{feature.title}</p>
                                    </li>
                                ))}
                            </ul>
                        </CardBody>
                        <CardFooter>
                            <Button
                                fullWidth
                                as={Link}
                                variant={plan?.planName === "Premium" ? "light" : "solid"}
                                color={plan?.planName === "Premium" ? "default" : "primary"}
                                onClick={plan?.planName === "Premium" ? handleClientPortalAccess : handleUpgradePlan}
                            >
                                {plan?.planName === "Premium" ? "Manage Plan" : "Upgrade Plan"}
                            </Button>
                        </CardFooter>
                    </Card>
                    <Card className="relative p-3" shadow="md">
                        <CardHeader className="flex flex-col items-start gap-2 pb-6">
                            <h2 className="text-large font-medium">{tiers[2]?.key}</h2>
                            <p className="text-medium text-default-500">{tiers[2]?.description}</p>
                        </CardHeader>
                        <Divider />
                        <CardBody className="gap-8">
                            <p className="flex items-baseline gap-1 pt-2">
                                <span className="inline bg-gradient-to-br from-foreground to-foreground-600 bg-clip-text text-4xl font-semibold tracking-tight text-transparent">
                                    {tiers[2]?.price}
                                </span>
                            </p>
                            <ul className="flex flex-col gap-2">
                                {tiers[2]?.features?.map((feature, index) => (
                                    <li key={index} className="flex items-center gap-2">
                                        {feature.included ? (
                                            <FaCheck className="text-primary" />
                                        ) : (
                                            <FaTimes className="text-danger" />
                                        )}
                                        <p className="text-default-500">{feature.title}</p>
                                    </li>
                                ))}
                            </ul>
                        </CardBody>
                        <CardFooter>
                            <Button
                                fullWidth
                                as={Link}
                                isDisabled
                            >
                                Comming Soon
                            </Button>
                        </CardFooter>
                    </Card>
                </div>
                <Spacer y={12} />
                <div className="flex py-2">
                    <p className="text-default-400">
                        Do you want to see more details?&nbsp;
                        <Link color="foreground" href="/prices" underline="always">
                            View all features
                        </Link>
                    </p>
                </div>
            </div>
        </div >
    )
}
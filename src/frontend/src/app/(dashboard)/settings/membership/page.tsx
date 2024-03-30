"use client";

import {
    Button,
    Card,
    CardBody,
    CardHeader,
    CardFooter,
    Chip,
    Link,
    Divider,
} from "@nextui-org/react";

import { FaCheck, FaTimes } from "react-icons/fa";

export default function MembershipSettingsPage() {
    return (
        <div className="flex flex-col gap-6">
            <Card className="full-width p-4" >
                <CardHeader className="flex flex-col items-start px-4 pb-0 pt-4">
                    <p className="text-large">Membership</p>
                    <p className="text-small text-default-500">Manage your current membership plan.</p>
                </CardHeader>
                <CardBody className="flex flex-col gap-4">

                </CardBody>
            </Card>
            <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
                <Card className="relative p-3" shadow="md">
                    <CardHeader className="flex flex-col items-start gap-2 pb-6">
                        <h2 className="text-large font-medium">Free</h2>
                        <p className="text-medium text-default-500">For starters and hobbyists that want to try out.</p>
                    </CardHeader>
                    <Divider />
                    <CardBody className="gap-8">
                        <p className="flex items-baseline gap-1 pt-2">
                            <span className="inline bg-gradient-to-br from-foreground to-foreground-600 bg-clip-text text-4xl font-semibold tracking-tight text-transparent">
                                Free
                            </span>
                        </p>
                        <ul className="flex flex-col gap-2">
                            <li className="flex items-center gap-2">
                                <FaCheck className="text-primary" />
                                <p className="text-default-500">3 Trips per month</p>
                            </li>
                            <li className="flex items-center gap-2">
                                <FaTimes className="text-danger" />
                                <p className="text-default-500">All features</p>
                            </li>
                        </ul>
                    </CardBody>
                    <CardFooter>
                        <Button
                            fullWidth
                            as={Link}
                        >
                            Choose Plan
                        </Button>
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
                        <h2 className="text-large font-medium">Premium</h2>
                        <p className="text-medium text-default-500">For the avid traveler seeking full control</p>
                    </CardHeader>
                    <Divider />
                    <CardBody className="gap-8">
                        <p className="flex items-baseline gap-1 pt-2">
                            <span className="inline bg-gradient-to-br from-foreground to-foreground-600 bg-clip-text text-4xl font-semibold tracking-tight text-transparent">
                                $9.99
                            </span>
                            <span className="text-small font-medium text-default-400">
                                /per month
                            </span>
                        </p>
                        <ul className="flex flex-col gap-2">
                            <li className="flex items-center gap-2">
                                <FaCheck className="text-primary" />
                                <p className="text-default-500">3 Trips per month</p>
                            </li>
                            <li className="flex items-center gap-2">
                                <FaTimes className="text-danger" />
                                <p className="text-default-500">All features</p>
                            </li>
                        </ul>
                    </CardBody>
                    <CardFooter>
                        <Button
                            fullWidth
                            as={Link}
                        >
                            Choose Plan
                        </Button>
                    </CardFooter>
                </Card>
                <Card className="relative p-3" shadow="md">
                    <CardHeader className="flex flex-col items-start gap-2 pb-6">
                        <h2 className="text-large font-medium">Business</h2>
                        <p className="text-medium text-default-500">For trip organizers and small businesses.</p>
                    </CardHeader>
                    <Divider />
                    <CardBody className="gap-8">
                        <p className="flex items-baseline gap-1 pt-2">
                            <span className="inline bg-gradient-to-br from-foreground to-foreground-600 bg-clip-text text-4xl font-semibold tracking-tight text-transparent">
                                Contact us
                            </span>
                        </p>
                        <ul className="flex flex-col gap-2">
                            <li className="flex items-center gap-2">
                                <FaCheck className="text-primary" />
                                <p className="text-default-500">3 Trips per month</p>
                            </li>
                            <li className="flex items-center gap-2">
                                <FaTimes className="text-danger" />
                                <p className="text-default-500">All features</p>
                            </li>
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
        </div>
    )
}
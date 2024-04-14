"use client";

import React, { useEffect, useState } from "react";
import {
    Button,
    Card,
    CardBody,
    CardFooter,
    CardHeader,
    Chip,
    Divider,
    Link,
    Spacer,
    Tooltip,
    Skeleton
} from "@nextui-org/react";

import { tiers, features, getPremiumPrice } from '@/config/plans';
import { FaCheck, FaTimes } from "react-icons/fa";
import { IoIosInformationCircleOutline } from "react-icons/io";

export default function Prices() {
    const [premiumPrice, setPremiumPrice] = useState<number | undefined>(undefined);
    useEffect(() => {
        const fetchPremiumPrice = async () => {
            const price = await getPremiumPrice();
            setPremiumPrice(price);
        };

        fetchPremiumPrice();
    }, []);

    return (
        <div className="mx-auto flex max-w-5xl flex-col items-center py-24">
            <div className="flex max-w-xl flex-col text-center">
                <h2 className="font-medium leading-7 text-primary">Pricing</h2>
                <h1 className="text-4xl font-medium tracking-tight">Compare plans & features</h1>
                <Spacer y={4} />
                <h2 className="text-large text-default-500">
                    Discover the ideal plan, tailored to your needs
                </h2>
            </div>

            <Spacer y={12} />

            <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:hidden">
                <Card className="relative p-3 !border-medium border-default-100 bg-transparent" shadow="md">
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
                        <Button
                            fullWidth
                            as={Link}
                            href="/dashboard"
                            variant="flat"
                        >
                            Continue with Free
                        </Button>
                    </CardFooter>
                </Card>
                <Card className="relative p-3 border-2 border-primary shadow-2xl shadow-primary/20" shadow="md">
                    <Chip
                        classNames={{
                            base: "absolute top-4 right-4",
                        }}
                        color="primary"
                        variant="flat"
                    >
                        Most Popular
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
                            href="/dashboard"
                            color="primary"
                        >
                            Get Started
                        </Button>
                    </CardFooter>
                </Card>
                <Card className="relative p-3 border-content3 bg-content2 dark:border-content2 dark:bg-content1" shadow="md">
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
                            isDisabled
                            variant="flat"
                        >
                            Comming Soon
                        </Button>
                    </CardFooter>
                </Card>
                <Spacer y={4} />
            </div>

            <div className="p-6 lg:hidden text-center text-default-500">
                A comparison table and all the features of each plan are available on desktop.
            </div>

            <div className="isolate hidden lg:block">
                <div className="relative -mx-8">
                    <table className="w-full table-fixed border-separate border-spacing-x-4 text-left">
                        <caption className="sr-only">Pricing plan comparison</caption>
                        <colgroup>
                            {Array.from({ length: tiers.length + 1 }).map((_, index) => (
                                <col key={index} className="w-1/4" />
                            ))}
                        </colgroup>
                        <thead>
                            <tr>
                                <td />
                                <th scope="col" className="relative px-6 pt-6 xl:px-8 xl:pt-8">
                                    <div className="text-large font-medium text-foreground">{tiers[0].title}</div>
                                </th>
                                <th scope="col" className="bg-primary relative px-6 pt-6 xl:px-8 xl:pt-8 rounded-t-medium">
                                    <Chip
                                        classNames={{
                                            base: "absolute top-2 right-2 bg-primary-foreground border-medium border-primary",
                                            content: "font-medium text-primary",
                                        }}
                                        color="primary"
                                    >
                                        Most Popular
                                    </Chip>
                                    <div className="text-large font-medium text-primary-foreground">{tiers[1].title}</div>
                                </th>
                                <th scope="col" className="bg-content2 dark:bg-content1 relative px-6 pt-6 xl:px-8 xl:pt-8 rounded-t-medium">
                                    <div className="text-large font-medium text-foreground">{tiers[2].title}</div>
                                </th>
                            </tr>
                            <tr>
                                <th scope="row">
                                    <span className="sr-only">Price</span>
                                </th>
                                <td className="relative px-6 pt-4 xl:px-8">
                                    <div className="flex items-baseline gap-1 text-foreground">
                                        <span className="inline bg-gradient-to-br from-foreground to-foreground-600 bg-clip-text text-4xl font-semibold leading-8 tracking-tight text-transparent">
                                            {tiers[0].price}
                                        </span>
                                        <span className="!text-small font-medium text-default-600">
                                            {tiers[0].priceSuffix}
                                        </span>
                                    </div>
                                    <Button fullWidth className='mt-6' variant='flat' as={Link} href='/dashboard'>
                                        Continue with Free
                                    </Button>
                                </td>
                                <td className="bg-primary relative px-6 pt-4 xl:px-8">
                                    <div className="flex items-baseline gap-1 text-foreground">
                                        <span className="inline bg-gradient-to-br from-foreground to-foreground-600 bg-clip-text text-4xl font-semibold leading-8 tracking-tight text-primary-foreground">
                                            {premiumPrice ? (
                                                "$" + premiumPrice
                                            ) : (
                                                <Skeleton className="rounded-lg h-8 w-20 bg-primary dark:bg-primary" />
                                            )}
                                        </span>
                                    </div>
                                    <Button fullWidth className='mt-6 bg-primary-foreground font-medium text-primary shadow-sm shadow-default-500/50' as={Link} href='/dashboard'>
                                        Get Started
                                    </Button>
                                </td>
                                <td className="bg-content2 dark:bg-content1 relative px-6 pt-4 xl:px-8">
                                    <div className="flex items-baseline gap-1 text-foreground">
                                        <span className="inline bg-gradient-to-br from-foreground to-foreground-600 bg-clip-text text-4xl font-semibold leading-8 tracking-tight text-transparent">
                                            {tiers[2].price}
                                        </span>
                                    </div>
                                    <Button fullWidth className='mt-6' variant='flat' isDisabled>
                                        Comming Soon
                                    </Button>
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                            {features.map((feat, featIndex) => (
                                <React.Fragment key={feat.title}>
                                    <tr>
                                        <th
                                            className={`pb-4 pt-12 text-large font-semibold text-foreground ${featIndex === 0 ? "pt-16" : ""}`}
                                            colSpan={1}
                                            scope="colgroup"
                                        >
                                            {feat.title}
                                            <Divider className="absolute -inset-x-4 mt-2 bg-default-600/10" />
                                        </th>
                                        {tiers.map((tier) => (
                                            <td
                                                key={tier.key}
                                                className={`relative py-4 ${tier.key === "Premium" ? "bg-primary" : ""} ${tier.key === "Business" ? "bg-content2 dark:bg-content1" : ""}`}
                                            />
                                        ))}
                                    </tr>
                                    {feat.items.map((tierFeature, tierFeatureIndex) => (
                                        <tr key={tierFeature.title}>
                                            <th className="py-4 text-medium font-normal text-default-700" scope="row">
                                                {tierFeature.helpText ? (
                                                    <div className="flex items-center gap-1">
                                                        <span>{tierFeature.title}</span>
                                                        <Tooltip
                                                            className="max-w-[240px]"
                                                            color="foreground"
                                                            content={tierFeature.helpText}
                                                            placement="right"
                                                        >
                                                            <IoIosInformationCircleOutline className="text-default-600" />
                                                        </Tooltip>
                                                    </div>
                                                ) : (
                                                    tierFeature.title
                                                )}
                                            </th>

                                            {tiers.map((tier) => {
                                                const isLastOne =
                                                    tierFeatureIndex === feat.items.length - 1 &&
                                                    featIndex === features.length - 1;

                                                return (
                                                    <td
                                                        key={tier.key}
                                                        className={`relative px-6 py-4 xl:px-8 ${tier.key === "Premium" ? "bg-primary" : ""} ${tier.key === "Business" ? "bg-content2 dark:bg-content1" : ""} ${isLastOne ? "rounded-b-medium" : ""}`}
                                                    >
                                                        {typeof tierFeature.tiers[tier.key] === "string" ? (
                                                            <div
                                                                className={`text-center text-medium text-default-500 ${tier.key === "Premium" ? "text-primary-foreground/70" : ""}`}
                                                            >
                                                                {tierFeature.tiers[tier.key]}
                                                            </div>
                                                        ) : (
                                                            <>
                                                                {tierFeature.tiers[tier.key] === true ? (
                                                                    <FaCheck className={`mx-auto text-primary ${tier.key === "Premium" ? "text-primary-foreground" : ""}`} />
                                                                ) : (
                                                                    <FaTimes className={`mx-auto text-default-400 ${tier.key === "Premium" ? "text-primary-foreground/50" : ""}`} />

                                                                )}

                                                                <span className="sr-only">
                                                                    {tierFeature.tiers[tier.key] === true
                                                                        ? "Included"
                                                                        : "Not included"}
                                                                    &nbsp;in&nbsp;{tier.title}
                                                                </span>
                                                            </>
                                                        )}
                                                    </td>
                                                );
                                            })}
                                        </tr>
                                    ))}
                                </React.Fragment>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    )
}
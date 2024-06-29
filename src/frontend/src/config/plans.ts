import { axiosInstance } from "@/components/utilities/axiosInstance";

export enum TiersEnum {
    Free = "Free",
    Premium = "Premium",
    Business = "Business",
}

type Feautre = {
    included: boolean;
    title: string;
}

export type Tier = {
    key: TiersEnum;
    title: string;
    price?: number | string;
    priceSuffix?: string;
    description: string;
    recommended?: boolean;
    features?: Feautre[];
};

export async function getPremiumPrice(): Promise<number> {
    const response = await axiosInstance.get("subscriptions/price");
    return response.data / 100
}

export const tiers: Tier[] = [
    {
        key: TiersEnum.Free,
        title: "Free",
        price: "Free",
        description: "For the casual traveler",
        features: [
            { included: true, title: "3 trips per month" },
            { included: true, title: "Up to 3 days trip duration"},
            { included: true, title: "Manual itinerary creation" },
            { included: false, title: "Automated itinerary creation" },
            { included: false, title: "Mobile app access" },
            { included: false, title: "Pre-trip email recommendations" },
            { included: false, title: "Advanced booking options" },
        ],
    },
    {
        key: TiersEnum.Premium,
        title: "Premium",
        priceSuffix: "per month",
        description: "For the avid traveler seeking full control",
        recommended: true,
        features: [
            { included: true, title: "10 trips per month" },
            { included: true, title: "Up to 5 days trip duration" },
            { included: true, title: "Manual itinerary creation" },
            { included: true, title: "Automated itinerary creation" },
            { included: true, title: "Mobile app access" },
            { included: true, title: "Pre-trip email recommendations" },
            { included: true, title: "Advanced booking options" },
        ],
    },
    {
        key: TiersEnum.Business,
        title: "Business",
        price: "Contact us",
        description: "For trip organizers and small businesses",
        features: [
            { included: true, title: "Unlimited trips per month" },
            { included: true, title: "Up to 10 days trip duration" },
            { included: true, title: "Manual itinerary creation" },
            { included: true, title: "Popular itinerary creation" },
            { included: true, title: "Advanced booking options" },
            { included: true, title: "Custom branding on itineraries" },
            { included: true, title: "API access for integration" },
        ],
    },
];

type PricingFeatureItem = {
    title: string;
    tiers: {
        [key in TiersEnum]: boolean | string;
    };
    helpText?: string;
};

type PricingFeatures = Array<{
    title: string;
    items: PricingFeatureItem[];
}>;

export const features: PricingFeatures = [
    {
        title: "Itinerary",
        items: [
            {
                title: "Number of trips",
                tiers: {
                    [TiersEnum.Free]: "3 per month",
                    [TiersEnum.Premium]: "10 per month",
                    [TiersEnum.Business]: "Unlimited",
                },
                helpText: "The number of trips you can create per month.",
            },
            {
                title: "Trip duration",
                tiers: {
                    [TiersEnum.Free]: "Up to 3 days",
                    [TiersEnum.Premium]: "Up to 5 days",
                    [TiersEnum.Business]: "Up to 10 days",
                },
            },
            {
                title: "Manual itinerary creation",
                tiers: {
                    [TiersEnum.Free]: true,
                    [TiersEnum.Premium]: true,
                    [TiersEnum.Business]: true,
                },
                helpText: "Create your own itinerary by adding activities and travel details.",
            },
            {
                title: "Automated itinerary creation",
                tiers: {
                    [TiersEnum.Free]: false,
                    [TiersEnum.Premium]: true,
                    [TiersEnum.Business]: true,
                },
                helpText: "Get a recommended itinerary based on your preferences.",
            },
        ],
    },
    {
        title: "Features",
        items: [
            {
                title: "Mobile app access",
                tiers: {
                    [TiersEnum.Free]: false,
                    [TiersEnum.Premium]: true,
                    [TiersEnum.Business]: true,
                },
                helpText: "Access your itineraries on the go with our mobile app.",
            },
            {
                title: "Pre-trip email recommendations",
                tiers: {
                    [TiersEnum.Free]: false,
                    [TiersEnum.Premium]: true,
                    [TiersEnum.Business]: true,
                },
                helpText:
                    "Get recommendations on things to do and places to visit before your trip.",
            },
            {
                title: "Advanced booking options",
                tiers: {
                    [TiersEnum.Free]: false,
                    [TiersEnum.Premium]: true,
                    [TiersEnum.Business]: true,
                },
                helpText: "Book activities and accommodations directly from your itinerary.",
            },
            {
                title: "Popular itinerary creation",
                tiers: {
                    [TiersEnum.Free]: false,
                    [TiersEnum.Premium]: false,
                    [TiersEnum.Business]: true,
                },
                helpText: "Create itineraries based on popular destinations and activities.",
            },
        ],
    },
    {
        title: "Business",
        items: [
            {
                title: "Custom branding on itineraries",
                tiers: {
                    [TiersEnum.Free]: false,
                    [TiersEnum.Premium]: false,
                    [TiersEnum.Business]: true,
                },
                helpText: "Add your company logo and branding to your itineraries.",
            },
            {
                title: "API access for integration",
                tiers: {
                    [TiersEnum.Free]: false,
                    [TiersEnum.Premium]: false,
                    [TiersEnum.Business]: true,
                },
                helpText:
                    "Integrate our platform with your existing systems to automate your workflow.",
            },
        ],
    },
    {
        title: "Support",
        items: [
            {
                title: "Help center",
                tiers: {
                    [TiersEnum.Free]: true,
                    [TiersEnum.Premium]: true,
                    [TiersEnum.Business]: true,
                },
                helpText:
                    "Browse our articles in our knowledge base to find answers to your questions regarding the platform.",
            },
            {
                title: "Customer support",
                tiers: {
                    [TiersEnum.Free]: false,
                    [TiersEnum.Premium]: true,
                    [TiersEnum.Business]: true,
                },
                helpText: "Get help from our team via email.",
            },
        ],
    },
];
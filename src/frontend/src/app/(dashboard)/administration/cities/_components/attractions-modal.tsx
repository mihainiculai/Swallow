"use client";

import React, { FC, useState, useEffect } from "react";
import dynamic from 'next/dynamic'

import {
    Modal,
    ModalContent,
    ModalHeader,
    ModalBody,
    ModalFooter,
    Button,
    Autocomplete,
    AutocompleteItem,
    Input,
    Pagination
} from "@nextui-org/react";

import useSWR, { mutate } from "swr";
import { fetcher } from "@/components/utilities/fetcher";
import { axiosInstance } from "@/components/utilities/axiosInstance";

const PlaceListItem = dynamic(() => import("./place-list-item"), { ssr: false });

import { CiSearch } from "react-icons/ci";
import { MdAttractions } from "react-icons/md";
import { MdAutoAwesome } from "react-icons/md";

interface AttractionsModalProps {
    isOpen: boolean;
    onOpenChange: () => void;
    cityId: number | undefined;
    cityName: string;
}

interface AttractionProps {
    attractionId: number;
    name: string;
    description: string;
    categories: number[];
    rating: number;
    popularity: number;
    price: number;
    pictureUrl: string;
    schedules: ScheduleProps[];
}

interface CategoryProps {
    attractionCategoryId: number;
    name: string;
}

interface ScheduleProps {
    weekdayId: number;
    weekdayName: string;
    openTime: string;
    closeTime: string;
}

export const AttractionsModal: FC<AttractionsModalProps> = ({ isOpen, onOpenChange, cityId, cityName }) => {
    const { data: attractions, isLoading } = useSWR<AttractionProps[]>(`attractions?cityId=${cityId}`, fetcher);
    const { data: categories } = useSWR<CategoryProps[]>('attractions/categories', fetcher);

    const [filteredAttractions, setFilteredAttractions] = useState<AttractionProps[]>([]);
    const [showdAttractions, setShowdAttractions] = useState<AttractionProps[]>([]);

    const getCategoryNames = (categoriesIds: number[]): string[] => {
        return categoriesIds.map((categoryId): string => {
            const category = categories?.find(category => category.attractionCategoryId === categoryId);
            return category ? category.name : "";
        }).filter(name => name !== "");
    };

    const [selectedCategory, setSelectedCategory] = useState<number | null | string>(null);
    const [searchQuery, setSearchQuery] = useState("");

    const [isSyncing, setIsSyncing] = useState(false);
    const [error, setError] = useState("");

    const syncData = () => {
        setIsSyncing(true);
        setError("");

        axiosInstance.post(`attractions/sync?cityId=${cityId}`, null, { timeout: 240000 })
            .then(() => {
                mutate(`attractions?cityId=${cityId}`);
            })
            .catch((error) => {
                setError(error.response?.data || "An error occurred while syncing data");
            })
            .finally(() => {
                setIsSyncing(false);
            });
    }

    useEffect(() => {
        setSelectedCategory(null);
        setSearchQuery("");
        setIsSyncing(false);
        setError("");
    }, [cityId, isOpen]);

    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);

    useEffect(() => {
        var filtered = attractions || [];

        filtered = filtered.filter(attraction => {
            const categoryMatch = !selectedCategory || attraction.categories.includes(Number(selectedCategory));
            const searchMatch = !searchQuery || attraction.name.toLowerCase().includes(searchQuery.toLowerCase());

            return categoryMatch && searchMatch;
        });

        setTotalPages(Math.ceil(filtered.length / 100));
        setFilteredAttractions(filtered);
    }, [selectedCategory, searchQuery, currentPage, attractions]);

    useEffect(() => {
        const start = (currentPage - 1) * 100;
        const end = start + 100;
        setShowdAttractions(filteredAttractions.slice(start, end));
    }, [currentPage, filteredAttractions]);

    useEffect(() => {
        setCurrentPage(1);
    }, [selectedCategory, searchQuery]);

    return (
        <Modal isOpen={isOpen} onOpenChange={onOpenChange} size="full">
            <ModalContent>
                {(onClose) => (
                    <>
                        <div className="flex flex-col h-full">
                            <ModalHeader className="flex flex-col gap-1">Tourist Attractions</ModalHeader>
                            <div className="mb-6 flex flex-wrap items-center justify-between gap-4 md:flex-nowrap px-8">
                                <div className="flex w-full items-center gap-2">
                                    <MdAttractions className="text-2xl text-primary" />
                                    <h1 className="text-medium font-semibold md:text-large">{cityName}</h1>
                                    <div className="flex items-center ml-2">
                                        <span className="text-right text-small text-default-500 lg:text-medium">
                                            {attractions?.length} attractions
                                        </span>
                                    </div>
                                </div>
                                <div className="flex w-full items-center justify-end gap-4">
                                    <Input
                                        fullWidth
                                        size="lg"
                                        aria-label="Search"
                                        className="w-72"
                                        placeholder="Search attractions"
                                        startContent={<CiSearch />}
                                        value={searchQuery}
                                        onValueChange={setSearchQuery}
                                    />
                                    <Autocomplete
                                        label="Category"
                                        className="w-60"
                                        size="sm"
                                        selectedKey={selectedCategory}
                                        onSelectionChange={(key) => setSelectedCategory(key)}
                                    >
                                        {(categories || []).map((category: CategoryProps) => (
                                            <AutocompleteItem key={category.attractionCategoryId} value={category.attractionCategoryId}>
                                                {category.name}
                                            </AutocompleteItem>
                                        ))}
                                    </Autocomplete>
                                </div>
                            </div>
                            <ModalBody className="flex flex-col flex-grow overflow-auto gap-4" style={{ scrollbarWidth: 'thin' }}>
                                {(isLoading || isSyncing) && (
                                    <div className="grid grid-cols-1 gap-5 p-4 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5">
                                        <PlaceListItem isLoading />
                                        <PlaceListItem isLoading />
                                        <PlaceListItem isLoading className="hidden sm:block" />
                                        <PlaceListItem isLoading className="hidden md:block" />
                                        <PlaceListItem isLoading className="hidden lg:block" />
                                        <PlaceListItem isLoading className="hidden lg:block" />
                                        <PlaceListItem isLoading className="hidden lg:block" />
                                    </div>
                                )}

                                {(!isSyncing && attractions) && (
                                    <>
                                        {attractions.length === 0 ? (
                                            <div className="flex items-center justify-center w-full h-96">
                                                <p className="text-2xl text-default-500">No attractions found</p>
                                            </div>
                                        ) : (
                                            <div className="grid grid-cols-1 gap-5 p-4 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5">
                                                {showdAttractions.map((attraction) => (
                                                    <PlaceListItem
                                                        key={attraction.attractionId}
                                                        attractionId={attraction.attractionId}
                                                        name={attraction.name}
                                                        priority={attraction.popularity}
                                                        price={attraction.price}
                                                        rating={attraction.rating}
                                                        description={attraction.description}
                                                        pictureUrl={attraction.pictureUrl}
                                                        categories={getCategoryNames(attraction.categories)}
                                                        schedules={attraction.schedules}
                                                    />
                                                ))}
                                            </div>
                                        )}
                                    </>
                                )}
                            </ModalBody>
                            <ModalFooter className="flex flex-col md:flex-row items-center">
                                <div className="w-full md:w-auto">
                                    <Pagination
                                        showControls
                                        total={totalPages}
                                        initialPage={1}
                                        page={currentPage}
                                        onChange={setCurrentPage}
                                    />
                                </div>
                                <div className="flex flex-grow items-center ml-2">
                                    {error && (
                                        <p className="text-danger-500 text-small">
                                            {error}
                                        </p>
                                    )}
                                </div>
                                <div>
                                    <Button color="danger" variant="light" onPress={onClose}>
                                        Close
                                    </Button>
                                    <Button
                                        color="primary"
                                        startContent={<MdAutoAwesome />}
                                        isLoading={isSyncing}
                                        onClick={syncData}
                                    >
                                        Auto Sync Data
                                    </Button>
                                </div>
                            </ModalFooter>
                        </div>
                    </>
                )}
            </ModalContent>
        </Modal >
    )
}
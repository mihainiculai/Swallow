"use client"

import React from "react";
import {
    Modal,
    ModalContent,
    ModalBody,
    Button,
    Input,
    Avatar,
    Link
} from "@nextui-org/react"

import { CountryFlagUrl } from "@/components/country-flag";

import useSWR from "swr";
import { fetcher } from "@/components/utilities/fetcher";
import { SlArrowRightCircle } from "react-icons/sl";

interface SearchModalProps {
    isOpen: boolean;
    onOpen: () => void;
    onOpenChange: () => void;
}

export const SearchModal: React.FC<SearchModalProps> = ({ isOpen, onOpen, onOpenChange }) => {
    const [query, setQuery] = React.useState<string>("")

    const getApiUrl = () => {
        if (query) {
            return `search?query=${query}`
        }
        return "search"
    }

    const { data } = useSWR(getApiUrl(), fetcher)

    return (
        <Modal
            isOpen={isOpen}
            onOpenChange={onOpenChange}
            size="xl"
            backdrop="blur"
            hideCloseButton
            classNames={{
                body: "py-6",
                base: "border border-default-100 rounded-large bg-content bg-opacity-100n"
            }}
        >
            <ModalContent>
                {(onClose) => (
                    <>
                        <ModalBody className="flex flex-col gap-4">
                            {data?.map((item: any) => (
                                <Button
                                    key={item.id}
                                    fullWidth
                                    as={Link}
                                    href={`/destination/${item.cityId}`}
                                    onClick={onOpenChange}
                                    className="p-4 bg-content2 h-auto flex items-center justify-between"
                                >
                                    <div className="flex gap-5">
                                        <Avatar radius="full" size="md" src={CountryFlagUrl(item.countryCode)} />
                                        <div className="flex flex-col gap-1 items-start justify-center">
                                            <h4 className="text-small font-semibold leading-none text-default-600">{item.name}</h4>
                                            <h5 className="text-small tracking-tight text-default-400">{item.countryName}</h5>
                                        </div>
                                    </div>
                                    <div>
                                        <SlArrowRightCircle className="text-xl text-default-500" />
                                    </div>
                                </Button>
                            ))}
                            {data?.length === 0 && (
                                <h3 className="text-center text-default-500 py-4">No results found</h3>
                            )}
                        </ModalBody>
                        <Input
                            placeholder="Search destination..."
                            size="lg"
                            value={query}
                            onValueChange={setQuery}
                            isClearable
                            onClear={() => setQuery("")}
                        />
                    </>
                )}
            </ModalContent>
        </Modal>
    )
}
"use client"

import { useRouter } from 'next/navigation'

import { Card, Button, Link, useDisclosure, Dropdown, DropdownTrigger, DropdownMenu, DropdownItem } from '@nextui-org/react';
import { GrAttachment } from 'react-icons/gr';
import { MdHotel, MdMoreHoriz } from 'react-icons/md';
import { LodgingModal } from './lodging-modal';
import { AttachmentsModal } from './attachments-modal';
import { axiosInstance } from '@/components/utilities/axiosInstance';

import { MdArchive, MdDelete } from "react-icons/md";

interface ManageCardProps {
    tripId: string;
    cityId: number;
    lodging: any;
    totalCost: number;
    onViewMoreClick: () => void;
}

export const ManageCard = ({ tripId, cityId, lodging, totalCost, onViewMoreClick }: ManageCardProps) => {
    const { isOpen: isLodgingOpen, onOpen: onLodgingOpen, onOpenChange: onLodgingOpenChange } = useDisclosure();
    const { isOpen: isAttachmentOpen, onOpen: onAttachmentOpen, onOpenChange: onAttachmentOpenChange } = useDisclosure();

    const router = useRouter()

    const handleArchive = () => {
        axiosInstance
            .post(`itineraries/${tripId}/archive`)
            .finally(() => router.push('/dashboard'));
    }

    const handleDelete = () => {
        axiosInstance.delete(`itineraries/${tripId}`)
            .finally(() => router.push('/dashboard'));
    }

    return (
        <Card className="w-full grid grid-cols-5 relative min-h-[8rem] h-[8rem] items-center p-4 gap-4">
            <div className="col-span-2 flex flex-col gap-2">
                <h3 className="font-bold text-large">Budget</h3>
                <p className="text-gray-500 text-2xl">${totalCost}</p>
                <Link size='sm' href="#" onClick={onViewMoreClick}>
                    View details
                </Link>
            </div>
            <div className="col-span-3 flex flex-row gap-4 justify-end">
                <Button isIconOnly size='lg' variant='light' className='w-20 h-20' onClick={onLodgingOpen}>
                    <div className="flex flex-col items-center gap-2">
                        <MdHotel fontSize={24} />
                        <p className="text-[0.65rem]">Lodging</p>
                        <LodgingModal tripId={tripId} isOpen={isLodgingOpen} onOpen={onLodgingOpen} onOpenChange={onLodgingOpenChange} cityId={cityId} lodging={lodging} />
                    </div>
                </Button>
                <Button isIconOnly size='lg' variant='light' className='w-20 h-20' onClick={onAttachmentOpen}>
                    <div className="flex flex-col items-center gap-2">
                        <GrAttachment fontSize={24} />
                        <p className="text-[0.65rem]">Attachments</p>
                        <AttachmentsModal tripId={tripId} isOpen={isAttachmentOpen} onOpen={onAttachmentOpen} onOpenChange={onAttachmentOpenChange} />
                    </div>
                </Button>
                <Dropdown>
                    <DropdownTrigger>
                        <Button isIconOnly size='lg' variant='light' className='w-20 h-20'>
                            <div className="flex flex-col items-center gap-2">
                                <MdMoreHoriz fontSize={24} />
                                <p className="text-[0.65rem]">More</p>
                            </div>
                        </Button>
                    </DropdownTrigger>
                    <DropdownMenu aria-label="Static Actions">
                        <DropdownItem
                            key="archive"
                            startContent={<MdArchive />}
                            onPress={handleArchive}
                        >
                            Archive
                        </DropdownItem>
                        <DropdownItem
                            key="delete"
                            className="text-danger"
                            color="danger"
                            startContent={<MdDelete />}
                            onPress={handleDelete}
                        >
                            Delete
                        </DropdownItem>
                    </DropdownMenu>
                </Dropdown>
            </div>
        </Card>
    )
}
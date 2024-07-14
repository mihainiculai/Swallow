"use client"

import React, { useRef } from "react";
import { Modal, ModalContent, ModalHeader, ModalBody, ModalFooter, Button } from "@nextui-org/react";
import useSWR from "swr";
import { mutate } from "swr";
import { fetcher } from "@/components/utilities/fetcher";
import { axiosInstance } from "@/components/utilities/axiosInstance";

import { IoMdDocument } from "react-icons/io";
import { MdFileDownload, MdDelete } from "react-icons/md";

interface AttachmentsModalProps {
    tripId: string;
    isOpen: boolean;
    onOpen: () => void;
    onOpenChange: () => void;
}

export const AttachmentsModal = ({ tripId, isOpen, onOpen, onOpenChange }: AttachmentsModalProps) => {
    const fileInputRef = useRef<HTMLInputElement>(null);
    const { data: attachments, error } = useSWR(`itineraries/${tripId}/attachments`, fetcher);

    const handleUpload = () => {
        fileInputRef.current?.click();
    };

    const uploadAttachment = async (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        if (!file) return;

        const formData = new FormData();
        formData.append('file', file);

        try {
            await axiosInstance.post(`itineraries/${tripId}/attachments`, formData, {
                headers: { 'Content-Type': 'multipart/form-data' }
            });
            mutate(`itineraries/${tripId}/attachments`);
            // Reset the file input
            if (fileInputRef.current) fileInputRef.current.value = '';
        } catch (error) {
            console.error("Error uploading file:", error);
        }
    };

    const downloadAttachment = async (attachmentId: string) => {
        try {
            const response = await axiosInstance.get(`itineraries/${tripId}/attachments/${attachmentId}`, {
                responseType: 'blob'
            });
            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', attachments[attachmentId]);
            document.body.appendChild(link);
            link.click();
            link.remove();
        } catch (error) {
            console.error("Error downloading file:", error);
        }
    };

    const deleteAttachment = async (attachmentId: string) => {
        try {
            await axiosInstance.delete(`itineraries/${tripId}/attachments/${attachmentId}`);
            mutate(`itineraries/${tripId}/attachments`);
        } catch (error) {
            console.error("Error deleting file:", error);
        }
    };

    return (
        <Modal isOpen={isOpen} onOpenChange={onOpenChange}>
            <ModalContent>
                {(onClose) => (
                    <>
                        <ModalHeader className="flex flex-col gap-1">Attachments</ModalHeader>
                        <ModalBody>
                            {attachments && Object.entries(attachments).map(([id, fileName]) => (
                                <div key={id} className="flex justify-between items-center">
                                    <span>{fileName}</span>
                                    <div className="flex gap-2">
                                        <Button
                                            isIconOnly
                                            size="sm"
                                            color="primary"
                                            variant="light"
                                            onPress={() => downloadAttachment(id)}
                                        >
                                            <MdFileDownload />
                                        </Button>
                                        <Button
                                            isIconOnly
                                            size="sm"
                                            color="danger"
                                            variant="light"
                                            onPress={() => deleteAttachment(id)}
                                        >
                                            <MdDelete />
                                        </Button>
                                    </div>
                                </div>
                            ))}
                            <input
                                type="file"
                                ref={fileInputRef}
                                onChange={uploadAttachment}
                                style={{ display: 'none' }}
                            />
                        </ModalBody>
                        <ModalFooter>
                            <Button color="danger" variant="light" onPress={onClose}>
                                Close
                            </Button>
                            <Button color="primary" onPress={handleUpload}>
                                Upload
                            </Button>
                        </ModalFooter>
                    </>
                )}
            </ModalContent>
        </Modal>
    )
}
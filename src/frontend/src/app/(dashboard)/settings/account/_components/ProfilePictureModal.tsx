import React, { useRef, useState, useEffect, ChangeEvent } from 'react';
import { Badge, Avatar, Button, Modal, ModalContent, ModalHeader, ModalBody, ModalFooter } from '@nextui-org/react';
import { FaTrash } from "react-icons/fa6";
import { axiosInstance } from '@/components/axiosInstance';
import { mutate } from 'swr';

interface ProfilePictureModalProps {
    isOpen: boolean;
    onOpenChange: (isOpen: boolean) => void;
    user: any;
}

export const ProfilePictureModal: React.FC<ProfilePictureModalProps> = ({ isOpen, onOpenChange, user }) => {
    const [imageFile, setImageFile] = useState<File | null>(null);
    const [imagePreview, setImagePreview] = useState<string | null | undefined>(user?.profilePictureUrl);
    const [uploadError, setUploadError] = useState('');
    const [imageChanged, setImageChanged] = useState(false);
    const [isUploading, setIsUploading] = useState(false);

    useEffect(() => {
        setImageFile(null);
        setImagePreview(user?.profilePictureUrl);
        setUploadError('');
        setImageChanged(false);
    }, [isOpen, user?.profilePictureUrl]);

    const fileInputRef = useRef<HTMLInputElement>(null);

    const allowedFileTypes = ['image/jpeg', 'image/png'];
    const allowedFileSize = 5 * 1024 * 1024; // 5MB

    const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
        const files = event.target.files;
        if (files && files.length > 0) {
            const file = files[0];

            if (!allowedFileTypes.includes(file.type)) {
                setUploadError('Invalid file type. Allowed types are JPEG, PNG, and GIF.');
                return;
            }

            if (file.size > allowedFileSize) {
                setUploadError('File size is too large. Max file size is 5MB.');
                return;
            }

            setImageFile(file);
            setUploadError('');

            const reader = new FileReader();
            reader.onloadend = () => {
                setImagePreview(reader.result as string);
            };
            reader.readAsDataURL(file);

            setImageChanged(true);
        }
    };

    const handleUpload = () => {
        if (!imageChanged) {
            onOpenChange(false);
            return;
        }

        const formData = new FormData();

        if (imageFile) {
            formData.append('file', imageFile);
        }

        setIsUploading(true);
        axiosInstance.post('/users/profile-picture', formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        }).then(() => {
            onOpenChange(false);
        }).catch(error => {
            setUploadError(error.response.data);
        }).finally(() => {
            setIsUploading(false);
            mutate('auth/me');
        });
    };

    const handleDelete = () => {
        if (imagePreview) {
            setImageFile(null);
            setImagePreview(null);
            setUploadError('');
            setImageChanged(true);
        }
    };

    return (
        <Modal placement="center" isOpen={isOpen} onOpenChange={onOpenChange}>
            <ModalContent>
                <ModalHeader>Profile Picture</ModalHeader>
                <ModalBody className="flex flex-col items-center gap-10">
                    <Badge
                        classNames={{
                            badge: "w-6 h-6",
                        }}
                        color="danger"
                        content={
                            <Button
                                isIconOnly
                                className="p-0 text-primary-foreground"
                                radius="full"
                                size="sm"
                                variant="light"
                                onClick={handleDelete}
                            >
                                <FaTrash />
                            </Button>
                        }
                        placement="bottom-right"
                        shape="circle"
                    >
                        <Avatar src={imagePreview ? imagePreview : undefined} className="w-20 h-20 text-large" />
                    </Badge>
                    <input type="file" onChange={handleFileChange} className="hidden" accept="image/*" id="file" ref={fileInputRef} />
                    <label htmlFor="file">
                        <Button variant="light" color="primary" onClick={() => fileInputRef.current?.click()}>
                            Upload Image
                        </Button>
                    </label>
                    {uploadError && <p className='text-danger'>{uploadError}</p>}
                </ModalBody>
                <ModalFooter>
                    <Button variant="light" onClick={() => onOpenChange(false)}>Cancel</Button>
                    <Button color="primary" onClick={handleUpload} isLoading={isUploading}>Update</Button>
                </ModalFooter>
            </ModalContent>
        </Modal>
    );
};
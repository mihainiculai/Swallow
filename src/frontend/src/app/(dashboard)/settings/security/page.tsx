"use client";
import {
    Button,
    Card,
    CardBody,
    CardHeader,
    useDisclosure,
} from "@nextui-org/react";
import { CellWrapper } from "@/components/cell-wrapper";
import { SwitchCell } from "@/components/ui-elements/switch-cell";
import { ChangePasswordModal } from "./_components/change-password-modal";
import { DeleteAccountModal } from "./_components/delete-account-modal";

export default function AccountSettingsPage() {
    const { isOpen: isOpenPasswordModal, onOpen: onOpenPasswordModal, onOpenChange: onOpenChangePasswordModal } = useDisclosure();
    const { isOpen: isOpenDeleteModal, onOpen: onOpenDeleteModal, onOpenChange: onOpenChangeDeleteModal } = useDisclosure();

    return (
        <>
            <Card className="full-width p-4">
                <CardHeader className="flex flex-col items-start px-4 pb-0 pt-4">
                    <p className="text-large">Security Settings</p>
                    <p className="text-small text-default-500">Manage your security preferences</p>
                </CardHeader>
                <CardBody className="flex flex-col gap-4">
                    <SwitchCell
                        defaultSelected
                        description="Add an extra layer of security to your account."
                        label="Two-Factor Authentication"
                    />
                    <CellWrapper>
                        <div>
                            <p>Password</p>
                            <p className="text-small text-default-500">
                                Set a unique password to protect your account.
                            </p>
                        </div>
                        <Button radius="full" variant="bordered" onClick={onOpenPasswordModal}>
                            Change
                        </Button>
                    </CellWrapper>
                    <CellWrapper>
                        <div>
                            <p>Delete Account</p>
                            <p className="text-small text-default-500">Delete your account and all your data.</p>
                        </div>
                        <Button color="danger" radius="full" variant="flat" onClick={onOpenDeleteModal}>
                            Delete
                        </Button>
                    </CellWrapper>
                </CardBody>
            </Card >

            <ChangePasswordModal isOpen={isOpenPasswordModal} onOpenChange={onOpenChangePasswordModal} />
            <DeleteAccountModal isOpen={isOpenDeleteModal} onOpenChange={onOpenChangeDeleteModal} />
        </>
    );
}
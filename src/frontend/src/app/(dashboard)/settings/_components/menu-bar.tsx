import { TabMenubar } from "@/components/TabManuBar";
import { MdAccountCircle, MdWallet, MdLock, MdSettings } from "react-icons/md";

const items = [
    {
        key: 'account',
        title: 'Account',
        icon: <MdAccountCircle />,
    },
    {
        key: 'security',
        title: 'Security',
        icon: <MdLock />,
    },
    {
        key: 'preferences',
        title: 'Preferences',
        icon: <MdSettings />,
    },
    {
        key: 'membership',
        title: 'Membership',
        icon: <MdWallet />,
    },
];

export const Menubar = () => {
    return (
        <TabMenubar items={items} basePath="/settings" />
    );
}
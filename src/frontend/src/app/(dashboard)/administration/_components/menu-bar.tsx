import { TabMenubar } from "@/components/TabManuBar";
import { MdDashboard } from "react-icons/md";
import { IoIosStats } from "react-icons/io";
import { FaCity } from "react-icons/fa";


const items = [
    {
        key: 'dashboard',
        title: 'Dashboard',
        icon: <MdDashboard />,
    },
    {
        key: 'statistics',
        title: 'Statistics',
        icon: <IoIosStats />,
    },
    {
        key: 'cities',
        title: 'Cities',
        icon: <FaCity />,
    }
];

export const Menubar = () => {
    return (
        <TabMenubar items={items} basePath="/administration" />
    );
}
"use client"

import { Card } from "@nextui-org/react";
import useSWR from "swr"
import { fetcher } from "@/components/utilities/fetcher";
import { Bar } from 'react-chartjs-2';
import { Chart as ChartJS, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend } from 'chart.js';
import { FaUsers, FaGlobe, FaCity, FaPlane } from 'react-icons/fa';

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

export default function StatisticsPage() {
    const { data } = useSWR("administration/statistics", fetcher)

    const chartData = {
        labels: data ? Object.keys(data.tripsPerMonth).reverse() : [],
        datasets: [
            {
                label: 'Trips per Month',
                data: data ? Object.values(data.tripsPerMonth).reverse() : [],
                backgroundColor: '#FD8524',
            },
        ],
    };

    const chartOptions = {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: {
                display: false,
            },
            title: {
                display: true,
                text: 'Trips per Month',
            },
        },
    };

    const StatCard = ({ icon, value, label }: { icon: React.ReactNode, value: number, label: string }) => (
        <Card className="p-6 col-span-1 flex flex-col items-center justify-center">
            <div className="w-20 h-20 rounded-full bg-[#FD8524] flex items-center justify-center mb-4">
                {icon}
            </div>
            <div className="text-2xl font-bold">{value}</div>
            <div className="text-sm text-gray-500">{label}</div>
        </Card>
    );

    return (
        <div className="grid md:grid-rows-2 grid-cols-2 md:grid-cols-5 gap-4">
            <Card className="p-6 row-span-2 col-span-2 md:col-span-3">
                {data && <Bar options={chartOptions} data={chartData} />}
            </Card>
            <StatCard 
                icon={<FaUsers size={30} color="white" />}
                value={data?.numberOfUsers}
                label="Users"
            />
            <StatCard 
                icon={<FaGlobe size={30} color="white" />}
                value={data?.numberOfCountries}
                label="Countries"
            />
            <StatCard 
                icon={<FaCity size={30} color="white" />}
                value={data?.numberOfCities}
                label="Cities"
            />
            <StatCard 
                icon={<FaPlane size={30} color="white" />}
                value={data?.numberOfTrips}
                label="Trips"
            />
        </div>
    )
}
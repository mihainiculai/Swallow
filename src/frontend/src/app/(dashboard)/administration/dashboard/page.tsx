"use client";

import React, { useEffect, useState } from "react";
import axios from "axios";
import { Card } from "@nextui-org/react";
import { Bar, Pie, Doughnut } from 'react-chartjs-2';
import { Chart as ChartJS, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend, PointElement, LineElement, ArcElement, ChartOptions } from 'chart.js';

ChartJS.register(
    CategoryScale,
    LinearScale,
    BarElement,
    Title,
    Tooltip,
    Legend,
    PointElement,
    LineElement,
    ArcElement
);

interface Metrics {
    requestDuration: Record<string, number>;
    requestCount: Record<string, number>;
    cpuUsage: number;
    memoryUsage: number;
    threadCount: number;
    gcCollections: Record<string, number>;
}

export default function AdministrativeDashboardPage() {
    const [metrics, setMetrics] = useState<Metrics | null>(null);

    useEffect(() => {
        axios.get<string>(`${process.env.NEXT_PUBLIC_API_URL}/metrics`)
            .then((response) => {
                setMetrics(parseMetrics(response.data));
            })
            .catch((error) => {
                console.error(error);
            });
    }, []);

    const parseMetrics = (data: string): Metrics => {
        let parsedData: Metrics = {
            requestDuration: {},
            requestCount: {},
            cpuUsage: 0,
            memoryUsage: 0,
            threadCount: 0,
            gcCollections: {}
        };

        const lines = data.split('\n');
        lines.forEach(line => {
            if (line.startsWith('http_request_duration_seconds_sum')) {
                const match = line.match(/{([^}]+)}/);
                if (match) {
                    const labels = match[1].split(',');
                    const endpoint = labels.find(l => l.startsWith('endpoint='))?.split('=')[1].replace(/"/g, '') ?? '';
                    parsedData.requestDuration[endpoint] = parseFloat(line.split(' ')[1]);
                }
            } else if (line.startsWith('http_requests_received_total')) {
                const match = line.match(/{([^}]+)}/);
                if (match) {
                    const labels = match[1].split(',');
                    const endpoint = labels.find(l => l.startsWith('endpoint='))?.split('=')[1].replace(/"/g, '') ?? '';
                    parsedData.requestCount[endpoint] = parseInt(line.split(' ')[1], 10);
                }
            } else if (line.startsWith('process_cpu_seconds_total')) {
                parsedData.cpuUsage = parseFloat(line.split(' ')[1]);
            } else if (line.startsWith('process_working_set_bytes')) {
                parsedData.memoryUsage = parseInt(line.split(' ')[1], 10) / (1024 * 1024);
            } else if (line.startsWith('process_num_threads')) {
                parsedData.threadCount = parseInt(line.split(' ')[1], 10);
            } else if (line.startsWith('dotnet_collection_count_total')) {
                const match = line.match(/{([^}]+)}/);
                if (match) {
                    const generation = match[1].split('=')[1].replace(/"/g, '');
                    parsedData.gcCollections[generation] = parseInt(line.split(' ')[1], 10);
                }
            }
        });

        return parsedData;
    };

    const chartOptions: ChartOptions<'bar' | 'pie' | 'doughnut'> = {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: {
                display: false,
            },
            tooltip: {
                enabled: true,
            },
        },
        scales: {
            x: {
                ticks: {
                    display: false,
                },
                grid: {
                    display: false,
                },
            },
            y: {
                ticks: {
                    display: false,
                },
                grid: {
                    display: true,
                },
            },
        },
    };

    const renderCharts = () => {
        if (!metrics) return <p className="text-center text-gray-600">Loading...</p>;

        const durationData = {
            labels: Object.keys(metrics.requestDuration),
            datasets: [
                {
                    label: 'HTTP Request Duration (seconds)',
                    data: Object.values(metrics.requestDuration),
                    backgroundColor: '#FD8524',
                    borderColor: '#FD8524',
                    borderWidth: 1,
                },
            ],
        };

        const countData = {
            labels: Object.keys(metrics.requestCount),
            datasets: [
                {
                    label: 'HTTP Requests Received',
                    data: Object.values(metrics.requestCount),
                    backgroundColor: '#FD8524',
                    borderColor: '#FFFFFF',
                    borderWidth: 1,
                },
            ],
        };

        const performanceData = {
            labels: ['CPU Usage (*100)', 'Memory Usage (MB)', 'Thread Count'],
            datasets: [
                {
                    label: 'Performance Metrics',
                    data: [metrics.cpuUsage * 100, metrics.memoryUsage, metrics.threadCount],
                    backgroundColor: [
                        '#FD8524',
                        '#FD8524',
                        '#FD8524',
                    ],
                    borderColor: [
                        '#FD8524',
                        '#FD8524',
                        '#FD8524',
                    ],
                    borderWidth: 1,
                },
            ],
        };

        const gcData = {
            labels: Object.keys(metrics.gcCollections),
            datasets: [
                {
                    label: 'GC Collections',
                    data: Object.values(metrics.gcCollections),
                    backgroundColor: [
                        '#FD8524',
                        '#FD8524',
                        '#FD8524',
                    ],
                    borderColor: '#FFFFFF',
                    borderWidth: 1,
                },
            ],
        };

        return (
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                <Card className="p-6">
                    <h2 className="text-xl font-bold mb-2">Request Duration</h2>
                    <div className="h-96 w-full">
                        <Bar data={durationData} options={chartOptions} />
                    </div>
                </Card>
                <Card className="p-6">
                    <h2 className="text-xl font-bold mb-2">Request Count</h2>
                    <div className="h-96 w-full">
                        <Pie data={countData} options={chartOptions} />
                    </div>
                </Card>
                <Card className="p-6">
                    <h2 className="text-xl font-bold mb-2">Performance Metrics</h2>
                    <div className="h-96 w-full">
                        <Bar data={performanceData} options={chartOptions} />
                    </div>
                </Card>
                <Card className="p-6">
                    <h2 className="text-xl font-bold mb-2">Garbage Collection</h2>
                    <div className="h-96 w-full">
                        <Doughnut data={gcData} options={chartOptions} />
                    </div>
                </Card>
            </div>
        );
    };

    return (
        <div>
            {renderCharts()}
        </div>
    );
}

import React from 'react';
import { Spacer } from "@nextui-org/react";
import { Button } from '@nextui-org/button';
import Typography from '@/components/Typography';

export default function Home() {
    return (
        <>
            <section
                className="bg-cover bg-center h-screen text-center relative"
                style={{ backgroundImage: 'url(/landing/hero-cover.webp)' }}
                aria-label='Hero Section'
            >
                <div
                    style={{ backgroundColor: 'rgba(0, 0, 0, 0.35)' }}
                    className="absolute inset-0 flex flex-col justify-center items-center"
                >
                    <Typography variant='title' color='primary' size='lg' fullWidth={true}>
                        Swallow
                    </Typography>
                    <Spacer y={6} />
                    <Typography variant='title' fullWidth={true}>
                        Where Your Next Adventure Takes Wing
                    </Typography>
                    <Spacer y={12} />
                    <Button size='lg' color='primary'>Get Started</Button>
                </div>
            </section>
        </>
    )
}

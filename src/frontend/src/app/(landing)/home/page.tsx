import React from 'react';
import Image from 'next/image';
import { Spacer } from "@nextui-org/react";
import { Button } from '@nextui-org/button';
import Typography from '@/components/ui-elements/typography';
import Link from 'next/link';

export default function Home() {
    return (
        <>
            <section
                className="bg-cover bg-center h-screen text-center relative"
                aria-label='Hero Section'
            >
                <Image
                    fill
                    src="/landing/hero-cover.webp"
                    alt="Hero Cover"
                    className="bg-cover bg-center"
                />
                <div
                    style={{ backgroundColor: 'rgba(0, 0, 0, 0.35)' }}
                    className="absolute inset-0 flex flex-col justify-center items-center"
                >
                    <Typography variant='title' color='primary' size='lg' fullWidth={true}>
                        Swallow
                    </Typography>
                    <Spacer y={6} />
                    <Typography variant='title' fullWidth={true} className='text-neutral-100'>
                        Where Your Next Adventure Takes Wing
                    </Typography>
                    <Spacer y={12} />
                    <Button as={Link} href='/auth/login' size='lg' color='primary'>Get Started</Button>
                </div>
            </section>
        </>
    )
}

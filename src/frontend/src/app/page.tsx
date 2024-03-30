import { redirect } from 'next/navigation'

export default function Root(): never {
    redirect('/home')
}
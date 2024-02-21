import { redirect } from 'next/navigation'

export default function SettingsPage() {
    redirect('/administration/stats');
    return null;
}
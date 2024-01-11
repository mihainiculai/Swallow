export type NavbarItemType = {
    name: string
    path: string
}

type NavbarConfigType = {
    items: NavbarItemType[]
}

export const NavbarConfig: NavbarConfigType = {
    items: [
        {
            name: 'Dashboard',
            path: '/dashboard',
        },
        {
            name: 'Discover',
            path: '/discover',
        },
    ]
}
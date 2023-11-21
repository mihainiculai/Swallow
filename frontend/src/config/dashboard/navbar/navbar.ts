type NavbarItemType = {
    name: string
    path: string
}

type NavbarConfigType = {
    items: NavbarItemType[]
}

export const NavbarConfig: NavbarConfigType = {
    items: [
        {
            name: 'dashboard',
            path: '/dashboard',
        },
        {
            name: 'Discover',
            path: '/discover',
        },
    ]
}
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
            name: 'Home',
            path: '/home',
        },
        {
            name: 'About',
            path: '/about',
        },
        {
            name: 'Prices',
            path: '/prices',
        },
        {
            name: 'FAQ',
            path: '/faq',
        },
        {
            name: 'Contact',
            path: '/contact',
        },
    ]
}
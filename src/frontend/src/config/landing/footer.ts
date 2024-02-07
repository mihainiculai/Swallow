type FooterItemType = {
    name: string
    path: string
}

type FooterConfigType = {
    items: FooterItemType[]
}

export const FooterConfig: FooterConfigType = {
    items: [
        {
            name: 'About',
            path: '/about',
        },
        {
            name: 'Privacy Policy',
            path: '/privacy',
        },
        {
            name: 'Terms of Service',
            path: '/terms',
        },
        {
            name: 'Contact',
            path: '/contact',
        },
    ]
}
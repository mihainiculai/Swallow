type DropdownItemType = {
    name: string
    path: string
}

type DropdownConfigType = {
    items: DropdownItemType[]
}

export const DropdownConfig: DropdownConfigType = {
    items: [
        {
            name: 'My Profile',
            path: '/profile',
        },
        {
            name: 'Memberships',
            path: '/memberships',
        },
        {
            name: 'Settings',
            path: '/settings',
        },
    ]
}
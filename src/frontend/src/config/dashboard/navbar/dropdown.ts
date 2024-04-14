export type DropdownItemType = {
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
            path: '/settings/membership',
        },
        {
            name: 'Settings',
            path: '/settings',
        },
    ]
}
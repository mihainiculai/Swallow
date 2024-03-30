export interface User {
    email: string;
    username: string;
    firstName: string;
    lastName: string;
    fullName: string;
    profilePictureUrl?: string;
    public: boolean;
}
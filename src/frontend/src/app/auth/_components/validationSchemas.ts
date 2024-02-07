import * as yup from "yup";

export const loginSchema = yup.object({
    email: yup.string().email("Invalid email format").required("Email is required"),
    password: yup.string().required("Password is required"),
})

export const registrationSchema = yup.object({
    email: yup
        .string()
        .email("Invalid email format")
        .required("Email is required")
        .max(100, 'Email should be at most 100 characters long')
        .matches(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/, 'Email can only contain letters, numbers, and special characters'),
    password: yup
        .string()
        .required("Password is required")
        .min(6, 'Password should be at least 6 characters long')
        .matches(/[a-z]/, 'Password must contain at least one lowercase letter')
        .matches(/[A-Z]/, 'Password must contain at least one uppercase letter')
        .matches(/[0-9]/, 'Password must contain at least one number')
        .matches(/[@$!%*#?&+\-\[\]]/, 'Password must contain at least one special character')
        .matches(/^[a-zA-Z0-9@$!%*#?&+\-\[\]]+$/, 'Password can only contain letters, numbers, and special characters'),
    confirmPassword: yup
        .string()
        .oneOf([yup.ref('password')], 'Passwords must match')
        .required('Confirm Password is required'),
    firstName: yup
        .string()
        .required("First name is required")
        .min(2, 'First name should be at least 2 characters long')
        .max(100, 'First name should be at most 100 characters long')
        .matches(/^[a-zA-Z]+$/, 'First name can only contain letters'),
    lastName: yup
        .string()
        .required("Last name is required"
        ).min(2, 'Last name should be at least 2 characters long')
        .max(100, 'Last name should be at most 100 characters long')
        .matches(/^[a-zA-Z]+$/, 'Last name can only contain letters'),
    terms: yup
        .boolean()
        .oneOf([true], "You must accept the terms and conditions"),
});

export const forgotPasswordSchema = yup.object({
    email: yup.string().email("Invalid email format").required("Email is required").max(100, 'Email should be at most 100 characters long'),
});

export const resetPasswordSchema = yup.object({
    password: yup
        .string()
        .required("Password is required")
        .min(6, 'Password should be at least 6 characters long')
        .matches(/[a-z]/, 'Password must contain at least one lowercase letter')
        .matches(/[A-Z]/, 'Password must contain at least one uppercase letter')
        .matches(/[0-9]/, 'Password must contain at least one number')
        .matches(/[@$!%*#?&+\-\[\]]/, 'Password must contain at least one special character')
        .matches(/^[a-zA-Z0-9@$!%*#?&+\-\[\]]+$/, 'Password can only contain letters, numbers, and special characters'),
    confirmPassword: yup
        .string()
        .oneOf([yup.ref('password')], 'Passwords must match')
        .required('Confirm Password is required'),
});
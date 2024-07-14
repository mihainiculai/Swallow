/** @type {import('next').NextConfig} */
const nextConfig = {
    experimental: {
        missingSuspenseWithCSRBailout: false,
    },
    typescript: {
        ignoreBuildErrors: true,
    },
}

module.exports = nextConfig

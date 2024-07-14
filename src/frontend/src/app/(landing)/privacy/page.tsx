export default function Privacy() {
    return (
        <div className="mx-auto w-full max-w-6xl py-20 sm:px-6 lg:px-8">
            <h1 className="text-4xl font-bold mb-8">Privacy Policy</h1>

            <h2 className="text-3xl font-semibold mb-6">Introduction</h2>
            <p className="text-base mb-6">Welcome to Swallow, your AI-powered trip planner. We value your privacy and are committed to protecting your personal information. This Privacy Policy outlines how we collect, use, and safeguard your data. Our aim is to be transparent and clear, so you can make informed decisions about using our services.</p>

            <h2 className="text-3xl font-semibold mb-6">Data We Collect</h2>

            <h3 className="text-2xl font-medium mb-4">Personal Information</h3>
            <p className="text-base mb-4">When you sign up for an account or use our services, we may collect the following personal information:</p>
            <ul className="list-disc list-inside mb-6">
                <li><strong>Contact Information:</strong> Name, email address, and phone number.</li>
                <li><strong>Profile Information:</strong> Preferences, travel history, and interests.</li>
                <li><strong>Payment Information:</strong> Credit card details and billing address (processed securely via Stripe).</li>
            </ul>

            <h3 className="text-2xl font-medium mb-4">Usage Data</h3>
            <p className="text-base mb-4">We collect data on how you interact with our website and app, including:</p>
            <ul className="list-disc list-inside mb-6">
                <li><strong>Device Information:</strong> IP address, browser type, operating system, and device identifiers.</li>
                <li><strong>Activity Data:</strong> Pages visited, time spent on pages, clicks, and navigation paths.</li>
            </ul>

            <h3 className="text-2xl font-medium mb-4">Cookies and Tracking</h3>
            <p className="text-base mb-6">We use cookies and similar technologies to enhance your experience. Cookies help us remember your preferences and understand how you use our site. You can control cookies through your browser settings.</p>

            <h2 className="text-3xl font-semibold mb-6">How We Use Your Data</h2>

            <h3 className="text-2xl font-medium mb-4">Providing Services</h3>
            <p className="text-base mb-4">We use your personal information to:</p>
            <ul className="list-disc list-inside mb-6">
                <li>Create and manage your account.</li>
                <li>Generate personalized travel itineraries.</li>
                <li>Process payments and manage subscriptions.</li>
            </ul>

            <h3 className="text-2xl font-medium mb-4">Improving Our Services</h3>
            <p className="text-base mb-4">We analyze usage data to:</p>
            <ul className="list-disc list-inside mb-6">
                <li>Enhance website functionality and user experience.</li>
                <li>Develop new features and improve existing ones.</li>
                <li>Monitor and prevent any fraudulent activity.</li>
            </ul>

            <h3 className="text-2xl font-medium mb-4">Communication</h3>
            <p className="text-base mb-4">We may use your contact information to:</p>
            <ul className="list-disc list-inside mb-6">
                <li>Send updates, newsletters, and promotional materials.</li>
                <li>Respond to your inquiries and provide customer support.</li>
                <li>Notify you of changes to our services or policies.</li>
            </ul>

            <h2 className="text-3xl font-semibold mb-6">Sharing Your Data</h2>
            <p className="text-base mb-4">We do not sell or rent your personal information to third parties. We may share your data with:</p>
            <ul className="list-disc list-inside mb-6">
                <li><strong>Service Providers:</strong> Trusted partners who help us operate our website, process payments, and deliver services (e.g., Stripe for payment processing).</li>
                <li><strong>Legal Compliance:</strong> If required by law or to protect our rights and property.</li>
            </ul>

            <h2 className="text-3xl font-semibold mb-6">Data Security</h2>
            <p className="text-base mb-4">We implement advanced security measures to protect your data, including:</p>
            <ul className="list-disc list-inside mb-6">
                <li><strong>Encryption:</strong> Data is encrypted both in transit and at rest.</li>
                <li><strong>Access Controls:</strong> Limited access to personal information by authorized personnel only.</li>
                <li><strong>Regular Audits:</strong> Security practices are reviewed regularly to ensure ongoing protection.</li>
            </ul>

            <h2 className="text-3xl font-semibold mb-6">Your Rights</h2>
            <p className="text-base mb-4">You have the following rights regarding your personal data:</p>
            <ul className="list-disc list-inside mb-6">
                <li><strong>Access:</strong> Request a copy of the personal information we hold about you.</li>
                <li><strong>Correction:</strong> Update or correct your personal information.</li>
                <li><strong>Deletion:</strong> Request the deletion of your personal data.</li>
                <li><strong>Restriction:</strong> Limit the processing of your personal information.</li>
                <li><strong>Objection:</strong> Object to the processing of your data for specific purposes.</li>
                <li><strong>Portability:</strong> Request a transfer of your data to another service provider.</li>
            </ul>
            <p className="text-base mb-6">To exercise these rights, please contact us at <a href="mailto:privacy@swallow.com" className="text-blue-500 hover:underline">privacy@swallow.com</a>.</p>

            <h2 className="text-3xl font-semibold mb-6">Data Retention</h2>
            <p className="text-base mb-6">We retain your personal information for as long as necessary to provide our services and comply with legal obligations. When data is no longer needed, we securely delete or anonymize it.</p>

            <h2 className="text-3xl font-semibold mb-6">Changes to This Policy</h2>
            <p className="text-base mb-6">We may update this Privacy Policy from time to time. We will notify you of any significant changes by email or through our website. Your continued use of our services after any changes indicates your acceptance of the updated policy.</p>

            <h2 className="text-3xl font-semibold mb-6">Contact Us</h2>
            <p className="text-base mb-2">If you have any questions or concerns about this Privacy Policy, please contact us at:</p>

            <p className="text-base mb-6">
                <strong>Swallow AI Trip Planner</strong>
                <br />
                Email: <a href="mailto:privacy@swallow.com" className="text-blue-500 hover:underline">privacy@swallow.com</a>
                <br />
                Address: 1234 Travel Lane, Wanderlust City, Explore Country
            </p>

            <p className="text-base">Thank you for trusting Swallow with your travel planning needs. We are committed to protecting your privacy and ensuring a safe and enjoyable experience.</p>    
        </div>
    );
}

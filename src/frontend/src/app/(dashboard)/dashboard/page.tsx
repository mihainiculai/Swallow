
import Typography from "@/components/ui-elements/typography"
import {
    Button,
    Card,
    CardFooter,
    Image,
    Spacer,
} from "@nextui-org/react"
import Link from "next/link"
import { UpcomingTrips } from "./_components/upcoming-trips"

export default function Dashboard() {
    return (
        <div className="flex flex-col p-6 max-w-7xl mx-auto gap-4">
            <Card className="w-full relative h-[20rem]">
                <Image
                    removeWrapper
                    alt="City Picture"
                    className="z-0 w-full h-full object-cover"
                    src="/dashboard/DashboardCard.jpg"
                />
                <CardFooter className="absolute z-10 bottom-0 bg-black/40 p-4 flex w-full justify-between">
                    <div>
                        <Typography variant='title' color='primary' size='sm' fullWidth={true}>
                            Looking for Your Next Adventure?
                        </Typography>
                        <p className="text-neutral-100 text-sm">
                            Explore Exciting New Destinations Now!
                        </p>
                    </div>
                    <Button as={Link} href='/discover' color='primary'>Discover Now</Button>
                </CardFooter>
            </Card>
            <Spacer y={1} />
            <div>
                <h4 className="text-2xl text-primary font-semibold">Upcoming Trips</h4>
                <p className="text-default-600">You can&apos;t wait for them, right?</p>
            </div>
            <UpcomingTrips />
        </div>
    )
}
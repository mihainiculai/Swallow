export default function CityDetails({ params }: { params: { id: string } }) {
    return (
        <div>
            <h1>City Details</h1>
            <h2>{params.id}</h2>
        </div>
    );
}
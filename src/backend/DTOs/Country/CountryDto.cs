namespace Swallow.DTOs.Country
{
    public class CountryDto
    {
        public short CountryId { get; set; }
        public required string Name { get; set; }
        public required string Iso2 { get; set; }
    }
}

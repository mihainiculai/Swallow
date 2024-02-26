using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Swallow.Utils
{
    public partial class TripAdvisorAttractionsCollector(HttpClient httpClient)
    {
        private static readonly string BaseUrl = "https://www.tripadvisor.com";

        [GeneratedRegex(@"^\d+\.\s")]
        private static partial Regex TitleRegex();

        [GeneratedRegex(@"([\d,]+)\sreviews?")]
        private static partial Regex ReviewRegex();

        [GeneratedRegex(@"from\s+\$(\d+\.\d+|\d+)")]
        private static partial Regex PriceRegex();

        [GeneratedRegex(@"(\d+\.\d+|\d+) of \d+ bubbles")]
        private static partial Regex RatingRegex();

        public async Task<List<Attraction>> GetAttractionsAsync(string TripAdvisorUrl)
        {
            List<Attraction> attractions = [];
            string url = TripAdvisorUrl;

            while (!string.IsNullOrEmpty(url))
            {
                var document = await GetHtmlDocumentAsync(url);

                if (document == null) continue;

                var sections = document.DocumentNode.SelectNodes("//section[@data-automation='WebPresentation_SingleFlexCardSection']");

                if (sections != null)
                {
                    var tasks = sections.Select(section => FetchSingleAttractionAsync(section));
                    var results = await Task.WhenAll(tasks);

                    attractions.AddRange(results.Where(result => result != null).Cast<Attraction>());
                }

                var nextButton = document.DocumentNode.SelectSingleNode("//a[@aria-label='Next page']");
                url = nextButton != null ? BaseUrl + nextButton.Attributes["href"].Value : "";
            }

            return attractions;
        }

        private async Task<Attraction?> FetchSingleAttractionAsync(HtmlNode section)
        {
            var nameElement = section.SelectSingleNode(".//h3[contains(@class, 'biGQs')]");
            var linkElement = section.SelectSingleNode(".//div[contains(@class, 'alPVI')]/a");

            if (nameElement != null && linkElement != null)
            {
                var name = TitleRegex().Replace(nameElement.InnerText.Trim(), "");
                var link = BaseUrl + linkElement.Attributes["href"].Value;

                var details = await GetAttractionDetailsAsync(link);

                if (details == null) return null;

                var categories = details.Categories;
                if (categories.Any(c => c.Contains("Trips") || c.Contains("Tours")))
                {
                    return null;
                }

                return new Attraction
                {
                    Name = name,
                    TripAdvisorLink = link,
                    Details = details
                };
            }

            return null;
        }

        private async Task<HtmlDocument?> GetHtmlDocumentAsync(string url)
        {
            var response = await httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var document = new HtmlDocument();
            document.LoadHtml(content);
            return document;
        }

        private async Task<AttractionDetails> GetAttractionDetailsAsync(string attractionUrl)
        {
            var details = new AttractionDetails();

            var htmlDoc = await GetHtmlDocumentAsync(attractionUrl);

            if (htmlDoc == null) return details;

            details.Rating = GetRating(htmlDoc);
            details.Reviews = GetReviews(htmlDoc);
            details.Categories = GetCategories(htmlDoc);
            details.VisitDuration = GetVisitDuration(htmlDoc);
            details.OpeningHours = GetOpeningHours(htmlDoc);
            details.Price = GetPrice(htmlDoc);
            details.ImageUrl = GetImageUrl(htmlDoc);
            details.Description = GetDescription(htmlDoc);

            return details;
        }

        private static double? GetRating(HtmlDocument htmlDoc)
        {
            var ratingDiv = htmlDoc.DocumentNode.SelectSingleNode("//div[@aria-label]");
            if (ratingDiv != null)
            {
                var ariaLabel = ratingDiv.GetAttributeValue("aria-label", "");
                var match = RatingRegex().Match(ariaLabel);
                if (match.Success)
                {
                    return double.Parse(match.Groups[1].Value);
                }
            }
            return null;
        }

        private static string? GetImageUrl(HtmlDocument htmlDoc)
        {
            var imgElement = htmlDoc.DocumentNode.SelectSingleNode("//li[contains(@class, 'MBoCH')]//img");
            if (imgElement != null)
            {
                var imgUrl = imgElement.GetAttributeValue("src", null);
                if (!string.IsNullOrEmpty(imgUrl))
                {
                    return imgUrl.Split('?')[0];
                }
            }
            return null;
        }

        private static int? GetReviews(HtmlDocument htmlDoc)
        {
            var reviewElement = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'KxBGd')]//span");
            if (reviewElement != null)
            {
                var reviewText = reviewElement.InnerText.Trim();
                var match = ReviewRegex().Match(reviewText);
                if (match.Success)
                {
                    return int.Parse(match.Groups[1].Value.Replace(",", ""));
                }
            }
            return null;
        }

        private static List<string> GetCategories(HtmlDocument htmlDoc)
        {
            var categoryElements = htmlDoc.DocumentNode.SelectNodes("//span[contains(@class, 'eojVo')]");
            return categoryElements?.Select(node => node.InnerText).ToList() ?? [];
        }

        private static string? GetPrice(HtmlDocument htmlDoc)
        {
            var priceElement = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'ncFvv')]//div[contains(@class, 'f')]");
            if (priceElement != null)
            {
                var priceText = priceElement.InnerText.Trim();
                var match = PriceRegex().Match(priceText);
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }
            return null;
        }

        private static string? GetVisitDuration(HtmlDocument htmlDoc)
        {
            var durationElement = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(text(), 'Suggested duration')]");
            if (durationElement != null)
            {
                var nextSibling = durationElement.SelectSingleNode("following-sibling::div");
                if (nextSibling != null)
                {
                    return nextSibling.InnerText.Trim();
                }
            }
            return null;
        }

        private static Dictionary<string, string> GetOpeningHours(HtmlDocument htmlDoc)
        {
            var hours = new Dictionary<string, string>();
            var hourElements = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'XZfLa')]");
            if (hourElements != null)
            {
                foreach (var element in hourElements)
                {
                    var day = element.SelectSingleNode(".//span[contains(@class, 'pZUbB')]").InnerText.Trim();
                    var time = element.SelectSingleNode(".//div[contains(@class, 'pZUbB')]").InnerText.Trim();
                    hours[day] = time;
                }
            }
            return hours;
        }

        private static string? GetDescription(HtmlDocument htmlDoc)
        {
            var descriptionElement = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'pZUbB')]//span[@class='JguWG']");
            if (descriptionElement != null)
            {
                var description = descriptionElement.InnerText.Trim();
                if (!string.IsNullOrEmpty(description))
                {
                    var lines = description.Split('\n');
                    var filteredLines = lines.Where(line => !line.Trim().Contains("Tripadvisor")).Select(line => line.Trim());
                    var filteredDescription = string.Join("\n", filteredLines).Trim();
                    return filteredDescription.Replace("\n\n", "\n");
                }
            }
            return null;
        }
    }

    public class Attraction
    {
        public required string Name { get; set; }
        public required string TripAdvisorLink { get; set; }
        public required AttractionDetails Details { get; set; }
    }

    public class AttractionDetails
    {
        public double? Rating { get; set; }
        public int? Reviews { get; set; }
        public List<string> Categories { get; set; } = [];
        public string? VisitDuration { get; set; }
        public Dictionary<string, string> OpeningHours { get; set; } = [];
        public string? Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
    }
}

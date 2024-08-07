﻿using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Swallow.DTOs.Attraction;

namespace Swallow.Utils.AttractionDataProviders;

public interface ITripAdvisorAttractionsCollector
{
    Task<List<TripAdvisorAttraction>> GetAttractionsAsync(string tripAdvisorUrl);
}

public partial class TripAdvisorAttractionsCollector(HttpClient httpClient) : ITripAdvisorAttractionsCollector
{
    private const string BaseUrl = "https://www.tripadvisor.com";

    [GeneratedRegex(@"^(\d+)\.\s*(.*)")]
    private static partial Regex TitleRegex();

    [GeneratedRegex(@"from\s+\$(\d+\.\d+|\d+)")]
    private static partial Regex PriceRegex();

    public async Task<List<TripAdvisorAttraction>> GetAttractionsAsync(string tripAdvisorUrl)
    {
        List<TripAdvisorAttraction> attractions = [];
        var url = tripAdvisorUrl;

        while (!string.IsNullOrEmpty(url))
        {
            var document = await GetHtmlDocumentAsync(url);

            if (document == null) break;

            var sections = document.DocumentNode.SelectNodes("//div[contains(@class, 'ALtqV')]");

            if (sections != null)
            {
                var tasks = sections.Select(section => FetchSingleAttractionAsync(section));
                var results = await Task.WhenAll(tasks);

                attractions.AddRange(results.Where(result => result != null).Cast<TripAdvisorAttraction>());
            }

            var nextButton = document.DocumentNode.SelectSingleNode("//a[@aria-label='Next page']");
            url = nextButton != null ? BaseUrl + nextButton.Attributes["href"].Value : "";
        }

        return attractions;
    }

    private async Task<TripAdvisorAttraction?> FetchSingleAttractionAsync(HtmlNode section)
    {
        var nameElement = section.SelectSingleNode(".//h3[contains(@class, 'biGQs')]");
        var linkElement = section.SelectSingleNode(".//div[contains(@class, 'alPVI')]/a");

        if (nameElement == null || linkElement == null) return null;
            
        var innerText = nameElement.InnerText.Trim();
        var match = TitleRegex().Match(innerText);

        if (!match.Success) return null;

        var popularity = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        var name = WebUtility.HtmlDecode(match.Groups[2].Value);
        var link = BaseUrl + linkElement.Attributes["href"].Value;

        var details = await GetAttractionDetailsAsync(link);

        if (details.Categories.Any(c => c.Contains("Trips") || c.Contains("Tours")))
        {
            return null;
        }

        return new TripAdvisorAttraction
        {
            Name = name,
            Popularity = popularity,
            TripAdvisorLink = link,
            Details = details
        };
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

    private async Task<TripAdvisorAttractionDetails> GetAttractionDetailsAsync(string attractionUrl)
    {
        var details = new TripAdvisorAttractionDetails();
        var htmlDoc = await GetHtmlDocumentAsync(attractionUrl);

        if (htmlDoc == null) return details;

        details.Categories = GetCategories(htmlDoc);
        details.VisitDuration = GetVisitDuration(htmlDoc);
        details.Price = GetPrice(htmlDoc);
        details.ImageUrl = GetImageUrl(htmlDoc);
        details.Description = GetDescription(htmlDoc);

        return details;
    }

    private static string? GetImageUrl(HtmlDocument htmlDoc)
    {
        var imgElement = htmlDoc.DocumentNode.SelectSingleNode("//li[contains(@class, 'MBoCH')]//img");
        if (imgElement == null) return null;
            
        var imgUrl = imgElement.GetAttributeValue("src", null);
        return !string.IsNullOrEmpty(imgUrl) ? imgUrl.Split('?')[0] : null;
    }

    private static List<string> GetCategories(HtmlDocument htmlDoc)
    {
        var categoryElements = htmlDoc.DocumentNode.SelectNodes("//span[contains(@class, 'eojVo')]");
        return categoryElements?.Select(node => WebUtility.HtmlDecode(node.InnerText)).ToList() ?? [];
    }

    private static decimal? GetPrice(HtmlDocument htmlDoc)
    {
        var priceElement = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'YqMbD')]//div[contains(@class, 'uuBRH')]");
        if (priceElement == null) return null;
            
        var priceText = priceElement.InnerText.Trim();
        var match = PriceRegex().Match(priceText);
        if (!match.Success) return null;
            
        var price = match.Groups[1].Value;
        return decimal.TryParse(price, out var result) ? result : null;
    }

    private static string? GetVisitDuration(HtmlDocument htmlDoc)
    {
        var durationElement = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(text(), 'Duration:')]");
        if (durationElement == null) return null;
            
        var durationText = durationElement.InnerText;
        var splitText = durationText.Split(':');
        return splitText.Length > 1 ? WebUtility.HtmlDecode(splitText[1].Trim()) : null;
    }

    private static string? GetDescription(HtmlDocument htmlDoc)
    {
        var descriptionElement = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'pZUbB')]//span[@class='JguWG']");
        if (descriptionElement == null) return null;
            
        var description = descriptionElement.InnerText.Trim();
        if (string.IsNullOrEmpty(description)) return null;
            
        var lines = description.Split('\n');
        var filteredLines = lines.Where(line => !line.Trim().Contains("Tripadvisor")).Select(line => line.Trim());
        var filteredDescription = string.Join("\n", filteredLines).Trim();
        return WebUtility.HtmlDecode(filteredDescription.Replace("\n\n", "\n"));
    }
}
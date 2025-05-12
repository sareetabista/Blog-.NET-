using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Talktoyeat.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Talktoyeat.UI.Services
{
    public class ContentIntegrityService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ContentIntegrityService> _logger;
        private readonly IConfiguration _configuration;
        
        // API Keys from configuration
      
        
        public ContentIntegrityService(ILogger<ContentIntegrityService> logger, IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _logger = logger;
            _configuration = configuration;
        }
        
        public async Task<ContentVerificationResult> VerifyContent(string content, string title)
        {
            var result = new ContentVerificationResult 
            { 
                IsValid = true,
                Score = 100,
                Issues = new List<string>()
            };
            
            try
            {
                // 1. Check for broken links (free, no API key needed)
                result = await CheckBrokenLinks(content, result);
                
                // 2. Check for factual claims using Google Fact Check API (free)
                result = await CheckFactualClaims(content, result);
                
                // 3. Check for content safety using WebPurify free tier
                result = await CheckContentSafety(content, result);
                
                // 4. Check against OpenAI text classifier (if you have an OpenAI key)
                // Optional based on if you have this key available
                if (!string.IsNullOrEmpty(_configuration["ApiKeys:OpenAI"]))
                {
                    result = await CheckAIGenerated(content, result);
                }
                
                // Calculate final score based on issues found
                result.Score = CalculateFinalScore(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during content verification");
                result.Issues.Add("Verification service error: " + ex.Message);
                result.Score = 50; // Default to medium score on error
            }
            
            return result;
        }

        private async Task<ContentVerificationResult> CheckBrokenLinks(string content, ContentVerificationResult result)
        {
            // Extract URLs from content using regex
            var urls = ExtractUrls(content);
            
            foreach (var url in urls)
            {
                try
                {
                    var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
                    if (!response.IsSuccessStatusCode)
                    {
                        result.Issues.Add($"Broken link detected: {url}");
                        result.IsValid = false;
                    }
                }
                catch
                {
                    result.Issues.Add($"Unable to verify link: {url}");
                    result.IsValid = false;
                }
            }
            
            return result;
        }
        
        private async Task<ContentVerificationResult> CheckFactualClaims(string content, ContentVerificationResult result)
        {
            try
            {
                // Use Google Fact Check API (free)
                var query = HttpUtility.UrlEncode(content.Substring(0, Math.Min(content.Length, 500)));
                
                // API KEY PLACEHOLDER: Replace with your Google Fact Check API key
                var apiKey = "AIzaSyBWgqukcgbebBY4zIa3HYY4Lw1YM1tXAgw";
                if (string.IsNullOrEmpty(apiKey))
                {
                    _logger.LogWarning("Google Fact Check API key not configured");
                    return result;
                }
                
                var response = await _httpClient.GetAsync(
                    $"https://factchecktools.googleapis.com/v1alpha1/claims:search?query={query}&key={apiKey}"
                );
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var factCheckResult = JsonSerializer.Deserialize<JsonDocument>(responseContent);
                    
                    // Check if any claims were found and reported
                    if (factCheckResult.RootElement.TryGetProperty("claims", out var claims) && 
                        claims.GetArrayLength() > 0)
                    {
                        result.Issues.Add("Content contains claims that have been fact-checked. Review for accuracy.");
                        result.IsValid = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking factual claims");
            }
            
            return result;
        }
        
        private async Task<ContentVerificationResult> CheckContentSafety(string content, ContentVerificationResult result)
        {
            try
            {
                // Use WebPurify free tier (1000 free requests/month)
                
                // API KEY PLACEHOLDER: Replace with your WebPurify API key
                var apiKey = "808749ff84a62530786b9b0e2160783f";
                if (string.IsNullOrEmpty(apiKey))
                {
                    _logger.LogWarning("WebPurify API key not configured");
                    return result;
                }
                
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("api_key", apiKey),
                    new KeyValuePair<string, string>("text", content),
                    new KeyValuePair<string, string>("format", "json")
                });
                
                var response = await _httpClient.PostAsync(
                    "https://api1.webpurify.com/services/rest/",
                    formContent
                );
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    
                    // Check if content contains profanity or inappropriate content
                    if (responseContent.Contains("\"found\":") && !responseContent.Contains("\"found\":\"0\""))
                    {
                        result.Issues.Add("Content may contain inappropriate language or material");
                        result.IsValid = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking content safety");
            }
            
            return result;
        }
        
        private async Task<ContentVerificationResult> CheckAIGenerated(string content, ContentVerificationResult result)
        {
            try
            {
                // Optional: Use OpenAI API to check if content is AI-generated
                var openAiKey = _configuration["ApiKeys:OpenAI"];
                if (string.IsNullOrEmpty(openAiKey))
                {
                    return result;
                }
                
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", openAiKey);
                
                var requestBody = JsonSerializer.Serialize(new
                {
                    model = "text-moderation-latest",
                    input = content.Substring(0, Math.Min(content.Length, 1000))
                });
                
                var httpContent = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync(
                    "https://api.openai.com/v1/moderations",
                    httpContent
                );
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonSerializer.Deserialize<JsonDocument>(responseContent);
                    
                    // Check for flagged content
                    if (jsonResponse.RootElement.TryGetProperty("results", out var results) &&
                        results[0].TryGetProperty("flagged", out var flagged) &&
                        flagged.GetBoolean())
                    {
                        result.Issues.Add("Content may contain problematic material according to content moderation");
                        result.IsValid = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking AI content generation");
            }
            
            return result;
        }
        
        private string[] ExtractUrls(string content)
        {
            // Simple regex to extract URLs
            var regex = new System.Text.RegularExpressions.Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            var matches = regex.Matches(content);
            
            return matches.Select(m => m.Value).ToArray();
        }
        
        private int CalculateFinalScore(ContentVerificationResult result)
        {
            // Start with 100 and subtract based on issues
            int score = 100;
            
            // Deduct points based on number and type of issues
            score -= result.Issues.Count * 10;
            
            // Ensure the score stays within 0-100 range
            return Math.Max(0, Math.Min(100, score));
        }
    }
    
    public class ContentVerificationResult
    {
        public bool IsValid { get; set; }
        public int Score { get; set; }
        public List<string> Issues { get; set; }
    }
}
using Blazor_OpenAI.Models;
using System.Text;
using System.Text.Json;

namespace Blazor_OpenAI.Services
{

    /// <summary>
    /// Create a service that will handle the interaction with Azure OpenAI. 
    /// </summary>
    public class QnAServiceClient
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IConfiguration _configuration;

        public QnAServiceClient(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<string?> GetMyAnswersAsync(string question)
        {
            string? answer = null;
            try
            {
                // 1. Make the HTTP POST Request to Authorize the Client
                string? endpoint = _configuration["Deployments:Endpoint"];
                if (string.IsNullOrEmpty(endpoint))
                {
                    throw new InvalidOperationException("Endpoint configuration is missing or empty.");
                }
                var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

                // 2. Add the Authorization header
                string? apiKey = _configuration["Deployments:APIKey"];
                if (string.IsNullOrEmpty(apiKey))
                {
                    throw new InvalidOperationException("API Key is missing or empty.");
                }
                request.Headers.Add("api-key", apiKey);

                // 3. Define a Request body, here the prompt will be the question and the max tokens will be 100
                var body = new
                {
                    messages = new[]
                    {
                            new { role = "user", content = question }
                        },
                    max_tokens = 100,
                    model = "gpt-4-32k" // Ensure this matches the model name supported by the API
                };

                // 4. Define the Request Contents
                request.Content = new StringContent(
                    JsonSerializer.Serialize(body),
                    Encoding.UTF8, "application/json"
                );

                // 5. Make the HTTP Request
                var response = await _httpClient.SendAsync(request);

                // 6. Check for Bad Request and log the error message
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new InvalidOperationException($"Bad Request: {errorResponse}");
                }

                response.EnsureSuccessStatusCode();

                // 7. Read the Response
                var responseBody = await response.Content.ReadAsStringAsync();
                // 8. Deserialize the response
                var responseAnswer = JsonSerializer.Deserialize<AnswerResponse>(responseBody);
                // 9. Process the Response
                if (responseAnswer?.choices != null && responseAnswer.choices.Length > 0 && responseAnswer.choices[0].message?.content != null)
                {
                    answer = responseAnswer.choices[0]?.message?.content;
                }
                else
                {
                    throw new InvalidOperationException("The response does not contain a valid answer.");
                }
            }
            catch (Exception ex)
            {
                answer = $"There is an error occurred while processing your request: {ex.Message}";
            }
            return answer;
        }
    }
}

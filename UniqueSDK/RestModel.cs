using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace UniqueSDK
{
	public class RestModel
	{
        /// <summary>
        /// Makes a post request to the provided URL with a provided object parameter.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
		public static async Task<RestResponse> ExecutePostAsync(
            string url,
            object parameters,
            CancellationToken cancellationToken = default
        )
		{
            var client = new HttpClient();

            string json = System.Text.Json.JsonSerializer.Serialize(
                parameters,
                new System.Text.Json.JsonSerializerOptions
                {
                    // Ignore all default / null properties
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                }
            );

            Console.WriteLine("json: ");

            Console.WriteLine(json);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            // Add headers
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            // TODO: This part can be improved
            try
            {
                var response = await client.PostAsync(url, content, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<RestResponse>(responseContent) ?? new ();
                }
                else
                {
                    Console.WriteLine("Request failed.");
                    Console.WriteLine($"Status Code: {response.StatusCode}");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }

            // TODO: Handle the Exception better
            throw new Exception("Rest request failed");
        }
	}
}


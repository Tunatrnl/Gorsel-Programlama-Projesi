using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Project1
{
    internal class Application
    {
        public static async Task Main()
        {
            string entryDate = "Date: " + DateTime.Now;
            Console.WriteLine("     * Hava Durumu *     "+ entryDate);
            Console.WriteLine("İSTANBUL/ANKARA/İZMİR illerinin bugünkü ve 3 gün sonrasına kadar olan hava durumu");
            Weather istanbulWeather = await GetWeather("https://goweather.herokuapp.com/weather/istanbul");
            Weather ankaraWeather = await GetWeather("https://goweather.herokuapp.com/weather/ankara");
            Weather izmirWeather = await GetWeather("https://goweather.herokuapp.com/weather/izmir");

            Console.WriteLine("--------------------------------------------------------------------------------------------------");
            Console.WriteLine("İstanbul İçin Hava Durumu");
            PrintWeatherInfo(istanbulWeather);
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("Ankara İçin Hava Durumu");
            PrintWeatherInfo(ankaraWeather);
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("İzmir İçin Hava Durumu");
            PrintWeatherInfo(izmirWeather);
        }

        public static async Task<Weather> GetWeather(string apiUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    Weather weather = JsonConvert.DeserializeObject<Weather>(responseData);
                    return weather;
                }
                else
                {
                    Console.WriteLine($"API'den veri alınamıyor. Lütfen tekrar deneyiniz. Durum Kodu: {response.StatusCode}");
                    return null;
                }
            }
        }

        public static void PrintWeatherInfo(Weather weather)
        {
            if (weather != null)
            {
                Console.WriteLine("\n");
                Console.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("dddd"));
                Console.WriteLine($"Rüzgar: {weather.Wind}");
                Console.WriteLine($"Durum: {weather.Description}");
                Console.WriteLine($"Sıcaklık: {weather.Temperature}");
                Console.WriteLine("\n");
                Console.WriteLine("Tahmini Hava Durumu:");

                DateTime currentDate = DateTime.Now;

                foreach (var forecast in weather.Forecast)
                {
                    currentDate = currentDate.AddDays(1);

                    Console.WriteLine($" {currentDate.ToString("dd MMMM dddd")} : {forecast.Wind} - {weather.Description} - {forecast.Temperature}");
                }
            }
        }
    }

    // Hava class
    public class Weather
    {
        public string Wind { get; set; }
        public string Description { get; set; }
        public string Degree { get; set; }

        public string Temperature { get; set; }
        public List<Forecast> Forecast { get; set; }
    }

    // Tahmin class
    public class Forecast
    {
        public string Day { get; set; }
        public string Wind { get; set; }
        public string Description { get; set; }
        public string Degree { get; set; }
        public string Temperature { get; set; } 
    }
}

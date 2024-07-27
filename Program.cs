using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using FireSharp.Config;
using FireSharp.Interfaces;


namespace WeatherApp
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            string apiKey = "000000"; 

            Console.WriteLine("Digite o nome da cidade");
            string cidadesInput = Console.ReadLine();

            string[] cidades = cidadesInput.Split(',');

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    foreach (string cidade in cidades)
                    {
                        
                        string url = $"http://api.openweathermap.org/data/2.5/weather?q={cidade}&appid=10d2cee01e3aa3ad4a82f221a6432995&units=metric";
                        HttpResponseMessage response = await client.GetAsync(url);
                        string responseBody = await response.Content.ReadAsStringAsync();

                        
                        WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(responseBody);

                        
                        Console.WriteLine();
                        Console.WriteLine($"Cidade: {weatherData.Name}");

                        if (weatherData.Main != null)
                        {
                            Console.WriteLine($"Temperatura atual: {weatherData.Main.Temp}°C");
                        }
                        else
                        {
                            Console.WriteLine("Dados de temperatura indisponíveis.");
                        }

                        if (weatherData.Weather.Length > 0)
                        {
                            Console.WriteLine($"Condições climáticas: {weatherData.Weather[0].Description}");
                        }
                        else
                        {
                            Console.WriteLine("Dados de condições climáticas indisponíveis.");
                        }
                    }
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("Erro ao obter os dados de previsão do tempo.");
                }
            }

            Console.ReadLine();
        }
    }

   
    public class WeatherData
    {
        public string Name { get; set; }
        public MainData Main { get; set; }
        public Weather[] Weather { get; set; }
    }

    public class MainData
    {
        public float Temp { get; set; }
    }

    public class Weather
    {
        public string Description { get; set; }
    }
}

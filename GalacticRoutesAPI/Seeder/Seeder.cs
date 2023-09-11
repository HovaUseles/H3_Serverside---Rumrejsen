using GalacticRoutesAPI.Managers;
using GalacticRoutesAPI.Models;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GalacticRoutesAPI.Seeder
{
    public class Seeder
    {
        //private readonly GalacticRouteManager _galacticRouteManager;
        //private readonly ApiKeyManager _apiKeyManager;
        //private readonly RequestManager _requestManager;
        //private readonly SpaceTravelerManager _spaceTravelerManager;

        //public Seeder(
        //    GalacticRouteManager galacticRouteManager,
        //    ApiKeyManager apiKeyManager,
        //    RequestManager requestManager,
        //    SpaceTravelerManager spaceTravelerManager
        //    )
        //{
        //    this._galacticRouteManager = galacticRouteManager;
        //    this._apiKeyManager = apiKeyManager;
        //    this._requestManager = requestManager;
        //    this._spaceTravelerManager = spaceTravelerManager;
        //}

        public void SeedMockDb(
            GalacticRouteManager galacticRouteManager,
            SpaceTravelerManager spaceTravelerManager)
        {

            string folderPath = "Seeder\\JsonFiles";

            var jsonFiles = Directory.EnumerateFiles(folderPath, "*.json");

            foreach (var jsonFilePath in jsonFiles)
            {

                string jsonData = File.ReadAllText(jsonFilePath);
                List<JsonElement> deserializedObject = JsonSerializer.Deserialize<List<JsonElement>>(jsonData);

                switch (jsonFilePath.Split("\\").Last().Replace(".json", ""))
                {
                    case "GalacticRoutes":
                        SeedGalacticRoutes(deserializedObject, galacticRouteManager);
                        break;
                    case "SpaceTravelers":
                        SeedSpaceTravelers(deserializedObject, spaceTravelerManager);
                        break;
                }
            }
        }

        private void SeedGalacticRoutes(List<JsonElement> jsonArray, GalacticRouteManager galacticRouteManager)
        {
            foreach (var jsonObject in jsonArray)
            {
                string durationString = jsonObject.GetProperty("duration").GetString();
                TimeSpan minDuration;
                TimeSpan? maxDuration;
                TryMapDurationStringToTimeSpan(durationString, out minDuration, out maxDuration);

                GalacticRoute route = new GalacticRoute()
                {
                    Name = jsonObject.GetProperty("name").GetString(),
                    StartDestination = jsonObject.GetProperty("start").GetString(),
                    EndDestination = jsonObject.GetProperty("end").GetString(),
                    NavigationPoints = jsonObject.GetProperty("navigationPoints").EnumerateArray().Select(t => t.GetString()).ToArray(),
                    Dangers = jsonObject.GetProperty("dangers").EnumerateArray().Select(t => t.GetString()).ToArray(),
                    FuelUsage = jsonObject.GetProperty("fuelUsage").GetString(),
                    Description = jsonObject.GetProperty("description").GetString(),
                    MinDuration = minDuration,
                    MaxDuration = maxDuration
                };

                galacticRouteManager.Add(route);
            }
        }

        private bool TryMapDurationStringToTimeSpan(string duration, out TimeSpan minTime, out TimeSpan? maxTime)
        {
            string[] splitDuration = duration.Split(' ');
            string valueString = splitDuration[0];
            string type = splitDuration[1];

            int minValue;
            int? maxValue = null;
            maxTime = null;

            // If value is an interval
            if (valueString.IndexOf('-') > -1)
            {
                string[] splitValueString = valueString.Split("-");
                minValue = int.Parse(splitValueString[0]);
                maxValue = int.Parse(splitValueString[1]);
            }
            else
            {
                minValue = int.Parse(valueString);
            }

            switch (type.ToLower())
            {
                case "år":
                    minTime = TimeSpan.FromDays(minValue * 365);
                    if (maxValue != null)
                    {
                        maxTime = TimeSpan.FromDays((int)maxValue * 365);
                    }
                    break;
                case "måneder":
                    minTime = TimeSpan.FromDays(minValue * 30);
                    if (maxValue != null)
                    {
                        maxTime = TimeSpan.FromDays((int)maxValue * 30);
                    }
                    break;
                case "dage":
                    minTime = TimeSpan.FromDays(minValue);
                    if (maxValue != null)
                    {
                        maxTime = TimeSpan.FromDays((int)maxValue);
                    }
                    break;
                default:
                    throw new NotImplementedException($"Duration type not recognized. Type={type}");
            }

            return true;
        }

        private void SeedSpaceTravelers(List<JsonElement> jsonArray, SpaceTravelerManager spaceTravelerManager)
        {
            Random random = new Random();
            foreach (var jsonObject in jsonArray)
            {
                SpaceTraveler spaceTraveler = new SpaceTraveler(
                    name: jsonObject.GetProperty("name").GetString(),
                    isCadet: jsonObject.GetProperty("isCadet").GetBoolean()
                    );

                int numberOfRequest = random.Next(0, 8);
                for (int i = 0; i < numberOfRequest; i++)
                {
                    int timeSinceRequest = random.Next(8, 60 * 4); // 4 hours max
                    spaceTraveler.Requests.Add(new Request
                    {
                        RequestTime = DateTime.Now.AddMinutes(timeSinceRequest * -1)
                    });
                }

                spaceTravelerManager.Add(spaceTraveler);
            }
        }
    }
}

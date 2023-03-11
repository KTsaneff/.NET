namespace Theatre.DataProcessor
{
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres = context.Theatres
                .ToList()
                .Where(t => t.NumberOfHalls >= numbersOfHalls)
                .Where(t => t.Tickets.Count >= 20)
                .Select(t => new
                {
                    t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = decimal.Parse(t.Tickets
                                    .Where(tc => tc.RowNumber >= 1 && tc.RowNumber <= 5)
                                    .Sum(tc => tc.Price).ToString("f2", CultureInfo.InvariantCulture)),
                    Tickets = t.Tickets
                                    .Where(tc => tc.RowNumber >= 1 && tc.RowNumber <= 5)
                                    .Select (tc => new
                                    {
                                        Price =decimal.Parse(tc.Price.ToString("f2", CultureInfo.InvariantCulture)),
                                        tc.RowNumber
                                    })
                                    .OrderByDescending(tc => tc.Price)
                })
                .OrderByDescending(t => t.Halls)
                .ThenBy(t => t.Name)
                .ToArray();

            return JsonConvert.SerializeObject(theatres, Formatting.Indented);
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            var plays = context.Plays
                .ToList()
                .Where(p => p.Rating <= rating)
                .Select(p => new PlayExportDto()
                {
                    Title = p.Title,
                    Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(CultureInfo.InvariantCulture),
                    Duration = p.Duration.ToString("c"),
                    Genre = Enum.GetName(typeof(Genre), p.Genre),
                    Actors = p.Casts
                                .Where(c => c.IsMainCharacter)
                                .Select(c => new ActorExportDto()
                                {
                                    FullName = c.FullName,
                                    MainCharacter = $"Plays main character in '{p.Title}'."
                                })
                                .OrderByDescending(a => a.FullName)
                                .ToArray()
                })
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            using (StringWriter writer = new StringWriter(sb))
            {
                XmlRootAttribute root = new XmlRootAttribute("Plays");
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                XmlSerializer serializer = new XmlSerializer(typeof(PlayExportDto[]), root);

                serializer.Serialize(writer, plays, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}

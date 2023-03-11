namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Plays");
            XmlSerializer serializer = new XmlSerializer(typeof(PlayImportDto[]), root);
            PlayImportDto[] playDtos;


            using (StringReader reader = new StringReader(xmlString))
            {
                playDtos = (PlayImportDto[])serializer.Deserialize(reader);
            };

            List<Play> validPlays = new List<Play>();

            foreach (var pDto in playDtos)
            {
                if (!IsValid(pDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                TimeSpan duration = TimeSpan.ParseExact(pDto.Duration, "c", CultureInfo.InvariantCulture);

                if (duration.Hours < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(!Enum.TryParse(typeof(Genre), pDto.Genre, out var genre))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validPlays.Add(new Play()
                {
                    Title = pDto.Title,
                    Duration = duration,
                    Rating = pDto.Rating,
                    Genre = (Genre)genre,
                    Description = pDto.Description,
                    Screenwriter = pDto.Screenwriter,
                });

                sb.AppendLine(string.Format(SuccessfulImportPlay, pDto.Title, pDto.Genre, pDto.Rating.ToString(CultureInfo.InvariantCulture)));
            }

            context.Plays.AddRange(validPlays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Casts");
            XmlSerializer serializer = new XmlSerializer(typeof(CastImportDto[]), root);
            CastImportDto[] castDtos;


            using (StringReader reader = new StringReader(xmlString))
            {
                castDtos = (CastImportDto[])serializer.Deserialize(reader);
            };

            List<Cast> validCasts = new List<Cast>();

            foreach (var castDto in castDtos)
            {
                if (!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validCasts.Add(new Cast()
                { 
                    FullName = castDto.FullName,
                    IsMainCharacter = castDto.IsMainCharacter,
                    PhoneNumber = castDto.PhoneNumber,
                    PlayId = castDto.PlayId,
                });

                sb.AppendLine(string.Format(SuccessfulImportActor, castDto.FullName, castDto.IsMainCharacter ? "main" : "lesser"));
            }

            context.Casts.AddRange(validCasts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Theatre> validTheatres = new List<Theatre>();

            var projDtos = JsonConvert.DeserializeObject<ProjectionImportDto[]>(jsonString);

            foreach (var pDto in projDtos)
            {
                if (!IsValid(pDto)) 
                { 
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Theatre validTheatre = new Theatre()
                {
                    Director = pDto.Director,
                    Name = pDto.Name,
                    NumberOfHalls = pDto.NumberOfHalls,
                };

                foreach (var tDto in pDto.Tickets)
                {
                    if (!IsValid(tDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    validTheatre.Tickets.Add(new Ticket() 
                    { 
                         PlayId = tDto.PlayId,
                         Price = tDto.Price,
                         RowNumber = tDto.RowNumber
                    });
                }
                validTheatres.Add(validTheatre);
                sb.AppendLine(string.Format(SuccessfulImportTheatre, validTheatre.Name, validTheatre.Tickets.Count));
            }

            context.Theatres.AddRange(validTheatres);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}

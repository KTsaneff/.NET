namespace Footballers.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Coaches");
            XmlSerializer serializer = new XmlSerializer(typeof(CoachImportDto[]), root);

            CoachImportDto[] coachDtos;

            using (StringReader reader = new StringReader(xmlString))
            {
                coachDtos = (CoachImportDto[])serializer.Deserialize(reader);
            };

            List<Coach> validCoaches = new List<Coach>();

            foreach (var cDto in coachDtos)
            {
                if (!IsValid(cDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                string nationality = cDto.Nationality;
                bool isNationalityInvalid = string.IsNullOrEmpty(nationality);

                if (isNationalityInvalid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Coach coach = new Coach()
                {
                    Name = cDto.Name,
                    Nationality = nationality,
                };

                foreach (var fDto in cDto.Footballers)
                {
                    if (!IsValid(fDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime contractStartDate;
                    bool isStartDateValid = DateTime
                        .TryParseExact(fDto.ContractStartDate, "dd/MM/yyyy", CultureInfo
                        .InvariantCulture, DateTimeStyles.None, out contractStartDate);

                    if (!isStartDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime contractEndDate;
                    bool isEndDateValid = DateTime
                        .TryParseExact(fDto.ContractEndDate, "dd/MM/yyyy", CultureInfo
                        .InvariantCulture, DateTimeStyles.None, out contractEndDate);

                    if (!isEndDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (contractStartDate > contractEndDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer f = new Footballer()
                    {
                        Name = fDto.Name,
                        ContractStartDate = contractStartDate,
                        ContractEndDate = contractEndDate,
                        BestSkillType = (BestSkillType)fDto.BestSkillType,
                        PositionType = (PositionType)fDto.PositionType,
                    };

                    coach.Footballers.Add(f);
                }

                validCoaches.Add(coach);
                sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
            }

            context.Coaches.AddRange(validCoaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Team> validTeams = new List<Team>();

            var teamDtos = JsonConvert.DeserializeObject<TeamImportDto[]>(jsonString);

            foreach (var tDto in teamDtos)
            {
                if (!IsValid(tDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Team team = new Team()
                {
                    Name = tDto.Name,
                    Nationality = tDto.Nationality,
                    Trophies = tDto.Trophies
                };

                if (team.Trophies <= 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                foreach(int fId in tDto.Footballers.Distinct())
                {
                    Footballer footballer = context.Footballers.Find(fId);
                    if (footballer == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    team.TeamsFootballers.Add(new TeamFootballer()
                    {
                        Footballer = footballer
                    });
                }

                validTeams.Add(team);
                sb.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
            }
            context.Teams.AddRange(validTeams);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}

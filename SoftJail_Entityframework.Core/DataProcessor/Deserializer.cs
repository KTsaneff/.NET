namespace SoftJail.DataProcessor
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Linq;
    using Data;
    using Microsoft.EntityFrameworkCore.Internal;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;
    using SoftJail.Common;
    using AutoMapper;
    using System.Globalization;
    using System.Xml.Serialization;
    using System.IO;
    using SoftJail.Data.Models.Enums;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportDepartmentWithCellsDto[] deparmentDtos = JsonConvert
                .DeserializeObject<ImportDepartmentWithCellsDto[]>(jsonString);

            ICollection<Department> validDepartments = new List<Department>();
            ICollection<Cell> cells = new List<Cell>();

            foreach (var departmentDto in deparmentDtos)
            {
                if (!IsValid(departmentDto) || !departmentDto.Cells.Any())
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (departmentDto.Cells.Any(c => !IsValid(c) || c.CellNumber < ValidationConstants.CellNumberMinValue || c.CellNumber > ValidationConstants.CellNumberMaxValue))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Department department = new Department()
                {
                    Name = departmentDto.Name,      
                };

                foreach (var cellDto in departmentDto.Cells)
                {
                    Cell cell = Mapper.Map<Cell>(cellDto);
                    department.Cells.Add(cell);
                }

                validDepartments.Add(department);
                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }

            context.Departments.AddRange(validDepartments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportPrisonerWithMailsDto[] prisonerDtos = JsonConvert
                .DeserializeObject<ImportPrisonerWithMailsDto[]>(jsonString);

            ICollection<Prisoner> validPrisoners = new List<Prisoner>();

            foreach (var prisonerDto in prisonerDtos)
            {
                if (!IsValid(prisonerDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (prisonerDto.Mails.Any(mailDto => !IsValid(mailDto)))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                bool isIncarcerationDateValid = DateTime.TryParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime incarcerationDate);

                if (!isIncarcerationDateValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime? releaseDate = null;
                if (!string.IsNullOrEmpty(prisonerDto.ReleaseDate))
                {
                    bool isReleaseDateValid = DateTime.TryParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDateValue);

                    if (!isReleaseDateValid)
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    releaseDate = releaseDateValue;
                }

                Prisoner prisoner = new Prisoner()
                {
                    FullName = prisonerDto.FullName,
                    Nickname = prisonerDto.Nickname,
                    Age = prisonerDto.Age,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = releaseDate,
                    Bail = prisonerDto.Bail,
                    CellId = prisonerDto.CellId
                };

                foreach (var mailDto in prisonerDto.Mails)
                {
                    Mail mail = Mapper.Map<Mail>(mailDto);
                    prisoner.Mails.Add(mail);
                }

                validPrisoners.Add(prisoner);
                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(validPrisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Officers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportOfficerWithPrisonersDto[]), xmlRoot);

            using StringReader reader = new StringReader(xmlString);

            ImportOfficerWithPrisonersDto[] officersDtos = (ImportOfficerWithPrisonersDto[])xmlSerializer
                .Deserialize(reader);

            ICollection<Officer> validOfficers = new List<Officer>();

            foreach (var officerDto in officersDtos)
            {
                if (!IsValid(officerDto))
                {
                    sb.AppendLine($"Invalid Data");
                    continue;
                }

                bool isPositionEnumValid = Enum.TryParse(typeof(Position), officerDto.Position, out object positionObj);

                if (!isPositionEnumValid)
                {
                    sb.AppendLine($"Invalid Data");
                    continue;
                }

                bool isWeaponEnumValid = Enum.TryParse(typeof(Weapon), officerDto.Weapon, out object weaponObj);

                if (!isWeaponEnumValid)
                {
                    sb.AppendLine($"Invalid Data");
                    continue;
                }

                Officer officer = new Officer()
                {
                    FullName = officerDto.FullName,
                    Salary = officerDto.Salary,
                    Position = (Position)positionObj,
                    Weapon = (Weapon)weaponObj,
                    DepartmentId = officerDto.DepartmentId
                };

                foreach (var prisonerDto in officerDto.Prisoners)
                {
                    OfficerPrisoner officerPrisoner = new OfficerPrisoner()
                    {
                        Officer = officer,
                        PrisonerId = prisonerDto.Id
                    };

                    officer.OfficerPrisoners.Add(officerPrisoner);
                }

                validOfficers.Add(officer);
                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
            }

            context.Officers.AddRange(validOfficers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
};
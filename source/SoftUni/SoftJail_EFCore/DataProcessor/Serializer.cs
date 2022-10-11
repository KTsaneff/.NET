namespace SoftJail.DataProcessor
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(p => ids.Contains(p.Id))
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers.Select(po => new
                    {
                        OfficerName = po.Officer.FullName,
                        Department = po.Officer.Department.Name,
                    })
                    .OrderBy(o => o.OfficerName).ToArray(),
                    TotalOfficerSalary = decimal.Parse(p.PrisonerOfficers.Sum(s => s.Officer.Salary).ToString("f2"))
                }).OrderBy(p => p.Name).ThenBy(p => p.Id).ToArray();

            string json = JsonConvert.SerializeObject(prisoners, Formatting.Indented);
            return json;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            string[] prisonerNamesArray = prisonersNames.Split(",");

            ExportPrisonerDto[] prisoners = context.Prisoners
                .Where(p => prisonerNamesArray.Contains(p.FullName))
                .ProjectTo<ExportPrisonerDto>(Mapper.Configuration)
                .OrderBy(p => p.FullName).ThenBy(p => p.Id)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            XmlRootAttribute root = new XmlRootAttribute("Prisoners");
            XmlSerializer serializer = new XmlSerializer(typeof(ExportPrisonerDto[]), root);

            serializer.Serialize(writer, prisoners, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
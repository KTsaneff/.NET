using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data.Models
{
    public class Person
    {
        public Person()
        {
            this.Pets = new List<Dog>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public List<Dog> Pets { get; set; }
    }
}



using Dapper.Contrib.Extensions;

namespace Dapper_Learn.Models
{
    [Table("Companies")]
    public class Company
    {
        //NOTICE
        // Key Should be Of Dapper.Contrib.Extensions Name Space Not System.ComponentModel.DataAnnotations
        [Key]
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }

        [Write(false)]
        public virtual ICollection<Employee>? Employees { get; set; }
    }
}

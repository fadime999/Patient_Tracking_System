using Microsoft.AspNetCore.Mvc.ModelBinding;
using PostgreSQL.Data;

namespace Patient_Tracking_System.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string IdentityNo { get; set; }
        public string NameSurname { get; set; }
        public DateOnly BirthDate { get; set; }

       // [BindNever]
//        public ICollection<Visit> Visits { get; set; }

    }
}

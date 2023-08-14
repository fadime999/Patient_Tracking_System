using System.ComponentModel.DataAnnotations;

namespace Patient_Tracking_System.Models
{
    public class Visit
    {
        [Key]
        public int VisitId { get; set; }
        public int PatientId { get; set; }
        public DateOnly VisitDate { get; set; }
        public string DoctorName { get; set; }
        public string Complaint { get; set; }
        public string TreatmentModalities { get; set; }

        public Patient Patient { get; set; }
    }
}

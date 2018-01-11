using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSome.Utilities
{
    public class TreatmentModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TreatmentTypeId { get; set; }
        public int HospitalId { get; set; }
        public DateTime TreatmentDate { get; set; }
        public int TreatmentPhase { get; set; }
        public bool IsInterested { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
using PDR.PatientBooking.Data.Models;
using PDR.PatientBooking.Service.Enums;
using System;

namespace PDR.PatientBooking.Service.BookingServices.Requests
{
    public class AddBookingRequest
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public virtual long PatientId { get; set; }
        public virtual long DoctorId { get; set; }

        public BookingStatus BookingStatus { get; set; }

    }
}

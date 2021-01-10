using PDR.PatientBooking.Data.Models;
using PDR.PatientBooking.Service.Enums;
using System.Collections.Generic;
using System;

namespace PDR.PatientBooking.Service.BookingServices.Responses
{

    public class GetPatientNextAppointmentResponse
    {
        public List<Booking> Bookings { get; set; }

        public class Booking
        {
            public Guid Id { get; set; }
            public virtual long DoctorId { get; set; }

            public virtual long PatientId { get; set; }

            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }




        }
    }
}

using PDR.PatientBooking.Data.Models;
using PDR.PatientBooking.Service.Enums;
using System;

namespace PDR.PatientBooking.Service.BookingServices.Requests
{
    public class CancelBookingRequest
    {
        public Guid Id { get; set; }

    }
}

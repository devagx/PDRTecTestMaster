﻿using PDR.PatientBooking.Service.BookingServices.Requests;
using PDR.PatientBooking.Service.BookingServices.Responses;
using PDR.PatientBooking.Data.Models;

namespace PDR.PatientBooking.Service.BookingServices
{
    public interface IBookingService
    {
        void AddBooking(AddBookingRequest request);

        bool CancelBooking(CancelBookingRequest request);

        GetPatientNextAppointmentResponse GetPatientNextAppointment(long identificationNumber);

        GetAllBookingsResponse GetAllBookings();


    }
}
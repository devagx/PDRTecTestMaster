using Microsoft.EntityFrameworkCore;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Service.Enums;
using PDR.PatientBooking.Data.Models;
using PDR.PatientBooking.Service.BookingServices.Requests;
using PDR.PatientBooking.Service.BookingServices.Responses;
using PDR.PatientBooking.Service.BookingServices.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDR.PatientBooking.Service.BookingServices
{
    public class BookingService : IBookingService
    {
        private readonly PatientBookingContext _context;
        private readonly IAddBookingRequestValidator _validator;

        public BookingService(PatientBookingContext context, IAddBookingRequestValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public void AddBooking(AddBookingRequest request)
        {
            var validationResult = _validator.ValidateRequest(request);

            if (!validationResult.PassedValidation)
            {
                throw new ArgumentException(validationResult.Errors.First());
            }

            
            _context.Order.Add(new Order
            {
                Id = request.Id,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                PatientId = request.PatientId,
                DoctorId = request.DoctorId
            });


            
            _context.SaveChanges();
        }
        

        public GetPatientNextAppointmentResponse GetPatientNextAppointment(long identificationNumber)
        {

            var bookings = _context
                .Order
                .Select(x => new GetPatientNextAppointmentResponse.Booking
                {
                    Id = x.Id,
                    DoctorId = x.DoctorId,
                    StartTime = x.StartTime,
                    PatientId = x.PatientId,
                    EndTime = x.EndTime
                }).Where(x => x.PatientId == identificationNumber && x.StartTime > DateTime.Now).OrderBy(x => x.StartTime).Take(1)
                .AsNoTracking()
                .ToList();

            return new GetPatientNextAppointmentResponse
            {
                Bookings = bookings
            };
        }

    }
}

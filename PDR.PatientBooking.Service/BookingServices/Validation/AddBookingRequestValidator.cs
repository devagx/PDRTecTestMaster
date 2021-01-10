using PDR.PatientBooking.Data;
using PDR.PatientBooking.Service.BookingServices.Requests;
using PDR.PatientBooking.Service.Validation;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PDR.PatientBooking.Service.BookingServices.Validation
{
    public class AddBookingRequestValidator : IAddBookingRequestValidator
    {
        private readonly PatientBookingContext _context;

        public AddBookingRequestValidator(PatientBookingContext context)
        {
            _context = context;
        }

        public PdrValidationResult ValidateRequest(AddBookingRequest request)
        {
            var result = new PdrValidationResult(true);

            if (MissingRequiredFields(request, ref result))
                return result;

            if (InvalidBooking(request, ref result))
                return result;

            return result;
        }

        public bool MissingRequiredFields(AddBookingRequest request, ref PdrValidationResult result)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(request.Id.ToString()))
                errors.Add("Id must be populated");

            if (errors.Any())
            {
                result.PassedValidation = false;
                result.Errors.AddRange(errors);
                return true;
            }

            return false;
        }

        public bool InvalidBooking(AddBookingRequest request, ref PdrValidationResult result)
        {
            try
            {
                //You can not make a booking in the past
                if (request.StartTime <= DateTime.Now || request.EndTime <= DateTime.Now)
                {
                    result.PassedValidation = false;
                    result.Errors.Add("You can not make bookings in the past");
                    return true;
                }

                //Get all bookings
                var bookings = _context.Order.OrderBy(x => x.StartTime).ToList();

                if (bookings.Where(x => x.DoctorId == request.DoctorId).Count() == 0)
                {
                    //Doctor does not have any bookings, so OK to create booking
                    return false;
                }
                else
                {
                    //Get all bookings against given doctor
                    var bookings2 = bookings.Where(
                        x => x.DoctorId == request.DoctorId &&
                        (
                            (request.StartTime >= x.StartTime && request.StartTime <= x.EndTime) ||
                            (request.EndTime >= x.StartTime && request.EndTime <= x.EndTime)
                            )
                        );

                    if (bookings2.Count() > 0)
                    {
                        //Doctor is busy during the selected period
                        result.PassedValidation = false;
                        result.Errors.Add("Doctor is busy in selected time period");
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }


        /*
        private bool BookingAlreadyInDb(AddBookingRequest request, ref PdrValidationResult result)
        {
            if (_context.Order.Any(x => x.Id == request.))
            {
                result.PassedValidation = false;
                result.Errors.Add("A Booking with that name already exists");
                return true;
            }

            return false;
        }
        */
    }
}

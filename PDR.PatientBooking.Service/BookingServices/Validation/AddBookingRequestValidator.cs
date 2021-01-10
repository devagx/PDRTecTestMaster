using PDR.PatientBooking.Data;
using PDR.PatientBooking.Service.BookingServices.Requests;
using PDR.PatientBooking.Service.Validation;
using System.Collections.Generic;
using System.Linq;
//devag
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

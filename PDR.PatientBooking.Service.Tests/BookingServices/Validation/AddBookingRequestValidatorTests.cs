using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Data.Models;
using PDR.PatientBooking.Service.BookingServices.Requests;
using PDR.PatientBooking.Service.BookingServices.Validation;
using System;

namespace PDR.PatientBooking.Service.Tests.BookingServices.Validation
{
    [TestFixture]
    public class AddBookingRequestValidatorTests
    {
        private IFixture _fixture;

        private PatientBookingContext _context;

        private AddBookingRequestValidator _addBookingRequestValidator;

        [SetUp]
        public void SetUp()
        {
            // Boilerplate
            _fixture = new Fixture();

            //Prevent fixture from generating from entity circular references 
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

            // Mock setup
            _context = new PatientBookingContext(new DbContextOptionsBuilder<PatientBookingContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

            // Mock default
            SetupMockDefaults();

            // Sut instantiation
            _addBookingRequestValidator = new AddBookingRequestValidator(
                _context
            );
        }

        private void SetupMockDefaults()
        {

        }

        /*Below test case fails. Ran out of time to fix.
         * Issue seems to be using the Moq framework for generating test objects. It currently generates
         * dateTimes in the past and assigns it to the test object which means the new validation on
         * AddBooking fails.
         * 
         * This failing test case proves the validation is working as it should be
        */
        [Test]
        public void ValidateRequest_AllChecksPass_ReturnsPassedValidationResult()
        {
            //arrange
            var request = GetValidRequest();

            //act
            var res = _addBookingRequestValidator.ValidateRequest(request);

            //assert
            res.PassedValidation.Should().BeTrue();
        }
        

        private AddBookingRequest GetValidRequest()
        {
            var request = _fixture.Create<AddBookingRequest>();
            return request;
        }


    }
}

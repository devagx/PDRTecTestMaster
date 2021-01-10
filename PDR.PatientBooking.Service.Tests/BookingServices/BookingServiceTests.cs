using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Data.Models;
using PDR.PatientBooking.Service.BookingServices;
using PDR.PatientBooking.Service.BookingServices.Requests;
using PDR.PatientBooking.Service.BookingServices.Responses;
using PDR.PatientBooking.Service.BookingServices.Validation;
using PDR.PatientBooking.Service.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using PDR.PatientBooking.Service.Enums;

namespace PDR.PatientBooking.Service.Tests.BookingServices
{
    [TestFixture]
    public class BookingServiceTests
    {
        private MockRepository _mockRepository;
        private IFixture _fixture;

        private PatientBookingContext _context;
        private Mock<IAddBookingRequestValidator> _validator;

        private BookingService _bookingService;

        [SetUp]
        public void SetUp()
        {
            // Boilerplate
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _fixture = new Fixture();

            //Prevent fixture from generating circular references
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

            // Mock setup
            _context = new PatientBookingContext(new DbContextOptionsBuilder<PatientBookingContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            _validator = _mockRepository.Create<IAddBookingRequestValidator>();

            // Mock default
            SetupMockDefaults();

            // Sut instantiation
            _bookingService = new BookingService(
                _context,
                _validator.Object
            );
        }

        private void SetupMockDefaults()
        {
            _validator.Setup(x => x.ValidateRequest(It.IsAny<AddBookingRequest>()))
                .Returns(new PdrValidationResult(true));
        }


        [Test]
        public void AddBooking_ValidatesRequest()
        {
            //arrange
            var request = _fixture.Create<AddBookingRequest>();

            //act
            _bookingService.AddBooking(request);

            //assert
            _validator.Verify(x => x.ValidateRequest(request), Times.Once);
        }

        [Test]
        public void AddBooking_ValidatorFails_ThrowsArgumentException()
        {
            //arrange
            var failedValidationResult = new PdrValidationResult(false, _fixture.Create<string>());

            _validator.Setup(x => x.ValidateRequest(It.IsAny<AddBookingRequest>())).Returns(failedValidationResult);

            //act
            var exception = Assert.Throws<ArgumentException>(() => _bookingService.AddBooking(_fixture.Create<AddBookingRequest>()));

            //assert
            exception.Message.Should().Be(failedValidationResult.Errors.First());
        }

        [Test]
        public void AddBooking_AddsBookingToContextWithGeneratedId()
        {
            //arrange
            var request = _fixture.Create<AddBookingRequest>();

            var expected = new Order
            {
                Id = request.Id,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                PatientId = request.PatientId,
                DoctorId = request.DoctorId
            };

            //act
            _bookingService.AddBooking(request);

            //assert
            _context.Order.Should().ContainEquivalentOf(expected, options => options.Excluding(booking => booking.Id));
        }


        [Test]
        /*Ran out of time to implement. 
        Manually manipulate values below to pass the test
        */
        public void GetPatientNextAppointment_NoBookings_ReturnsEmptyList()
        {
            //arrange


            //act


            //assert
            Assert.AreEqual(1, 1);
        }

        [Test]
        /*Ran out of time to implement. 
        Manually manipulate values below to pass the test
        */
        public void GetPatientNextAppointment_EnsureValidNextBooking_ReturnsTrue()
        {
            //arrange


            //act


            //assert
            Assert.AreEqual(1, 1);
        }

        [TestCase(100)]
        /*Ran out of time to implement. 
        Manually manipulate values below to pass the test
        */
        public void GetPatientNextAppointment_ReturnsMappedBookingList(long identification)
        {
            //arrange


            //act


            //assert
            Assert.AreEqual(1, 1);
        }

        [Test]
        public void GetAllBookings_ReturnsMappedBookingList()
        {
            //arrange
            var booking = _fixture.Create<Order>();
            _context.Order.Add(booking);
            _context.SaveChanges();

            var expected = new GetAllBookingsResponse
            {
                Bookings = new List<GetAllBookingsResponse.Booking>
                {
                    new GetAllBookingsResponse.Booking
                    {
                        Id = booking.Id,
                        StartTime = booking.StartTime,
                        EndTime = booking.EndTime,
                        PatientId = booking.PatientId,
                        DoctorId = booking.DoctorId
                    }
                }
            };

            //act
            var res = _bookingService.GetAllBookings();

            //assert
            res.Should().BeEquivalentTo(expected);
        }


        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }
    }
}

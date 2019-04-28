using System;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    internal class IT3_CookController
    {
        private CookController _uut;
        private IUserInterface _userInterface;
        private ITimer _timer;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private IOutput _output;

        [SetUp]
        void Init()
        {
            _userInterface = Substitute.For<IUserInterface>();                      // Substituting Interface
            _timer = Substitute.For<ITimer>();                                      // Substituting Timer

            _output = Substitute.For<IOutput>();                                    // Substituting Output
            _display = new Display(_output);                                        // Using real Display

            _powerTube = new PowerTube(_output);                                    // Using real PowerTube
            _uut = new CookController(_timer,_display,_powerTube,_userInterface);   // UUT Constructor.
        }

        // Zero
        [TestCase(0,0)]

        // Power Test 
        [TestCase(1,0)]
        [TestCase(100,0)]

        // Time Test
        [TestCase(0,60)]
        [TestCase(0,90)]
        [TestCase(0,30)]
        void CookController_Running_OnTimerTick_WithReal_PowerTube(int power, int timeinsec)
        {
            _uut.StartCooking(power,timeinsec);

            _timer.TimeRemaining.Returns(timeinsec * 1000);

            _timer.TimerTick += Raise.EventWith(this, EventArgs.Empty);

            string expectedout = $"Display shows: {timeinsec / 60:D2}:{timeinsec / 60:D2}";
            _output.Received(1).OutputLine( Arg.Is<string>(txt => txt == expectedout));
        }
    }
}

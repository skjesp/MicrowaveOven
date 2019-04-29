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
        public void Init()
        {
            _userInterface = Substitute.For<IUserInterface>();                      // Substituting Interface
            _timer = Substitute.For<ITimer>();                                      // Substituting Timer

            _output = Substitute.For<IOutput>();                                    // Substituting Output
            _display = new Display(_output);                                        // Using real Display

            _powerTube = new PowerTube(_output);                                    // Using real PowerTube
            _uut = new CookController(_timer,_display,_powerTube,_userInterface);   // UUT Constructor.
        }

        // Power Test 
        [TestCase(50,0)]
        [TestCase(100,0)]
        [TestCase(550,0)]
        [TestCase(700,0)]
        public void CookController_StartCooking_WithReal_PowerTube(int power, int timeinsec)
        {
            _uut.StartCooking(power,timeinsec);
            string expectedout = $"PowerTube works with {power} W";
            _output.Received(1).OutputLine( Arg.Is<string>(txt => txt == expectedout));
        }

        [Test]
        public void CookController_Running_TimerExpired_PowerTube_TurnsOff()
        {
            _uut.StartCooking(50, 0);
            _timer.Expired += Raise.EventWith(this, EventArgs.Empty);

            string expectedout = $"PowerTube turned off";
            _output.Received(1).OutputLine(Arg.Is<string>(txt => txt == expectedout));
        }

        [Test]
        public void CookController_Running_CookController_Stops_PowerTube_Turns_Off()
        {
            _uut.StartCooking(50, 0);
            _uut.Stop();

            string expectedout = $"PowerTube turned off";
            _output.Received(1).OutputLine(Arg.Is<string>(txt => txt == expectedout));
        }

    }
}

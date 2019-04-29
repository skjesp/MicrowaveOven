﻿using System;
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

        // Zero (OutOfRangeException, da power skal være mellem 1;100)
        [TestCase(50,0)]

        // Power Test 
        [TestCase(50,0)]
        [TestCase(100,0)]
        [TestCase(350,0)]
        [TestCase(700,0)]

        // Time Test
        [TestCase(50,60)]
        [TestCase(50,90)]
        [TestCase(50,30)]
        public void CookController_Running_OnTimerTick_WithReal_PowerTube(int power, int timeinsec)
        {
            _uut.StartCooking(power,timeinsec);
            string expectedout = $"PowerTube works with {power} W";
            _output.Received(1).OutputLine( Arg.Is<string>(txt => txt == expectedout));
        }

        // Zero (OutOfRangeException, da power skal være mellem 1;100)
        [TestCase(50, 0)]

        // Power Test 
        [TestCase(50, 0)]
        [TestCase(100, 0)]
        [TestCase(350, 0)]
        [TestCase(700, 0)]

        // Time Test
        [TestCase(50, 60)]
        [TestCase(50, 90)]
        [TestCase(50, 30)]
        public void CookController_Running_TimerExpired_PowerTube_TurnsOff(int power, int timeinsec)
        {
            _uut.StartCooking(power, timeinsec);
            _timer.Expired += Raise.EventWith(this, EventArgs.Empty);

            string expectedout = $"PowerTube turned off";
            _output.Received(1).OutputLine(Arg.Is<string>(txt => txt == expectedout));
        }

        // Zero (OutOfRangeException, da power skal være mellem 1;100)
        [TestCase(50, 0)]

        // Power Test 
        [TestCase(50, 0)]
        [TestCase(100, 0)]
        [TestCase(350, 0)]
        [TestCase(700, 0)]

        // Time Test
        [TestCase(50, 60)]
        [TestCase(50, 90)]
        [TestCase(50, 30)]
        public void CookController_Running_CookController_Stops_PowerTube_Turns_Off(int power, int timeinsec)
        {
            _uut.StartCooking(power, timeinsec);
            _uut.Stop();

            string expectedout = $"PowerTube turned off";
            _output.Received(1).OutputLine(Arg.Is<string>(txt => txt == expectedout));
        }

    }
}

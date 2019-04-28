using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;

namespace Microwave.Test.Integration
{
    internal class IT2_CookController
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
            _output = NSubstitute.Substitute.For<IOutput>();                            // Substitute output
            _display = new Display( _output );                                          // Create real display

            _userInterface = NSubstitute.Substitute.For<IUserInterface>();              // Substitute UI
            _timer = NSubstitute.Substitute.For<ITimer>();                              // Substitute timer
            _powerTube = NSubstitute.Substitute.For<IPowerTube>();                      // Substitute powerTube

            _uut = new CookController( _timer, _display, _powerTube, _userInterface );  // Create CookController (UUT)
        }


        [Test]
        public void CookController_Running_OnTimerTick__Display_Shows_Time( [Range( -10, 240, 10 )]int timeSec )
        {
            int power = 100;
            // Start CookController with given parameters
            // power should be irrelevant in this case
            _uut.StartCooking( power, timeSec );

            // Set timer to return timeSec as remaining seconds
            _timer.TimeRemaining.Returns( timeSec * 1000 );

            // Raise the TimerTick Event
            _timer.TimerTick += Raise.EventWith( this, EventArgs.Empty );

            // 1. Expect the CookController to retrieve the timers TimeRemaining
            // 2. CookController calls displays.ShowTime(minutes, seconds) 
            // 3. display write timeSec to output 

            // Expected output string
            string expectedOutput = $"Display shows: {timeSec / 60:D2}:{timeSec % 60:D2}";

            // Check output was called once with the correct string format
            _output.Received( 1 ).OutputLine(
                                            Arg.Is<string>(
                                                    txt => txt == expectedOutput
                                                )
                                            );
        }
    }
}

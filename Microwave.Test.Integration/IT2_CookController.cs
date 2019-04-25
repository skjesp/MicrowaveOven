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

        #region CookController_StartCooking_Function
        // Zero
        [TestCase( 0, 0 )]

        // Variable power
        [TestCase( -10, 0 )]
        [TestCase( 10, 0 )]

        // Variable timeSec 
        [TestCase( 0, -1000 )]
        [TestCase( 0, 1000 )]
        public void CookController_StartCooking__Starts_Timer( int power, int time )
        {
            // Start CookController with given parameters
            // power should be irrelevant in this case
            _uut.StartCooking( power, time );

            // Expect tha timer.Start was called ONCE with timeSec as parameter.
            _timer.Received( 1 ).Start(
                Arg.Is<int>( x => x == time )
            );
        }



        // Zero
        [TestCase( 0, 0 )]

        // Variable power
        [TestCase( -10, 0 )]
        [TestCase( 10, 0 )]

        // Variable timeSec 
        [TestCase( 0, -1000 )]
        [TestCase( 0, 1000 )]
        public void CookController_StartCooking__TurnOn_PowerTube( int power, int time )
        {
            // Start CookController with given parameters
            // timer should be irrelevant in this case
            _uut.StartCooking( power, time );

            // Expect tha powerTube.TurnOn was called ONCE with power as parameter.
            _powerTube.Received( 1 ).TurnOn(
                                                Arg.Is<int>( x => x == power )
                                            );
        }
        #endregion

        #region CookController_OnTimerTick 

        // Zero
        [TestCase( 0, 0 )]

        // Variable power
        [TestCase( -10, 0 )]
        [TestCase( 10, 0 )]

        // Variable timeSec 
        [TestCase( 0, -10 )]
        [TestCase( 0, 12 )]
        [TestCase( 0, 120 )]
        [TestCase( 0, 60 )]
        [TestCase( 0, 65 )]
        public void CookController_Running_OnTimerTick__Display_Shows_Time( int power, int timeSec )
        {
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


        // Variable timeSec 
        [TestCase( -10 )]
        [TestCase( 0 )]
        [TestCase( 10 )]
        [TestCase( 60 )]
        [TestCase( 65 )]
        [TestCase( 120 )]
        [TestCase( 125 )]
        public void CookController_Not_Running_OnTimerTick__Nothing_Happens( int timeSec )
        {
            // Make sure CookController is stopped
            // power and timeSec should be irrelevant in this case
            _uut.Stop();

            // Set timer to return timeSec as remaining
            _timer.TimeRemaining.Returns( timeSec * 1000 );

            // Raise the TimerTick Event
            _timer.TimerTick += Raise.EventWith( this, EventArgs.Empty );

            // 1. Expect the CookController to do nothing since its not cooking

            // Check output wasn't called
            _output.DidNotReceiveWithAnyArgs();
        }
        #endregion

        #region CookController_OnTimerExpired 


        #region PowerTube_Test_Integration
        [Test]
        public void CookController_Running_OnTimerExpired__Calls_PowertubeTurnOff()
        {
            // Start CookController
            // Parameters does not matter
            _uut.StartCooking( 10, 10 );

            // Raise the Expired Event
            _timer.Expired += Raise.EventWith( this, EventArgs.Empty );

            // Expect the CookController to call turnOff powerTube once
            _powerTube.Received( 1 );

        }

        [Test]
        public void CookController_Not_Running_OnTimerExpired__Doesnt_Call_PowertubeTurnOff()
        {
            // Stop CookController
            _uut.Stop();

            // Raise the Expired Event
            _timer.Expired += Raise.EventWith( this, EventArgs.Empty );

            // Expect the CookController not call turnOff() powerTube
            _powerTube.DidNotReceive();

        }


        #endregion

        #region Display_Test_Integration

        [Test]
        public void CookController_Running_OnTimerExpired__DisplayCleared()
        {

        }
        #endregion

        #endregion

    }
}

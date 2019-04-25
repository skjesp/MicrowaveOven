using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

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
        public void init()
        {
            _output = NSubstitute.Substitute.For<IOutput>();                            // Substitute output
            _display = new Display( _output );                                          // Create real display

            _userInterface = NSubstitute.Substitute.For<IUserInterface>();              // Substitute UI
            _timer = NSubstitute.Substitute.For<ITimer>();                              // Substitute timer
            _powerTube = NSubstitute.Substitute.For<IPowerTube>();                      // Substitute powerTube

            _uut = new CookController( _timer, _display, _powerTube, _userInterface );  // Create CookController (UUT)
        }


        // Zero
        [TestCase( 0, 0 )]

        // Variable power
        [TestCase( -10, 0 )]
        [TestCase( 10, 0 )]

        // Variable time 
        [TestCase( 0, -10 )]
        [TestCase( 0, 10 )]
        public void StartCooking_StartsTimer( int power, int time )
        {
            // Start CookController with given parameters
            // power shouldn't do anything
            _uut.StartCooking( power, time );

            // Expect tha timer.Start was called ONCE with time as parameter.
            _timer.Received( 1 ).Start(
                                            Arg.Is<int>( x => x == time )
                                        );
        }
    }
}

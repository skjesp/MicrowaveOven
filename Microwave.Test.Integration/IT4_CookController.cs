using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microwave.Test.Integration
{
    class IT4_CookController
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
            _output = Substitute.For<IOutput>();                            // Substitute output
            _display = new Display( _output );                              // Create real display
            _powerTube = new PowerTube( _output );                          // Create real powerTube
            _timer = new Timer();                                           // Create real timer

            _userInterface = Substitute.For<IUserInterface>();              // Substitute UI
            _uut = new CookController( _timer, _display, _powerTube, _userInterface );  // Create CookController (UUT)
        }

        // Check that the display prints to the output the correct amount of times
        // The display should print once every second passed
        [Parallelizable]    // Avoid test to take ages
        [Retry( 2 )]
        [TestCase( 0 )]
        [TestCase( 1 )]
        [TestCase( 2 )]
        [TestCase( 3 )]
        [TestCase( 4 )]
        public void CookController_StartsTimer_DisplayPrints_CorrectAmountOfTimes( int timeSec )
        {
            int power = 50;

            // List to store all the strings that have been outputted
            var outputList = new List<string>();


            _output.When( callInfo => callInfo.OutputLine( Arg.Any<string>() ) )
                .Do( callInfo =>
                {
                    outputList.Add( callInfo.ArgAt<string>( 0 ) );   // Add all the calls parameters to output list
                } );

            // Start CookController with given parameters
            _uut.StartCooking( power, timeSec );

            // Wait for timer to finish
            while ( _timer.TimeRemaining > 0 ) { }

            // Hard copy output list, since it might change afterwards timer is done
            var outputListHardCopy = new List<string>( outputList );

            // Expected output string format
            // Should match the following pattern: "Display shows: 01:10"
            const string expectedOutputRegex = @"(Display shows: )\d\d:\d\d";

            // Count how many times the display was updated
            int displayPrintCount = 0;
            foreach ( var line in outputListHardCopy )
            {
                if ( Regex.IsMatch( line, expectedOutputRegex ) )
                {
                    displayPrintCount++;
                    //Console.WriteLine( $"{displayPrintCount}. {line}" );
                }
            }


            // We expect that the display updates each second
            if ( timeSec == 0 )
            {
                // Display time will never update if timer is set to 0
                Assert.AreEqual( timeSec, displayPrintCount );
            }
            else
            {
                Assert.AreEqual( timeSec - 1, displayPrintCount );
            }
        }
    }
}

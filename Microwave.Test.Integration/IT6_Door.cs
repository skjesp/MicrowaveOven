using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Microwave.Test.Integration
{
    class IT6_Door
    {
        private IUserInterface _userInterface;
        private ICookController _cookController;
        private IDoor _uutDoor;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDisplay _display;
        private ILight _light;
        private IOutput _output;
        private IPowerTube _powerTube;
        private ITimer _timer;


        [SetUp]
        public void Init()
        {
            _timer = new Timer();
            _uutDoor = new Door();

            _output = Substitute.For<IOutput>();
            _display = new Display( _output );
            _powerTube = new PowerTube( _output );
            _light = new Light( _output );
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();

            CookController cookController = new CookController( _timer, _display, _powerTube );
            _userInterface = new UserInterface(
                    _powerButton,
                    _timeButton,
                    _startCancelButton,
                    _uutDoor,
                    _display,
                    _light,
                    cookController
                );

            cookController.UI = _userInterface;
            _cookController = cookController;
        }

        // #### STATE Ready ####

        [Test]
        public void Door_ReadyState_DoorOpens_lightTurnsOn()
        {
            _uutDoor.Open();

            string expectedOutput = "Light is turned on";

            _output.Received( 1 ).OutputLine(
                Arg.Is<string>(
                    txt => txt == expectedOutput
                )
            );
        }

        // #### STATE DoorIsOpen ####
        [Test]
        public void Door_DoorIsOpenState_DoorCloses_lightTurnsOff()
        {
            _uutDoor.Open();
            _output.ClearReceivedCalls();
            _uutDoor.Close();

            string expectedOutput = "Light is turned off";

            _output.Received( 1 ).OutputLine(
                Arg.Is<string>(
                    txt => txt == expectedOutput
                )
            );
        }


        public void GetToCookingStageAndPauseTimer()
        {
            // Current state: Ready
            _powerButton.Pressed += Raise.EventWith( this, EventArgs.Empty );

            // Current state: SetPower
            _timeButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _timeButton.Pressed += Raise.EventWith( this, EventArgs.Empty );

            // Current state: SetTime
            _startCancelButton.Pressed += Raise.EventWith( this, EventArgs.Empty );

            // Current state: Cooking


            // Force timer to stop avoid more timer events
            _timer.Stop();
        }

        // State Cooking
        [Test]
        public void Door_CookingState_DoorCloses_PowerTubeTurnsOff()
        {
            GetToCookingStageAndPauseTimer();

            // We don't care about the output coming from the startup process
            _output.ClearReceivedCalls();

            // List to store all the strings that will be outputted
            var outputList = new List<string>();

            // Add all the calls parameters to output list
            _output.When( callInfo => callInfo.OutputLine( Arg.Any<string>() ) )
                .Do( callInfo =>
                {
                    outputList.Add( callInfo.ArgAt<string>( 0 ) );
                } );

            _uutDoor.Open();

            // Check if that PowerTube turned off
            Assert.That( outputList, Has.Member( "PowerTube turned off" ) );
        }

        [Test]
        public void Door_CookingState_DoorCloses_DisplayClears()
        {
            GetToCookingStageAndPauseTimer();

            // We don't care about the output coming from the startup process
            _output.ClearReceivedCalls();

            // List to store all the strings that will be outputted
            var outputList = new List<string>();

            // Add all the calls parameters to output list
            _output.When( callInfo => callInfo.OutputLine( Arg.Any<string>() ) )
                .Do( callInfo =>
                {
                    outputList.Add( callInfo.ArgAt<string>( 0 ) );
                } );

            _uutDoor.Open();

            // Check if that display cleared
            Assert.That( outputList, Has.Member( "Display cleared" ) );
        }





    }
}

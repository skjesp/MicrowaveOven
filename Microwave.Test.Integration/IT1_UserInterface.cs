﻿using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT1_UserInterface
    {
        private ICookController _cookController;
        private IOutput _output;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;
        private ILight _light;
        private IDisplay _display;
        private IUserInterface _userInterface;

        [SetUp]
        public void init()
        {
            _cookController = Substitute.For<ICookController>();
            _output = Substitute.For<IOutput>();
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();
            _door = Substitute.For<IDoor>();
            _display = new Display( _output );
            _light = new Light( _output );
            _userInterface = new UserInterface( _powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController );
        }

        //*******************
        //**STATE: READY*****
        //*******************
        [Test]
        public void PowerButtonPressed_Ready_CorrectOutput()
        {
            //Arrange - No need to arrange since beginning state is Ready.

            //Act
            _powerButton.Pressed += Raise.EventWith( this, EventArgs.Empty );

            //Assert
            _output.Received( 1 ).OutputLine( Arg.Is<string>( str => str.Contains( "Display shows: 50 W" ) ) );
        }


        //*******************
        //**STATE: SETPOWER**
        //*******************
        [Test]
        public void DoorOpened_SetPower_CorrectOutput()
        {
            //Arrange
            _powerButton.Pressed += Raise.EventWith( this, EventArgs.Empty );

            //Act
            _door.Opened += Raise.EventWith( this, EventArgs.Empty );

            //Assert
            _output.Received( 1 ).OutputLine( Arg.Is<string>( str => str.Contains( "Display cleared" ) ) );
        }

        [Test]
        public void StartCancelButtonPressed_SetPower_CorrectOutput()
        {
            //Arrange
            _powerButton.Pressed += Raise.EventWith( this, EventArgs.Empty );

            //Act
            _startCancelButton.Pressed += Raise.EventWith( this, EventArgs.Empty );

            //Assert
            _output.Received( 1 ).OutputLine( Arg.Is<string>( str => str.Contains( "Display cleared" ) ) );
        }

        [TestCase( 0, 1, "Display shows: 50 W" )]
        [TestCase( 13, 1, "Display shows: 700 W" )]
        [TestCase( 14, 2, "Display shows: 50 W" )]
        public void PowerButtonPressed_SetPower_CorrectOutput( int ButtonPresses, int timesCalled, string Result )
        {
            //Arrange
            _powerButton.Pressed += Raise.EventWith( this, EventArgs.Empty );

            //Act
            for ( int i = 0; i < ButtonPresses; i++ )
            {
                _powerButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            }

            //Assert
            _output.Received( timesCalled ).OutputLine( Arg.Is<string>( str => str.Contains( Result ) ) );
        }

        [Test]
        public void TimeButtonPressed_SetPower_CorrectOutput()
        {
            //Arrange
            _powerButton.Pressed += Raise.EventWith( this, EventArgs.Empty );

            //Act
            _timeButton.Pressed += Raise.EventWith( this, EventArgs.Empty );

            //Assert
            _output.Received( 1 ).OutputLine( Arg.Is<string>( str => str.Contains( "Display shows: 01:00" ) ) );
        }

        //*******************
        //**STATE: SETTIME***
        //*******************
        [TestCase( 1, 1, "Display shows: 02:00" )]
        [TestCase( 98, 1, "Display shows: 99:00" )]
        [TestCase( 99, 1, "Display shows: 100:00" )]
        public void TimeButtonPressed_SetTime_CorrectOutput( int TimeButtonPresses, int timesCalled, string Result )
        {
            //Arrange
            _powerButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _timeButton.Pressed += Raise.EventWith( this, EventArgs.Empty );

            //Act
            for ( int i = 0; i < TimeButtonPresses; i++ )
            {
                _timeButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            }

            //Assert
            _output.Received( timesCalled ).OutputLine( Arg.Is<string>( str => str.Contains( Result ) ) );
        }

        [Test]
        public void DoorOpened_SetTime_CorrectOutput()
        {
            //Arrange
            _powerButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _timeButton.Pressed += Raise.EventWith( this, EventArgs.Empty );

            //Act
            _door.Opened += Raise.EventWith( this, EventArgs.Empty );

            //Assert
            _output.Received( 1 ).OutputLine( Arg.Is<string>( str => str.Contains( "Display cleared" ) ) );
        }

        //*******************
        //**STATE: COOKING**
        //*******************
        [Test]
        public void DoorOpened_Cooking_CorrectOutput()
        {
            //Arrange
            _powerButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _timeButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _startCancelButton.Pressed += Raise.EventWith( this, EventArgs.Empty );

            //Act
            _door.Opened += Raise.EventWith( this, EventArgs.Empty );

            //Assert
            _output.Received( 1 ).OutputLine( Arg.Is<string>( str => str.Contains( "Display cleared" ) ) );
        }

        [Test]
        public void StartCancelButtonPressed_Cooking_CorrectOutput()
        {
            //Arrange
            _powerButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _timeButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _startCancelButton.Pressed += Raise.EventWith( this, EventArgs.Empty );

            //Act
            _startCancelButton.Pressed += Raise.EventWith( this, EventArgs.Empty );

            //Assert
            _output.Received( 1 ).OutputLine( Arg.Is<string>( str => str.Contains( "Display cleared" ) ) );
        }

        [Test]
        public void CookingIsDone_Cooking_CorrectOutput()
        {
            //Arrange
            _powerButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _timeButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _startCancelButton.Pressed += Raise.EventWith( this, EventArgs.Empty );

            //Act
            _userInterface.CookingIsDone();

            //Assert
            _output.Received( 1 ).OutputLine( Arg.Is<string>( str => str.Contains( "Display cleared" ) ) );
        }
    }
}

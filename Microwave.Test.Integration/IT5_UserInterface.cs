using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT5_UserInterface
    {
        private IUserInterface _userInterface;
        private ICookController _cookController;
        private IDoor _door;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDisplay _display;
        private ILight _light;
        private IOutput _output;
        private IPowerTube _powerTube;
        private ITimer _timer;

        [SetUp]
        public void SetUp()
        {
            _door = Substitute.For<IDoor>();
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();
            _output = Substitute.For<IOutput>();
            _display = new Display( _output );
            _light = new Light( _output );
            _powerTube = new PowerTube( _output );
            _timer = Substitute.For<ITimer>();

            CookController cookController = new CookController( _timer, _display, _powerTube );
            _userInterface = new UserInterface( _powerButton, _timeButton,
                _startCancelButton, _door, _display, _light, cookController );

            cookController.UI = _userInterface;
            _cookController = cookController;
        }

        // actually my tests

        [Test]
        public void StartCooker_Timer1_Power50()
        {
            _powerButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _timeButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _startCancelButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _output.Received( 1 ).OutputLine( Arg.Is<string>( str => str.Contains( "PowerTube works with 50 W" ) ) );

        }

        [Test]
        public void StopCooker_DoorOpen()
        {
            _powerButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _timeButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _startCancelButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _door.Opened += Raise.EventWith( this, EventArgs.Empty );
            _output.Received( 1 ).OutputLine( Arg.Is<string>( str => str.Contains( "PowerTube turned off" ) ) );

        }

        [Test]
        public void StopCooker_CancelButton()
        {
            _powerButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _timeButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _startCancelButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _startCancelButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _output.Received( 1 ).OutputLine( Arg.Is<string>( str => str.Contains( "PowerTube turned off" ) ) );
        }

        [Test]
        public void CookerisDone()
        {
            _powerButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _timeButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _startCancelButton.Pressed += Raise.EventWith( this, EventArgs.Empty );
            _timer.Expired += Raise.EventWith( this, EventArgs.Empty );

            _output.Received( 1 ).OutputLine( Arg.Is<string>( str => str.Contains( "Light is turned off" ) ) );

        }
    }
}

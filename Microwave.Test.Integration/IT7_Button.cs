using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Microwave.Test.Integration
{
    [TestFixture]
    internal class IT7_Button
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
            _door = new Door();
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _output = Substitute.For<IOutput>();
            _display = new Display( _output );
            _light = new Light( _output );
            _powerTube = new PowerTube( _output );
            _timer = new Timer();

            CookController cookController = new CookController( _timer, _display, _powerTube );
            _userInterface = new UserInterface( _powerButton, _timeButton,
                _startCancelButton, _door, _display, _light, cookController );

            cookController.UI = _userInterface;
            _cookController = cookController;
        }

        [Test]
        public void PowerButtonPressed()
        {
            _powerButton.Press();

            _output.Received( 1 ).OutputLine( Arg.Is<string>( str => str.Contains( "50 W" ) ) );

        }

        [Test]
        public void TimerButtonPressed()
        {
            _powerButton.Press();
            _timeButton.Press();

            _output.Received( 1 ).OutputLine( Arg.Is<string>( str => str.Contains( "01:00" ) ) );
        }

        [Test]
        public void StartButtonPressed()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            _output.Received( 1 ).OutputLine( Arg.Is<string>( str => str.Contains( "PowerTube works with 50 W" ) ) );
        }
    }
}

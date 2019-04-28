using System;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    class IT8_Output
    {
        private IUserInterface _userInterface;
        private ICookController _cookController;
        private IDoor _door;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private ILight _light;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private Output _uut;

        [SetUp]
        void Init()
        {
            _door = new Door();
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _light = new Light(_uut);
            _display = new Display(_uut);
            _powerTube = new PowerTube(_uut);
            _timer = new Timer();

            _cookController = new CookController
            (
                _timer,
                _display,
                _powerTube,
                _userInterface
            );
            _userInterface = new UserInterface
            (
                _powerButton,
                _timeButton,
                _startCancelButton,
                _door,
                _display,
                _light,
                _cookController
            );
        }

    }
}

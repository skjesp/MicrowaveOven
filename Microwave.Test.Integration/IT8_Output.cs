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
        private IButton _button;
        private ILight _light;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private Output _uut;

        [SetUp]
        void Init()
        {

        }

    }
}

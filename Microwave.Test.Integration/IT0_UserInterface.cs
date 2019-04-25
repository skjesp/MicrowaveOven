using Microsoft.VisualStudio.TestTools.UnitTesting;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT0_UserInterface
    {
        private ICookController _cookController;
        private IOutput _output;
        private IButton _powerbutton = new Button();
        private IButton _timeButton = new Button();
        private IButton _startCancelButton = new Button();
        private IDoor _door = new Door();
        private ILight _light;
        private IDisplay _display;
        private IUserInterface _userInterface;

        [SetUp]
        public void init()
        {
            _cookController = Substitute.For<ICookController>();
            _output = Substitute.For<IOutput>();
            _display = Substitute.For<IDisplay>();
            _light = new Light(_output);
            _userInterface = new UserInterface(_powerbutton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
        }

        [Test]
        public void TurnOn_Ready_TurnOnHasBeenCalledOnce()
        {
            _door.Open();
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void TurnOff_Ready_TurnOffHasBeenCalledOnce()
        {
            _door.Open();
            _door.Close();
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }
    }
}

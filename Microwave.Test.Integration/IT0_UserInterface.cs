using Microsoft.VisualStudio.TestTools.UnitTesting;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT0_UserInterface
    {
        private ICookController _cookController;
        private IOutput _output;
        private IButton _powerbutton;
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
            _powerbutton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();
            _display = Substitute.For<IDisplay>();
            _door = Substitute.For<IDoor>();

            _light = new Light(_output);
            _userInterface = new UserInterface(_powerbutton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
        }

        [Test]
        public void TurnOn_Ready_TurnOnCalledOnce()
        {
            _door.Opened += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void TurnOff_Ready_TurnOffCalledOnce()
        {
            _door.Opened += Raise.EventWith(this, EventArgs.Empty);
            _door.Closed += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        [Test]
        public void TurnOn_SetTime_TurnOnCalledOnce()
        {
            //Arrange
            _powerbutton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            
            //Act
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            //Assert
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void TurnOff_Cooking_TurnOffCalledOnceWhenCookingIsDone()
        {
            //Arrange
            _powerbutton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            //Act
            _userInterface.CookingIsDone();

            //Assert
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        [Test]
        public void TurnOff_Cooking_TurnOffCalledOnceWhenCancelIsPressed()
        {
            //Arrange
            _powerbutton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            //Act
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            //Assert
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }
    }
}

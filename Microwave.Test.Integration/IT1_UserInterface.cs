using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

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
            _display = new Display(_output);
            _light = new Light(_output);
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
        }

        //Testing Lights
        [Test]
        public void TurnOn_Ready_TurnOnCalledOnce()
        {
            _door.Opened += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
        }

        [Test]
        public void TurnOff_Ready_TurnOffCalledOnce()
        {
            _door.Opened += Raise.EventWith(this, EventArgs.Empty);
            _door.Closed += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
        }

        [Test]
        public void TurnOn_SetTime_TurnOnCalledOnce()
        {
            //Arrange
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            //Act
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            //Assert
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
        }

        [Test]
        public void TurnOff_Cooking_TurnOffCalledOnceWhenCookingIsDone()
        {
            //Arrange
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            //Act
            _userInterface.CookingIsDone();

            //Assert
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
        }

        [Test]
        public void TurnOff_Cooking_TurnOffCalledOnceWhenCancelIsPressed()
        {
            //Arrange
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            //Act
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            //Assert
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
        }

        //Testing Display
        [Test]
        public void ShowPower_Ready_PowerIs50()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Display shows: 50 W")));
        }

        [Test]
        public void _SetPower_PowerIs100()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Display shows: 100 W")));
        }

        [Test]
        public void TimeButtonPressed_SetPower_TimeIs1()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("1")));
        }


        [Test]
        public void TimeButtonPressed_SetTime_TimeIs2()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("2")));
        }

        [Test]
        public void DoorOpen_ClearDisplay()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _door.Opened += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("cleared")));
        }


    }
}

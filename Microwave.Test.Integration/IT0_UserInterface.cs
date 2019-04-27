﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void DoorOpened_Ready_CorrectOutput()
        {
            _door.Opened += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
        }

        [Test]
        public void DoorClosed_Ready_CorrectOutput()
        {
            _door.Opened += Raise.EventWith(this, EventArgs.Empty);
            _door.Closed += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
        }

        [Test]
        public void StartCancelButtonPressed_SetTime_CorrectOutput()
        {
            //Arrange
            _powerbutton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            
            //Act
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            //Assert
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
        }

        [Test]
        public void StartCancelButtonPressed_Cooking_CorrectOutput()
        {
            //Arrange
            _powerbutton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            //Act
            _userInterface.CookingIsDone();

            //Assert
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
        }
    }
}

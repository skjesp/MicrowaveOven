﻿using MicrowaveOvenClasses.Interfaces;
using System;

namespace MicrowaveOvenClasses.Boundary
{
    public class PowerTube : IPowerTube
    {
        private IOutput myOutput;

        private bool IsOn = false;

        public PowerTube( IOutput output )
        {
            myOutput = output;
        }

        public void TurnOn( int power )
        {
            if ( power < 50 || 750 < power )
            {
                throw new ArgumentOutOfRangeException( "power", power, "Must be between 50 and 750 W (incl.)" );
            }

            if ( IsOn )
            {
                throw new ApplicationException( "PowerTube.TurnOn: is already on" );
            }

            myOutput.OutputLine( $"PowerTube works with {power} W" );
            IsOn = true;
        }

        public void TurnOff()
        {
            if ( IsOn )
            {
                myOutput.OutputLine( $"PowerTube turned off" );
            }

            IsOn = false;
        }
    }
}
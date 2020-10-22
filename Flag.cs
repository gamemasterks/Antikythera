using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using CTRE.Phoenix.MotorControl;
using CTRE.Phoenix.MotorControl.CAN;
using CTRE.Phoenix.Controller;​
namespace AntikytheraRobotCode
{
    public enum FlagState
    {
        TURN,
        TURN_SLOW,
        IDLE,
        REVERSE,
        INIT,
        UNKNOWN
    }​
   public class Flag
    {​
       private static TalonSRX talon = new TalonSRX(0);
        private static TalonSRX talon2 = new TalonSRX(1);​
       private static FlagState currentFlagState = FlagState.INIT;
        private static FlagState lastFlagState = FlagState.UNKNOWN;​
       private static GameController gamepad = new GameController(new CTRE.Phoenix.UsbHostDevice(0));​
​
       public static void Main()
        {​
           talon.ConfigFactoryDefault();
            talon2.ConfigFactoryDefault();
​
           /* simple counter to print and watch using the debugger */
           int counter = 0;
            bool on = false; // boolean to control on or off
            /* loop forever */
            while (true)
            {​
               if (counter % 50 == 0) // runs code every 50 iterations
                {
                    if (on)
                    {
                        on = false; // set to off
                        talon.Set(ControlMode.PercentOutput, 0.0); // set talon to 0%
                    }
                    else
                    {
                        on = true; // set to on
                        talon.Set(ControlMode.PercentOutput, 1);  // set talon 100%
                    }
                }​
               if (gamepad.GetButton(0)) // is button with id 0 pressed
                {
                    currentFlagState = FlagState.IDLE;
                }
                if (gamepad.GetButton(1))
                {
                    currentFlagState = FlagState.TURN;
                }
                if (gamepad.GetButton(2))
                {
                    currentFlagState = FlagState.TURN_SLOW;
                }
                if (gamepad.GetButton(3))
                {
                    currentFlagState = FlagState.REVERSE;
                }
​
               if (gamepad.GetButton(4))
                {
                    talon2.Set(ControlMode.PercentOutput, 1.0); // set talon to 100%
                }
                else
                {
                    talon2.Set(ControlMode.PercentOutput, 0.0); // set talon to 0%
                }
​
​
​
               if (currentFlagState != lastFlagState) // make code not run every update to have faster run time
                {
                    lastFlagState = currentFlagState; // update the last state for flag spinner
                    switch (currentFlagState) // switch on current state for flag spinner
                    {
                        case FlagState.INIT: // is it in INIT?
                            talon.Set(ControlMode.PercentOutput, 0.0); // set talon to 0%
                            currentFlagState = FlagState.IDLE; // set current state to IDLE
                            break; // leave the switch statement
                        case FlagState.IDLE: // is is in IDLE?
                            talon.Set(ControlMode.PercentOutput, 0.0); // set talon to 0%
                            break;
                        case FlagState.TURN:
                            talon.Set(ControlMode.PercentOutput, 1);  // set talon 100%
                            break;
                        case FlagState.TURN_SLOW:
                            talon.Set(ControlMode.PercentOutput, 0.2);  // set talon 20%
                            break;
                        case FlagState.REVERSE:
                            talon.Set(ControlMode.PercentOutput, -1);  // set talon 100%
                            break;
                        default:
                            talon.Set(ControlMode.PercentOutput, 0.0); // set talon to 0% to prevent broken behavior
                            currentFlagState = FlagState.IDLE; // set state to idle to prevent further errors
                            break;
                    }
                }
​
               /*
               the ifs are the equivalent of the case statement
​
              
               if (currentIntakeState == IntakeState.INIT)
               {
​
               } else if (currentIntakeState == IntakeState.IDLE)
               {
​
               } else if (currentIntakeState == IntakeState.IN)
               {
​
               } else if (currentIntakeState == IntakeState.IN_SLOW)
               {
​
               }
               else if (currentIntakeState == IntakeState.REVERSE)
               {
​
               } else   // this is the equivalent of the default section.
               {
​
               }
​
               */
               /* print the three analog inputs as three columns */
           Debug.Print("Counter Value: " + counter);​
               /* increment counter */
               ++counter; /* try to land a breakpoint here and hover over 'counter' to see it's current value.  Or add it to the Watch Tab */
​
               /* wait a bit */
               System.Threading.Thread.Sleep(100);
​
               // make sure the motors stay on
               CTRE.Phoenix.Watchdog.Feed();
            }
        }
    }
}

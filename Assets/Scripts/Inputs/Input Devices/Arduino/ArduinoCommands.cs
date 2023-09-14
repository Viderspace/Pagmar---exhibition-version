using System.Collections.Generic;

namespace Inputs.Input_Devices.Arduino
{
    public class ArduinoCommands
    {
        public readonly Stack<ProtocolCommand> commands = new();
        public bool IsEmpty => commands.Count == 0;

        public enum CmdType
        {
            RegisterLed = 0,
            ToggleLed = 1,
        }

        #region Inner Class For composing the output massages

        public class ProtocolCommand
        {
            private CmdType commandType;
            private byte ledPin;
            private bool turnOn;

            private const byte REGISTER_CMD = 0;
            private const byte TOGGLE_CMD = 1;

            public ProtocolCommand(CmdType command, byte pin, bool toggle)
            {
                this.commandType = command;
                this.ledPin = pin;
                this.turnOn = toggle;
            }

            public string GetMassage()
            {
                string massage =
                    $"{(commandType == CmdType.RegisterLed ? REGISTER_CMD : TOGGLE_CMD)},{ledPin},{(turnOn ? (byte) 0 : (byte) 1)}";
                return massage;
            }
        }

        #endregion

        public void RegisterLed(byte LEDPin)
        {
            commands.Push(new ProtocolCommand(CmdType.RegisterLed, LEDPin, false));
        }

        public void SetLed(byte LEDPin, bool turnOn)
        {
            commands.Push(new ProtocolCommand(CmdType.ToggleLed, LEDPin, turnOn));
        }

        public string GetNextCommand()
        {
            if (commands.Count == 0)
            {
                return null;
            }

            return commands.Pop().GetMassage();
        }
    }
}

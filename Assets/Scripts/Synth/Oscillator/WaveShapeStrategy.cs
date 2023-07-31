using extOSC;
using Inputs;
using Scriptable_Objects;

namespace Synth.Oscillator
{
    public interface IWaveShape
    {
        void TurnOffOtherButtons();
        OSCMessage GetWaveShapeMessage();
    }

    #region WaveShapeStrategy

    public class WaveShapeStrategy : IWaveShape
    {
        SynthController SynthController => Singleton.Instance.SynthController;
        private readonly WaveShapeBehaviour _waveShapeBehaviour;
        public static WaveShapeButton SquareButton { get; private set; }
        public static WaveShapeButton SineButton { get; private set; }
        public static WaveShapeButton SawtoothButton { get; private set; }
        public static WaveShapeButton TriangleButton { get; private set; }


        public WaveShapeStrategy(SynthController.WaveShape waveShapeType, WaveShapeButton buttonInstance)
        {
            switch (waveShapeType)
            {
                case SynthController.WaveShape .Square:
                    _waveShapeBehaviour = new SquareWave();
                    SquareButton = buttonInstance;
                    break;

                case SynthController.WaveShape.Sine:
                    _waveShapeBehaviour = new SineWave();
                    SineButton = buttonInstance;
                    break;

                case SynthController.WaveShape.Triangle:
                    _waveShapeBehaviour = new TriangleWave();
                    TriangleButton = buttonInstance;
                    break;

                case SynthController.WaveShape.Sawtooth:
                    _waveShapeBehaviour = new SawToothWave();
                    SawtoothButton = buttonInstance;
                    break;
                default:
                    _waveShapeBehaviour = null;
                    break;
            }
        }

        public void TurnOffOtherButtons()
        {
            _waveShapeBehaviour.TurnOffOtherButtons();
        }

        public OSCMessage GetWaveShapeMessage()
        {
            return _waveShapeBehaviour.GetWaveShapeMessage();
        }
    }

    #endregion

    #region WaveShapeBehaviour

    #region Abstract

    public abstract class WaveShapeBehaviour : IWaveShape
    {
        protected string OscAddress = "/waveShape";
        public abstract void TurnOffOtherButtons();

        public abstract OSCMessage GetWaveShapeMessage();
    }

    #endregion

    #region SquareWave

    public class SquareWave : WaveShapeBehaviour
    {
        private static float shapeValue = 1f;

        public override OSCMessage GetWaveShapeMessage()
        {
            var message = new OSCMessage(OscAddress);
            message.AddValue(OSCValue.Float(shapeValue));
            return message;
        }

        public override void TurnOffOtherButtons()
        {
            WaveShapeStrategy.SineButton.TurnButtonOff();
            WaveShapeStrategy.TriangleButton.TurnButtonOff();
            WaveShapeStrategy.SawtoothButton.TurnButtonOff();
        }
    }

    #endregion

    #region SineWave

    public class SineWave : WaveShapeBehaviour
    {
        private static float shapeValue = 0f;

        public override OSCMessage GetWaveShapeMessage()
        {
            var message = new OSCMessage(OscAddress);
            message.AddValue(OSCValue.Float(shapeValue));
            return message;
        }

        public override void TurnOffOtherButtons()
        {
            WaveShapeStrategy.SquareButton.TurnButtonOff();
            WaveShapeStrategy.TriangleButton.TurnButtonOff();
            WaveShapeStrategy.SawtoothButton.TurnButtonOff();
        }
    }

    #endregion

    #region TriangleWave

    public class TriangleWave : WaveShapeBehaviour
    {
        private static float shapeValue = 1 / 3f;

        public override OSCMessage GetWaveShapeMessage()
        {
            var message = new OSCMessage(OscAddress);
            message.AddValue(OSCValue.Float(shapeValue));
            return message;
        }

        public override void TurnOffOtherButtons()
        {
            WaveShapeStrategy.SquareButton.TurnButtonOff();
            WaveShapeStrategy.SineButton.TurnButtonOff();
            WaveShapeStrategy.SawtoothButton.TurnButtonOff();
        }
    }

    #endregion

    #region SawToothWave

    public class SawToothWave : WaveShapeBehaviour
    {
        private static float shapeValue = 2 / 3f;

        public override OSCMessage GetWaveShapeMessage()
        {
            var message = new OSCMessage(OscAddress);
            message.AddValue(OSCValue.Float(shapeValue));
            return message;
        }

        public override void TurnOffOtherButtons()
        {
            WaveShapeStrategy.SquareButton.TurnButtonOff();
            WaveShapeStrategy.SineButton.TurnButtonOff();
            WaveShapeStrategy.TriangleButton.TurnButtonOff();
        }
    }

    #endregion

    #endregion
}
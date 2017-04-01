namespace LaberBot.Xbox360Controller.Device
{
    public struct InputEvent
    {
        public enum InputType : byte
        {
            Button = 0x01,
            Axis = 0x02,
            Init = 0x80
        }

        public enum State : byte
        {
            Released = 0x00,
            Pressed = 0x01
        }
        
        public short Value;

        public InputType Type;

        public byte Number;
    }
}
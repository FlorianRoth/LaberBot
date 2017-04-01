namespace LaberBot.Xbox360Controller
{
    using System;

    public interface IController
    {
        event EventHandler<ControllerEventArgs> KeyPressed;

        void Start();

        void Stop();
    }
}
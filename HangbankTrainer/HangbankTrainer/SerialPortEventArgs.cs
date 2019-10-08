using System;

namespace HangbankTrainer
{
    internal class SerialPortEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public SerialPortEventArgs(string message)
        {
            Message = message; 
        }
    }
}

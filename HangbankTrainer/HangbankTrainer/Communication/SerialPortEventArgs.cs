using System;

namespace HangbankTrainer.Communication
{
    internal class SerialPortEventArgs : EventArgs
    {

        public int Left { get; }
        public int Right { get; }

        public SerialPortEventArgs(string message)
        {
            int i = message.IndexOf(',');
            if (i == -1)
            {
                return;
            }

            bool b1 = int.TryParse(message.Substring(0, i), out int left);
            bool b2 = int.TryParse(message.Substring(i + 1), out int right);
            if (!b1 || !b2)
            {
                return;
            }

            Left = left;
            Right = right;
        }

        public SerialPortEventArgs(int left, int right)
        {
            Left = left;
            Right = right;
        }

        public SerialPortEventArgs()
        {
            Left = 0;
            Right = 0;
        }
    }
}

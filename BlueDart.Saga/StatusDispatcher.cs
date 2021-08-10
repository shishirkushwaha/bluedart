using System;
using System.Threading.Tasks;

namespace BlueDart.Saga
{
    public static class StatusDispatcher
    {
        public static Task Dispatch(string statusMessage)
        {
            return Console.Out.WriteLineAsync(statusMessage);
        }
    }
}
using System;

namespace SimpleTwitchBot {

    public static class Logger {

        public static void log(string message, object source = null) {
            if (source == null) {
                Console.WriteLine(message);
            } else {
                Console.WriteLine($"[{source.GetType().Name}]: {message}");
            }
        }
    }
}
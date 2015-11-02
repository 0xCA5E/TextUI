using System;
using System.Runtime.InteropServices;

namespace TextUI {

    public static class ConsoleManager {

        #region Win32Interop
        private const int SW_SHOWMAXIMIZED = 3;

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static readonly IntPtr _consoleIntPtr = GetConsoleWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        #endregion

        private static int _startingX;
        private static int _startingY;
        private static ConsoleColor _startingForegroundColour;
        private static ConsoleColor _startingBackgroundColour;
        private static int _startingBufferHeight;
        private static int _startingBufferWidth;
        private static readonly object _renderLock = new object();

        public static void DrawColorBlock(ConsoleColor color, int startX, int startY, int endX, int endY) {
            lock (_renderLock) {
                if (Console.BackgroundColor != color)
                    Console.BackgroundColor = color;

                for (var y = startY; y < endY; y++) {
                    Console.CursorTop = y;       // Y pos
                    Console.CursorLeft = startX; // X pos
                    var lineLength = endX - startX;
                    Console.WriteLine("".PadLeft(lineLength));
                }
            }
        }

        public static void WriteText(string text, int startX, int startY, ConsoleColor textColour, ConsoleColor backgroundColour) {
            lock (_renderLock) {
                Console.CursorLeft = startX; // X pos
                Console.CursorTop = startY;  // Y pos

                Console.BackgroundColor = backgroundColour;
                Console.ForegroundColor = textColour;

                Console.Write(text);
            }
        }

        public static void UpdateWindow(int width, int height, ConsoleColor backgroundColor) {
            Console.CursorVisible = false;

            if (width > Console.BufferWidth) //new Width is bigger then buffer
            {
                Console.BufferWidth = width;
                Console.WindowWidth = width;
            } else {
                Console.WindowWidth = width;
                Console.BufferWidth = width;
            }

            if (height > Console.BufferHeight) //new Height is bigger then buffer
            {
                Console.BufferHeight = height;
                Console.WindowHeight = height;
            } else {
                Console.WindowHeight = height;
                Console.BufferHeight = height;
            }

            Console.BackgroundColor = backgroundColor;
            Console.Clear();
        }

        public static void SetWindowTitle(string title) {
            Console.Title = title;
        }

        public static void MaximizeWindow(ConsoleColor bgcolor) {
            var success = ShowWindow(_consoleIntPtr, SW_SHOWMAXIMIZED);
            UpdateWindow(Console.LargestWindowWidth, Console.LargestWindowHeight, bgcolor);
        }

        public static void MaximizeWindow() {
            var success = ShowWindow(_consoleIntPtr, SW_SHOWMAXIMIZED);
            UpdateWindow(Console.LargestWindowWidth, Console.LargestWindowHeight, Console.BackgroundColor);
        }

    }

}
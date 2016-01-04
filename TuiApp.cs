using System;

using TextUI.Windows.Base;

namespace TextUI {

    public class TuiApp {

        public TuiApp(Window rootWindow = null, string title = null, ConsoleColor backgroundColor = ConsoleColor.Black, bool fullscreenMode = false) {

            BackgroundColor = backgroundColor;
            Title = title ?? "TUI App";
            ConsoleManager.SetWindowTitle(Title);

            var success = SetRootWindow(rootWindow);
            if (!success) throw new Exception("Failed to initialize root window");

            if (fullscreenMode)
                Maximize();
            else
                RestoreDown();

            _rootWindow.Run();
        }

        public string Title { get; private set; }
        public ConsoleColor BackgroundColor { get; set; }
        public bool IsFullscreen { get; private set; }
        public int Width {
            get { return Console.WindowWidth; }
        }
        public int Height {
            get { return Console.WindowHeight; }
        }
        private Window _rootWindow;
        private int _marginX;
        private int _marginY;

        /// <summary>
        /// Sets the root window.
        /// </summary>
        /// <param name="window">A window.</param>
        /// <returns>True for success and False if window failed to be set</returns>
        public bool SetRootWindow(Window window) {
            try {
                _rootWindow = window;
                if (_rootWindow != null) {
                    _rootWindow.ContainingApp = this;
                    _rootWindow.ForwordContainingAppDownTuiGraph();
                }
            } catch (Exception ex) {
                return false;
            }

            return true;
        }


        private void Draw() {
            if (_rootWindow == null) return;
            _rootWindow.Draw();
        }

        public void Maximize() {
            if (_rootWindow == null)
                Environment.Exit(1);
            ConsoleManager.MaximizeWindow(BackgroundColor);
            IsFullscreen = true;
            CalculateMargins();
            Draw();
        }

        public void RestoreDown() {
            if (_rootWindow == null)
                Environment.Exit(1);
            ConsoleManager.UpdateWindow(_rootWindow.Width, _rootWindow.Height, BackgroundColor);
            IsFullscreen = false;
            CalculateMargins();
            Draw();
        }

        public void Exit() {
            Environment.Exit(1);
        }

        internal void DrawColorBlock(ConsoleColor backgroundColor, int startX, int startY, int endX, int endY) {

            // Return if start is outside root window's bounds
            if ((startX < 0) || (startY < 0) || (endX < 0) || (endY < 0)) return;
            if ((startX > _rootWindow.Width) || (startY > _rootWindow.Height)) return;

            // Cap end if it spans outside the root window's bounds
            if (endX > _rootWindow.Width)
                endX = _rootWindow.Width;    
            if (endY > _rootWindow.Height)
                endY = _rootWindow.Height;

            // Offset according to the margin (fullscreen apps)
            startX += _marginX;
            startY += _marginY;
            endX += _marginX;
            endY += _marginY;

            ConsoleManager.DrawColorBlock(backgroundColor, startX, startY, endX, endY);
        }

        internal void WirteText(string text, int startX, int startY, ConsoleColor textColor, ConsoleColor backgroundColor) {

            // Check if attempting to draw out of the root window's bounds
            if ((startX < 0) || (startY < 0)) return;
            if (startX >= _rootWindow.Width || startY >= _rootWindow.Height) return;
                
            var textLen = text.Length;
            var offsetStartX = startX + _marginX;
            var offsetStartY = startY + _marginY;
            var rootWindowRightEdgeX = _rootWindow.Width + _marginX;

            // Cap any drawing that spans outside the root window's bounds
            if (offsetStartX + textLen > rootWindowRightEdgeX)
                text = text.Substring(0, rootWindowRightEdgeX - offsetStartX);

            ConsoleManager.WriteText(text, offsetStartX, offsetStartY, textColor, backgroundColor);
        }

        private void CalculateMargins() {

            _marginX = 0;
            _marginY = 0;
            if (_rootWindow == null) return;
            if ((_rootWindow.Width >= Console.WindowWidth) || (_rootWindow.Height >= Console.WindowHeight)) return;
            _marginX = (Console.WindowWidth - _rootWindow.Width)/2;
            _marginY = (Console.WindowHeight - _rootWindow.Height)/2;
        }

    }

}
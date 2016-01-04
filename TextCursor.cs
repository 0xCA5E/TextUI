using System;
using System.Timers;

using TextUI.Structs;
using TextUI.Windows.Base;

namespace TextUI {

    public class TextCursor : IElement {

        public TextCursor(Window parentWindow) {
            ParentWindow = parentWindow;
        }

        public Window ParentWindow { get; private set; }
        public TuiApp ContainingApp { get; internal set; }
        public int Width { get { return _isVisible ? 1 : 0; } }
        public int Height { get { return _isVisible ? 1 : 0; } }
        public Point Origin { get; private set; }
        public Point Center { get { return Origin; } }
        private bool _cursorShow;

        private Timer _blinkTimer;
        private char _letterUnderCursor;
        private ConsoleColor _backgroundColor;
        private ConsoleColor _foregroundColor;
        private bool _isVisible;
        private const char CURSOR_CHAR_S = '█';
        private const ConsoleColor CURSOR_COLOR = ConsoleColor.White;

        public void Draw() {
            if (ContainingApp == null) return;
            ContainingApp.WirteText(_letterUnderCursor.ToString(), Origin.X, Origin.Y, _foregroundColor, _backgroundColor);
        }

        public void PlaceCursor(int x, int y, char letter, ConsoleColor currentTextColor, ConsoleColor currentBackgroundColor) {

            _isVisible = true;
            Origin = new Point { X = x, Y = y };
            _letterUnderCursor = (letter == '\r' || letter == '\n' || letter == ' ') ? CURSOR_CHAR_S : letter;
            _backgroundColor = currentBackgroundColor;
            _foregroundColor = currentTextColor;

            if (_letterUnderCursor == CURSOR_CHAR_S) {
                _foregroundColor = CURSOR_COLOR;
                _backgroundColor = currentBackgroundColor;
            } else {
                _foregroundColor = currentBackgroundColor;
                _backgroundColor = CURSOR_COLOR;
            }

            Draw();

            _blinkTimer = new Timer(500);
            _blinkTimer.Elapsed += BlinkTimerElapsed;
            _blinkTimer.Enabled = true;
        }

        public void RemoveCursor() {

            if (!_isVisible) return;

            _letterUnderCursor = ' ';
            Draw();
            if (_blinkTimer != null)
                _blinkTimer.Dispose();
            if (_isVisible)
                _isVisible = false;
        }

        private void BlinkTimerElapsed(object sender, ElapsedEventArgs e) {

            _cursorShow = !_cursorShow;
            if (!_isVisible) return;

            var saveBackgroundColor = _backgroundColor;
            var saveForegroundColor = _foregroundColor;

            if (_cursorShow) {
                if (_letterUnderCursor == CURSOR_CHAR_S) {
                    _foregroundColor = CURSOR_COLOR;
                } else {
                    _foregroundColor = _backgroundColor;
                    _backgroundColor = CURSOR_COLOR;
                }
            } else {
                if (_letterUnderCursor == CURSOR_CHAR_S) {
                    _letterUnderCursor = ' ';
                }
                //} else {
                //    ConsoleManager.WriteText(_letterUnderCursor.ToString(), _x, _y, _foregroundColor, _backgroundColor);
                //}
                    
            }

            Draw();

            _backgroundColor = saveBackgroundColor;
            _foregroundColor = saveForegroundColor;
        }

    }

}
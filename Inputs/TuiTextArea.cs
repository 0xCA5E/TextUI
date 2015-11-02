using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Inputs {

    public class TuiTextArea : TuiControl {

        public TuiTextArea(int x, int y, int width, int height, string id, TuiWindow parentWindow) 
            : base(x, y, height, width, parentWindow, id) {
            IsSelectable = true;
            TextCursor = new TuiTextCursor(parentWindow);
        }

        private int CursorDisplayX {
            get { return _cursorDisplayX; }
            set {
                _cursorDisplayX = value;
                SetOffset();
            }
        }
        private string Text {
            get { return _text; }
            set {
                if (OnChange != null && _text != value)
                    OnChange();

                _text = value;

                _splitText = CreateSplitText();
            }
        }
        private string TextWithoutNewLine {
            get { return RemoveNewLine(Text); }
        }
        public ConsoleColor BackgroundColour = ConsoleColor.Blue;
        public Action OnChange;
        private bool _selected;
        private int _cursorPostion;
        private int _cursorDisplayX;
        private int _cursorDisplayY;
        private int _offset;
        private List<string> _splitText = new List<string>();
        private string _text = "";

        private const ConsoleColor TEXT_COLOR = ConsoleColor.White;

        public override void Draw() {
            if (ContainingApp == null) return;

            RemoveCursor();

            UpdateCursorDisplayPostion();

            var lines = _splitText;

            //Draw test area
            for (var i = _offset; i < Height + _offset; i++) {
                var line = ' ' + "".PadRight(Width - 1, ' ');
                if (lines.Count > i)
                    line = ' ' + RemoveNewLine(lines[i]).PadRight(Width - 1, ' ');

                ContainingApp.WirteText(line, i + Origin.X - _offset, Origin.Y, TEXT_COLOR, BackgroundColour);
            }

            if (_selected)
                ShowCursor();

            //Draw Scroll Bar
            ContainingApp.DrawColorBlock(ConsoleColor.White, Origin.X + Width, Origin.Y , Origin.X + Width + 1, Origin.Y  + Height);

            double linesPerPixel = (double) lines.Count()/(Height);
            var postion = 0;
            if (linesPerPixel > 0)
                postion = (int) Math.Floor(_cursorDisplayX/linesPerPixel);

            ContainingApp.WirteText("■", Origin.X + postion, Origin.Y + Width, ConsoleColor.DarkGray, ConsoleColor.White);
        }

        public override void Select() {
            if (!_selected) {
                _selected = true;
                Draw();
            }
        }

        public override void Unselect() {
            if (_selected) {
                _selected = false;
                Draw();
            }
        }

        public override void AddLetter(char letter) {
            string textBefore = Text.Substring(0, _cursorPostion);
            string textAfter = Text.Substring(_cursorPostion, Text.Length - _cursorPostion);

            Text = textBefore + letter + textAfter;

            _cursorPostion++;
            Draw();
        }

        public override void CursorMoveLeft() {
            if (_cursorPostion != 0)
                _cursorPostion--;

            Draw();
        }

        public override void CursorMoveRight() {
            if (_cursorPostion != Text.Length) {
                _cursorPostion++;
                Draw();
            }
        }

        public override void CursorMoveDown() {
            if (_splitText.Count <= CursorDisplayX + 1) //TuiTextCursor at end of text in text area
            {
                ParentWindow.MovetoNextControlDown();
                return;
            }

            var nextLine = _splitText[CursorDisplayX + 1];

            var newCursor = 0;
            for (var i = 0; i < _cursorDisplayX + 1; i++) {
                newCursor += _splitText[i].Count();
            }

            if (nextLine.Count() > _cursorDisplayY)
                newCursor += _cursorDisplayY;
            else
                newCursor += nextLine.Where(x => x != '\n').Count();


            _cursorPostion = newCursor;

            Draw();
        }

        public override void CursorMoveUp() {
            var splitText = _splitText;

            if (0 == CursorDisplayX) //TuiTextCursor at top of text area
            {
                ParentWindow.MovetoNextControlUp();
                return;
            }

            var nextLine = splitText[CursorDisplayX - 1];

            var newCursor = 0;
            for (var i = 0; i < _cursorDisplayX - 1; i++) {
                newCursor += splitText[i].Count();
            }

            if (nextLine.Count() >= _cursorDisplayY)
                newCursor += _cursorDisplayY;
            else
                newCursor += nextLine.Where(x => x != '\n').Count();

            _cursorPostion = newCursor;
            Draw();
        }

        public override void CursorToStart() {
            var splitText = _splitText;

            var newCursor = 0;
            for (var i = 0; i < _cursorDisplayX; i++) {
                newCursor += splitText[i].Count();
            }

            _cursorPostion = newCursor;
            Draw();
        }

        public override void CursorToEnd() {
            var splitText = _splitText;
            var currentLine = splitText[_cursorDisplayX];

            var newCursor = 0;
            for (var i = 0; i < _cursorDisplayX + 1; i++) {
                newCursor += splitText[i].Count();
            }

            _cursorPostion = newCursor - currentLine.Count(x => x == '\n');
            Draw();
        }

        public override void BackSpace() {
            if (_cursorPostion != 0) {
                string textBefore = Text.Substring(0, _cursorPostion);
                string textAfter = Text.Substring(_cursorPostion, Text.Length - _cursorPostion);

                textBefore = textBefore.Substring(0, textBefore.Length - 1);

                Text = textBefore + textAfter;
                _cursorPostion--;
                Draw();
            }
        }

        public override void Enter() {
            AddLetter('\n');
        }

        public void SetText(string text) {
            Text = text;
            _cursorPostion = 0;
            Draw();
        }

        public string GetText() {
            return Text;
        }

        private List<string> CreateSplitText() {
            List<string> splitText = new List<string>();

            var lastSplit = 0;
            for (var i = 0; i < Text.Count() + 1; i++) {
                if (Text.Count() > i && Text[i] == '\n') {
                    splitText.Add(Text.Substring(lastSplit, i - lastSplit + 1));
                    lastSplit = i + 1;
                } else if (i - lastSplit == Width - 2) {
                    splitText.Add(Text.Substring(lastSplit, i - lastSplit));
                    lastSplit = i;
                }

                if (i == Text.Count())
                    splitText.Add(Text.Substring(lastSplit, Text.Count() - lastSplit));
            }

            return splitText.Select(x => x.Replace('\r', ' ')).ToList();
        }

        private void ShowCursor() {
            TextCursor.PlaceCursor(Origin.X + CursorDisplayX - _offset, Origin.Y + 1 + _cursorDisplayY, (Text + ' ')[_cursorPostion], TEXT_COLOR, BackgroundColour);
        }

        private void UpdateCursorDisplayPostion() {
            var lines = _splitText;
            var displayX = 0;
            var displayY = 0;

            for (var i = 0; i < _cursorPostion; i++) {
                if (lines[displayX].Count() > displayY && lines[displayX][displayY] == '\n') //Skip NewLine characters
                {
                    displayY++;
                }

                if (lines.Count > displayX) {
                    if (lines[displayX].Count() > displayY)
                        displayY++;
                    else if (lines.Count - 1 > displayX) {
                        displayX++;
                        displayY = 0;
                    }

                }

                if (displayY == 0 && displayX - 1 >= 0 && lines[displayX - 1].Last() != '\n') //Wordwrap Stuff
                {
                    displayY++;
                } else if (displayY == 1 && displayX - 1 >= 0 && lines[displayX - 1].Last() != '\n') {
                    displayY--;
                }

            }

            CursorDisplayX = displayX;
            _cursorDisplayY = displayY;
        }

        private void RemoveCursor() {
            TextCursor.RemoveCursor();
        }

        private void SetOffset() {
            while (CursorDisplayX - _offset > Height - 1)
                _offset++;

            while (CursorDisplayX - _offset < 0)
                _offset--;
        }

        private string RemoveNewLine(string text) {
            var toReturn = "";

            foreach (var letter in text) {
                if (letter != '\n')
                    toReturn += letter;
            }

            return toReturn;
        }

    }

}
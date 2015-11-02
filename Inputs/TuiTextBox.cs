using System;

using TextUI.Inputs.Base;
using TextUI.Windows.Base;

namespace TextUI.Inputs {

    public class TuiTextBox : TuiControl {

        public TuiTextBox(int x, int y, string id, TuiWindow parentWindow, int length = 38) 
            : base(x, y, 1, length, parentWindow, id) {

            TextCursor = new TuiTextCursor(parentWindow);
            IsSelectable = true;     
            Text = "";
        }

        public TuiTextBox(int x, int y, string text, string id, TuiWindow parentWindow, int length = 38) 
            : base(x, y, 1, length, parentWindow, id) {

            Text = text;

            TextCursor = new TuiTextCursor(parentWindow);
            CursorPostion = text.Length;

            IsSelectable = true;
            Text = "";
        }

        private int CursorPostion {
            get { return _cursorPostion; }
            set {
                _cursorPostion = value;
                SetOffset();
            }
        }
        public bool Selected { get; private set; }
        public string Text { get; private set; }
        private int _cursorPostion;
        private int _offset;
        
        private const ConsoleColor TEXT_COLOR = ConsoleColor.White;
        private const ConsoleColor BACKGROUND_COLOR = ConsoleColor.DarkGray;

        public override void Draw() {
            if (ContainingApp == null) return;

            RemoveCursor();

            string clippedPath;

            if (Selected)
                clippedPath = ' ' + Text.PadRight(Width + _offset, ' ').Substring(_offset, Width - 2);
            else
                clippedPath = ' ' + Text.PadRight(Width, ' ').Substring(0, Width - 2);

            ContainingApp.WirteText(clippedPath + " ", Origin.X, Origin.Y, TEXT_COLOR, BACKGROUND_COLOR);
            if (Selected)
                ShowCursor();
        }

        public override void Select() {
            if (Selected) return;
            Selected = true;
            Draw();
        }

        public override void Unselect() {
            if (!Selected) return;
            Selected = false;
            Draw();
        }

        public override void Enter() {
            ParentWindow.MoveToNextControl();
        }

        public override void AddLetter(char letter) {
            var textBefore = Text.Substring(0, CursorPostion);
            var textAfter = Text.Substring(CursorPostion, Text.Length - CursorPostion);

            Text = textBefore + letter + textAfter;
            CursorPostion++;
            Draw();
        }

        public override void BackSpace() {
            if (CursorPostion != 0) {
                var textBefore = Text.Substring(0, CursorPostion);
                var textAfter = Text.Substring(CursorPostion, Text.Length - CursorPostion);

                textBefore = textBefore.Substring(0, textBefore.Length - 1);

                Text = textBefore + textAfter;
                CursorPostion--;
                Draw();
            }
        }

        public override void CursorMoveLeft() {
            if (CursorPostion != 0) {
                CursorPostion--;
                Draw();
            } else
                ParentWindow.MovetoNextControlLeft();
        }

        public override void CursorMoveRight() {
            if (CursorPostion != Text.Length) {
                CursorPostion++;
                Draw();
            } else
                ParentWindow.MovetoNextControlRight();
        }

        public override void CursorMoveUp() {
            ParentWindow.MovetoNextControlUp();
        }

        public override void CursorToStart() {
            CursorPostion = 0;
            Draw();
        }

        public override void CursorToEnd() {
            CursorPostion = Text.Length;
            Draw();
        }

        public override void CursorMoveDown() {
            ParentWindow.MovetoNextControlDown();
        }

        public string GetText() {
            return Text;
        }

        public void SetText(string text) {
            Text = text;
            Draw();
        }

        private void ShowCursor() {
            var paddedText = Text + " ";
            TextCursor.PlaceCursor(Origin.X + CursorPostion - _offset + 1, Origin.Y, paddedText[CursorPostion], TEXT_COLOR, BACKGROUND_COLOR);
        }

        private void RemoveCursor() {
            TextCursor.RemoveCursor();
        }

        private void SetOffset() {
            while (CursorPostion - _offset > Width - 2)
                _offset++;

            while (CursorPostion - _offset < 0)
                _offset--;
        }

    }

}
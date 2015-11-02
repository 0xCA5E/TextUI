using System;
using System.Linq;

using TextUI.Inputs.Base;
using TextUI.Windows.Base;

namespace TextUI.Inputs {

    public class TuiButton : TuiControl {

        public TuiButton(int x, int y, string text, string id, TuiWindow parentWindow, Action onPress) 
            : base(x, y, 1, text.Count() + 2, parentWindow, id) {
            Text = text;
            IsSelectable = true;
            _onPress = onPress;
        }

        public string Text { get; private set; }
        private readonly Action _onPress;
        private const ConsoleColor BACKGROUND_COLOR = ConsoleColor.Black;
        private const ConsoleColor TEXT_COLOR = ConsoleColor.DarkCyan;
        private const ConsoleColor SELECTED_BACKGROUND_COLOR = ConsoleColor.Black;
        private const ConsoleColor SELECTED_TEXT_COLOR = ConsoleColor.Cyan;        

        public bool Selected { get; private set; }

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
            if (_onPress == null) return;
            _onPress();
        }

        public override void Spacebar() {
            if (_onPress == null) return;
            _onPress();
        }

        public override void Draw() {
            if (ContainingApp == null) return;
            if (Selected)
                ContainingApp.WirteText("<" + Text + ">", Origin.X, Origin.Y, SELECTED_TEXT_COLOR, SELECTED_BACKGROUND_COLOR);
            else
                ContainingApp.WirteText("<" + Text + ">", Origin.X, Origin.Y, TEXT_COLOR, BACKGROUND_COLOR);
        }
        
        public override void CursorMoveDown() {
            ParentWindow.MovetoNextControlDown();
        }

        public override void CursorMoveRight() {
            ParentWindow.MovetoNextControlRight();
        }

        public override void CursorMoveLeft() {
            ParentWindow.MovetoNextControlLeft();
        }

        public override void CursorMoveUp() {
            ParentWindow.MovetoNextControlUp();
        }

    }

}
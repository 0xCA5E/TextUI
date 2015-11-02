using System;

using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows.Base;

namespace ConsoleDraw.Inputs.TuiDropdown
{
    public class TuiDropdownItem : TuiControl
    {

        public TuiDropdownItem(String text, int x, String id, TuiWindow parentWindow) : base(x, parentWindow.Y + 1, 1, parentWindow.Width - 2, parentWindow, id)
        {
            Text = text;

            Selectable = true;
        }

        public String Text = "";
        private ConsoleColor TextColour = ConsoleColor.White;
        private ConsoleColor BackgroudColour = ConsoleColor.DarkGray;
        private ConsoleColor SelectedTextColour = ConsoleColor.Black;
        private ConsoleColor SelectedBackgroundColour = ConsoleColor.Gray;

        private bool Selected = false;
        public Action Action;

        public override void Draw()
        {
            if (ContainingApp == null) return;
            var paddedText = (Text).PadRight(Width, ' ');

            if (Selected)
                ContainingApp.WirteText(paddedText, X, Y, SelectedTextColour, SelectedBackgroundColour);
            else
                ContainingApp.WirteText(paddedText, X, Y, TextColour, BackgroudColour);
        }

        public override void Select()
        {
            if (!Selected)
            {
                Selected = true;
                Draw();

                if (Action != null)
                    Action();
            }
        }

        public override void Unselect()
        {
            if (Selected)
            {
                Selected = false;
                Draw();
            }
        }

        public override void BackSpace()
        {
            ParentWindow.SelectFirstControl();
            ParentWindow.Close();
        }

        public override void CursorMoveDown()
        {
            ParentWindow.MoveToNextControl();
        }
        public override void CursorMoveUp()
        {
            ParentWindow.MoveToLastControl();
        }

        public override void CursorMoveRight()
        {
            ParentWindow.Close();
            ParentWindow.ParentWindow.MoveToNextControl();
        }

        public override void CursorMoveLeft()
        {
            ParentWindow.Close();
            ParentWindow.ParentWindow.MoveToLastControl();
        }
    }
}

using System;

using TextUI.Inputs.Base;
using TextUI.Windows.Base;

namespace TextUI.Inputs.TuiMenu
{
    public class TuiMenuItem : TuiControl
    {

        public TuiMenuItem(String text, String id, TuiWindow parentWindow)
            : base(0, 0, 1, 0, parentWindow, id)
        {
            Text = text; 

            IsSelectable = true;
        }

        private String Text = "";
        private ConsoleColor TextColour = ConsoleColor.White;
        private ConsoleColor BackgroudColour = ConsoleColor.DarkGray;
        private ConsoleColor SelectedTextColour = ConsoleColor.Black;
        private ConsoleColor SelectedBackgroundColour = ConsoleColor.Gray;

        private bool Selected = false;
        public Action Action;

        public override void Draw()
        {
            if (ContainingApp == null) return;
            var paddedText = ('[' +Text + ']').PadRight(Width, ' ');

            if (Selected)
                ContainingApp.WirteText(paddedText, Origin.X, Origin.Y, SelectedTextColour, SelectedBackgroundColour);
            else
                ContainingApp.WirteText(paddedText, Origin.X, Origin.Y, TextColour, BackgroudColour);
        }

        public override void Select()
        {
            if (!Selected)
            {
                Selected = true;
                Draw();

               // new TuiMenuDropdown(X + 1, Y, ParentWindow);
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
            //ParentWindow.ParentWindow.MoveToNextControl();
            ParentWindow.SelectFirstControl();
            ParentWindow.Close();
        }

        public override void Enter()
        {
            if (Action != null)
            {
                ParentWindow.SelectFirstControl();
                Action();
            }
        }

        public override void CursorMoveDown()
        {
            ParentWindow.MoveToNextControl();
        }
        public override void CursorMoveUp()
        {
            ParentWindow.MoveToPrevControl();
        }

        public override void CursorMoveRight()
        {
            ParentWindow.SelectFirstControl();
            ParentWindow.Close();
            ParentWindow.ParentWindow.MoveToNextControl();
        }

        public override void CursorMoveLeft()
        {
            ParentWindow.SelectFirstControl();
            ParentWindow.Close();
            ParentWindow.ParentWindow.MoveToPrevControl();
        }
    }
}

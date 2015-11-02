using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Inputs
{
    public class TuiCheckBox : TuiControl
    {

        public TuiCheckBox(int x, int y, String id, TuiWindow parentWindow) : base(x, y, 1, 3, parentWindow, id)
        {
            BackgroundColour = parentWindow.BackgroundColor;
            Selectable = true;
        }

        public ConsoleColor BackgroundColour = ConsoleColor.Gray;
        private ConsoleColor TextColour = ConsoleColor.Black;

        private ConsoleColor SelectedBackgroundColour = ConsoleColor.DarkGray;
        public bool Checked = false;
        private ConsoleColor SelectedTextColour = ConsoleColor.White;

        private bool Selected = false;

        public Action Action;

        public override void Select()
        {
            if (!Selected)
            {
                Selected = true;
                Draw();
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

        public override void Enter()
        {
            Checked = !Checked; //Toggle Checked

            Draw();

            if (Action != null) //If an action has been set
                Action();
        }

        public override void Draw()
        {
            if (ContainingApp == null) return;
            var Char = Checked ? "X" : " ";

            if(Selected)
                ContainingApp.WirteText('[' + Char + ']', X, Y, SelectedTextColour, SelectedBackgroundColour);
            else
                ContainingApp.WirteText('[' + Char + ']', X, Y, TextColour, BackgroundColour);  
        }

        public override void CursorMoveDown()
        {
            ParentWindow.MovetoNextControlDown(X + 1, Y, Width);
        }

        public override void CursorMoveRight()
        {
            ParentWindow.MovetoNextControlRight(X - 1, Y + Width, 3);

        }

        public override void CursorMoveLeft()
        {
            ParentWindow.MovetoNextControlLeft(X - 1, Y, 3);
        }

        public override void CursorMoveUp()
        {
            ParentWindow.MovetoNextControlUp(X - 1, Y, Width);
        }
    }
}

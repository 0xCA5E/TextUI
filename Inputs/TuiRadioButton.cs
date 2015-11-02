using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Inputs {

    public class TuiRadioButton : TuiControl {

        public TuiRadioButton(int x, int y, String id, String radioGroup, TuiWindow parentWindow) : base(x, y, 1, 3, parentWindow, id) {
            RadioGroup = radioGroup;
            BackgroundColour = parentWindow.BackgroundColor;
            Selectable = true;
        }

        public ConsoleColor BackgroundColour = ConsoleColor.Gray;
        private ConsoleColor TextColour = ConsoleColor.Black;

        private ConsoleColor SelectedBackgroundColour = ConsoleColor.DarkGray;
        private ConsoleColor SelectedTextColour = ConsoleColor.White;

        public Action Action;
        private bool Selected = false;
        public bool Checked = false;
        public String RadioGroup;

        public override void Select() {
            if (!Selected) {
                Selected = true;
                Draw();
            }
        }

        public override void Unselect() {
            if (Selected) {
                Selected = false;
                Draw();
            }
        }

        public override void Enter() {
            if (Checked) //Already checked, no need to change
                return;

            //Uncheck all other Radio Buttons in the group
            ParentWindow.Controls.OfType<TuiRadioButton>().Where(x => x.RadioGroup == RadioGroup).ToList().ForEach(x => x.Uncheck());

            Checked = true;

            Draw();

            if (Action != null) //If an action has been set
                Action();
        }

        public void Uncheck() {
            if (!Checked) //Already unchecked, no need to change
                return;

            Checked = false;
            Draw();
        }

        public override void Draw() {
            if (ContainingApp == null) return;
            var Char = Checked ? "■" : " ";

            if (Selected)
                ContainingApp.WirteText('[' + Char + ']', X, Y, SelectedTextColour, SelectedBackgroundColour);
            else
                ContainingApp.WirteText('[' + Char + ']', X, Y, TextColour, BackgroundColour);
        }

        public override void CursorMoveDown() {
            ParentWindow.MovetoNextControlDown(X + 1, Y, Width);
        }

        public override void CursorMoveRight() {
            ParentWindow.MovetoNextControlRight(X - 1, Y + Width, 3);

        }

        public override void CursorMoveLeft() {
            ParentWindow.MovetoNextControlLeft(X - 1, Y, 3);
        }

        public override void CursorMoveUp() {
            ParentWindow.MovetoNextControlUp(X - 1, Y, Width);
        }

    }

}
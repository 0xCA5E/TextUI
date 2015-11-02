using System;
using System.Collections.Generic;
using System.Linq;

using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows.Base;

namespace ConsoleDraw.Inputs.TuiDropdown
{
    public class TuiDropdown : TuiControl
    {

        public TuiDropdown(int x, int y, List<String> options, String id, TuiWindow parentWindow, int length = 20) : base(x, y, 1, length - 2 + 3, parentWindow, id)
        {
            X = x;
            Y = y;
            Options = options;
            Text = Options.First();
            Length = length;
            BackgroudColour = parentWindow.BackgroundColor;

            Selectable = true;
        }

        private ConsoleColor TextColour = ConsoleColor.Black;
        private ConsoleColor BackgroudColour = ConsoleColor.Gray;
        private ConsoleColor SelectedTextColour = ConsoleColor.White;
        private ConsoleColor SelectedBackgroundColour = ConsoleColor.DarkGray;

        private bool Selected = false;
        public List<TuiDropdownItem> DropdownItems = new List<TuiDropdownItem>();
        public TuiDropdownSpread TuiDropdownSpread;

        private List<String> Options;
        public String Text;
        public int Length;

        public Action OnUnselect;

        public override void Draw()
        {
            if (ContainingApp == null) return;
            var paddedText = Text.PadRight(Length - 2, ' ').Substring(0, Length - 2);

            if (Selected)
                ContainingApp.WirteText('[' + paddedText + '▼' + ']', X, Y, SelectedTextColour, SelectedBackgroundColour);
            else
                ContainingApp.WirteText('[' + paddedText + '▼' + ']', X, Y, TextColour, BackgroudColour);
        }

        public override void Select()
        {
            if (!Selected)
            {
                Selected = true;
                Draw();

                new TuiDropdownSpread(X + 1, Y, Options, ParentWindow, this);
            }
        }

        public override void Unselect()
        {
            if (Selected)
            {
                Selected = false;
                Draw();
                if(OnUnselect != null)
                    OnUnselect();
            }
        }

        
    }
}

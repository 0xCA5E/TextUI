using System;
using System.Linq;

using TextUI.Inputs.Base;
using TextUI.Windows.Base;

namespace TextUI.Inputs {

    public class TuiLabel : TuiControl {

        public TuiLabel(int x, int y, String text, String id, TuiWindow parentWindow) : base(x, y, 1, text.Count(), parentWindow, id) {
            Text = text;
            BackgroundColour = parentWindow.BackgroundColor;
            IsSelectable = false;
        }

        private String Text = "";
        private ConsoleColor TextColour = ConsoleColor.Black;
        public ConsoleColor BackgroundColour = ConsoleColor.Gray;

        public override void Draw() {
            if (ContainingApp == null) return;
            ContainingApp.WirteText(Text, Origin.X, Origin.Y, TextColour, BackgroundColour);
        }

        public void SetText(String text) {
            Text = text;
            Width = text.Count();
            Draw();
        }

    }

}
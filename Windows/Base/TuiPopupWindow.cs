using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Windows.Base
{
    public class TuiPopupWindow : TuiWindow
    {

        public TuiPopupWindow(string title, int x, int y, int width, int height, TuiWindow parentTuiWindow)
            : base(x, y, width, height, parentTuiWindow, ConsoleColor.Gray)
        {
            Title = title;
        }

        protected string Title;

        protected ConsoleColor TitleBarColour = ConsoleColor.DarkGray;
        protected ConsoleColor TitleColour = ConsoleColor.Black;

        public override void Draw()
        {
            if (ContainingApp == null) return;
            base.Draw();
            ContainingApp.DrawColorBlock(TitleBarColour, X, Y, X + 1, Y + Width); //Title Bar
            ContainingApp.WirteText(' ' + Title + ' ', X, Y + 2, TitleColour, BackgroundColor);
        }

    }
}

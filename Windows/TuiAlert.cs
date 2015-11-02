using ConsoleDraw.Inputs;
using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Windows
{
    public class TuiAlert : TuiPopupWindow
    {
        private TuiButton okBtn;
        private static int textLength = 46;


        public TuiAlert(String Message, TuiWindow parentTuiWindow)
            : base("Message", 6, (Console.WindowWidth / 2) - 25, 50, 5 + (int)Math.Ceiling(((Double)Message.Count() / textLength)), parentTuiWindow)
        {
            Init(Message, parentTuiWindow);
        }

        public TuiAlert(String Message, TuiWindow parentTuiWindow, String Title)
            : base(Title, 6, (Console.WindowWidth / 2) - 30, 25, 5 + (int)Math.Ceiling(((Double)Message.Count() / textLength)), parentTuiWindow)
        {
            Init(Message, parentTuiWindow);
        }

        public TuiAlert(String Message, TuiWindow parentTuiWindow, ConsoleColor backgroundColor)
            : base("Message", 6, (Console.WindowWidth / 2) - 25, 50, 5 + (int)Math.Ceiling(((Double)Message.Count() / textLength)), parentTuiWindow)
        {
            BackgroundColor = backgroundColor;

            Init(Message, parentTuiWindow);
        }

        public TuiAlert(String Message, TuiWindow parentTuiWindow, ConsoleColor backgroundColor, String Title)
            : base(Title, 6, (Console.WindowWidth / 2) - 25, 50, 5 + (int)Math.Ceiling(((Double)Message.Count() / textLength)), parentTuiWindow)
        {
            BackgroundColor = backgroundColor;

            Init(Message, parentTuiWindow);
        }

        private void Init(String Message, TuiWindow parentTuiWindow)
        {
            var count = 0;
            while ((count*45) < Message.Count())
            {
                var splitMessage = Message.PadRight(textLength * (count + 1), ' ').Substring((count * textLength), textLength);
                var messageLabel = new TuiLabel(splitMessage, X + 2 + count, Y + 2, "messageLabel", this);
                AddControl(messageLabel);

                count++;
            }

            /*
            var messageLabel = new TuiLabel(Message, X + 2, Y + 2, "messageLabel", this);
            messageLabel.BackgroundColor = BackgroundColor;*/

            okBtn = new TuiButton(X + Height - 2, Y + 2, "OK", "OkBtn", this) {
                Action = delegate { Close(); }
            };

            AddControl(okBtn);

            CurrentlySelected = okBtn;

            Draw();
        }
    }
}

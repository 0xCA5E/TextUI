using ConsoleDraw.Inputs;
using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Windows
{
    public class TuiConfirm : TuiPopupWindow
    {

        public TuiConfirm(TuiWindow parentTuiWindow, String Message, String Title = "TuiConfirm")
            : base(Title, 6, (Console.WindowWidth / 2) - 25, 50, 5 + (int)Math.Ceiling(((Double)Message.Count() / textLength)), parentTuiWindow)
        {
            Create(Message, parentTuiWindow);
        }

        public TuiConfirm(String Message, TuiWindow parentTuiWindow, ConsoleColor backgroundColor, String Title = "Message")
            : base(Title, 6, (Console.WindowWidth / 2) - 25, 50, 5 + (int)Math.Ceiling(((Double)Message.Count() / textLength)), parentTuiWindow)
        {
            BackgroundColor = backgroundColor;

            Create(Message, parentTuiWindow);
        }

        private static int textLength = 46;
        
        private TuiButton okBtn;
        private TuiButton cancelBtn;

        public bool Result { get; private set; }

        private void Create(String Message, TuiWindow parentTuiWindow)
        {
            var count = 0;
            while ((count * 45) < Message.Count())
            {
                var splitMessage = Message.PadRight(textLength * (count + 1), ' ').Substring((count * textLength), textLength);
                var messageLabel = new TuiLabel(splitMessage, X + 2 + count, Y + 2, "messageLabel", this);
                AddControl(messageLabel);

                count++;
            }

            okBtn = new TuiButton(X + Height - 2, Y + 2, "OK", "OkBtn", this);
            okBtn.Action = delegate() { Result = true; Close(); };

            cancelBtn = new TuiButton(X + Height - 2, Y + 8, "Cancel", "cancelBtn", this);
            cancelBtn.Action = delegate() { Close(); };

            AddControl(okBtn);
            AddControl(cancelBtn);

            CurrentlySelected = okBtn;

            Draw();
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

using ConsoleDraw.Windows.Base;

namespace ConsoleDraw.Inputs.TuiDropdown
{
    public class TuiDropdownSpread : TuiWindow
    {

        public TuiDropdownSpread(int Xpostion, int Ypostion, List<String> options, TuiWindow parentTuiWindow, TuiDropdown root)
            : base(Xpostion, Ypostion, 20, options.Count, parentTuiWindow, ConsoleColor.Gray)
        {
            for (var i = 0; i < options.Count(); i++)
            {
                var item = new TuiDropdownItem(options[i], Xpostion + i, "option" + i, this);

                item.Action = delegate() {
                    root.Text = ((TuiDropdownItem)CurrentlySelected).Text;
                    root.Draw();
                };

                DropdownItems.Add(item);
            }

            _controls.AddRange(DropdownItems);

            CurrentlySelected = DropdownItems.FirstOrDefault(x => x.Text == root.Text);

            BackgroundColor = ConsoleColor.DarkGray;
            Draw();
        }

        private List<TuiDropdownItem> DropdownItems = new List<TuiDropdownItem>();
        public TuiDropdown root;

    }
}

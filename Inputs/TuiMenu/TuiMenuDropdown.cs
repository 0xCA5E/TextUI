using System;
using System.Collections.Generic;
using System.Linq;

using TextUI.Structs;
using TextUI.Windows.Base;

namespace TextUI.Inputs.TuiMenu
{
    public class TuiMenuDropdown : TuiWindow
    {

        public TuiMenuDropdown(int Xpostion, int Ypostion, List<TuiMenuItem> menuItems, TuiWindow parentTuiWindow)
            : base(Xpostion, Ypostion, 20, menuItems.Count() + 2, parentTuiWindow, ConsoleColor.Gray)
        {

            for (var i = 0; i < menuItems.Count(); i++)
            {
                menuItems[i].ParentWindow = this;
                menuItems[i].Width = this.Width - 2;
                menuItems[i].Origin = new Point { X = Xpostion + i + 1, Y = this.Origin.Y + 1};
            }

            MenuItems = menuItems;


            _controls.AddRange(MenuItems);
            
            CurrentlySelected = MenuItems.FirstOrDefault();

            BackgroundColor = ConsoleColor.DarkGray;
            Draw();
        }

        private List<TuiMenuItem> MenuItems;
        
    }
}

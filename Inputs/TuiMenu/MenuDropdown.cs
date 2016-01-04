using System;
using System.Collections.Generic;
using System.Linq;

using TextUI.Structs;
using TextUI.Windows.Base;

namespace TextUI.Inputs.TuiMenu
{
    public class MenuDropdown : Window
    {

        public MenuDropdown(int Xpostion, int Ypostion, List<MenuItem> menuItems, Window parentWindow)
            : base(Xpostion, Ypostion, 20, menuItems.Count() + 2, parentWindow, ConsoleColor.Gray)
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

        private List<MenuItem> MenuItems;
        
    }
}

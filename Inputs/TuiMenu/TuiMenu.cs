using System;
using System.Collections.Generic;
using System.Linq;

using TextUI.Inputs.Base;
using TextUI.Structs;
using TextUI.Windows.Base;

namespace TextUI.Inputs.TuiMenu {

    public class TuiMenu : TuiControl {

        private String Text = "";
        private ConsoleColor TextColour = ConsoleColor.Black;
        private ConsoleColor BackgroudColour = ConsoleColor.Gray;
        private ConsoleColor SelectedTextColour = ConsoleColor.White;
        private ConsoleColor SelectedBackgroundColour = ConsoleColor.DarkGray;

        private bool Selected = false;
        public List<TuiMenuItem> MenuItems = new List<TuiMenuItem>();
        public TuiMenuDropdown TuiMenuDropdown;

        public TuiMenu(String text, int x, int y, String id, TuiWindow parentWindow) : base(x, y, 1, text.Count() + 2, parentWindow, id) {
            Text = text;
            Origin = new Point { X = x, Y = y };

            IsSelectable = true;
        }

        public override void Draw() {
            if (ContainingApp == null) return;
            if (Selected)
                ContainingApp.WirteText('[' + Text + ']', Origin.X, Origin.Y, SelectedTextColour, SelectedBackgroundColour);
            else
                ContainingApp.WirteText('[' + Text + ']', Origin.X, Origin.Y, TextColour, BackgroudColour);
        }

        public override void Select() {
            if (!Selected) {
                Selected = true;
                Draw();

                new TuiMenuDropdown(Origin.X + 1, Origin.Y, MenuItems, ParentWindow);

            }
        }

        public override void Unselect() {
            if (Selected) {
                Selected = false;
                Draw();
            }
        }

        public override void Enter() {
            TuiMenuDropdown = new TuiMenuDropdown(Origin.X + 1, Origin.Y, MenuItems, ParentWindow);
        }

        public override void CursorMoveLeft() {
            ParentWindow.MoveToPrevControl();
        }

        public override void CursorMoveRight() {
            ParentWindow.MoveToNextControl();
        }

        public override void CursorMoveDown() {
            TuiMenuDropdown = new TuiMenuDropdown(Origin.X + 1, Origin.Y, MenuItems, ParentWindow);
        }

    }

}
using System;

using TextUI.Structs;
using TextUI.Windows.Base;

namespace TextUI.Inputs.Base {

    public abstract class Control : IControl {

        protected Control(int x, int y, int height, int width, Window parentWindow, string id) {

            var rightBoundry = (parentWindow.Origin.X + parentWindow.Width) - 1;
            var bottomBoundry = (parentWindow.Origin.Y + parentWindow.Height) - 1;
            if ((x > rightBoundry) || (y > bottomBoundry)) 
                throw new Exception("Control can't be placed outside its containing window");
            
            ParentWindow = parentWindow;
            Id = id;

            Origin = new Point { X = x, Y = y };

            Height = height;
            Width = width;
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public Point Origin { get; internal set; }
        public Point Center {
            get {
                return new Point {
                    X = Origin.X + (Width / 2),
                    Y = Origin.Y + (Height / 2)
                };
            }
        }
        internal TextCursor TextCursor { get; set; }
        public bool IsSelectable { get; set; }
        public string Id { get; set; }
        public Window ParentWindow { get; set; }
        public TuiApp ContainingApp { get; internal set; }

        public virtual void Select() {}

        public abstract void Draw();

        public virtual void AddLetter(char letter) {}

        public virtual void BackSpace() {}

        public virtual void CursorMoveLeft() {}

        public virtual void CursorMoveRight() {}

        public virtual void CursorMoveUp() {}

        public virtual void CursorMoveDown() {}

        public virtual void CursorToStart() {}

        public virtual void CursorToEnd() {}

        public virtual void Enter() {}

        public virtual void Tab() {
            ParentWindow.MoveToNextControl();
        }

        public virtual void Unselect() {}

        public virtual void Spacebar() {}

    }

}

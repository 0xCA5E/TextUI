using System;
using System.Collections.Generic;
using System.Linq;

using TextUI.Inputs.Base;
using TextUI.Structs;

namespace TextUI.Windows.Base {

    public class Window : IElement {

        public Window(int x, int y, int width, int height, Window parentWindow, ConsoleColor backgroundColor = ConsoleColor.Black) {
            Origin = new Point{ X = x, Y = y };
            Width = width;
            Height = height;
            BackgroundColor = backgroundColor;
            _controls = new List<Control>();

            ParentWindow = parentWindow;
            if (ParentWindow == null) return;

            ContainingApp = ParentWindow.ContainingApp;
            ParentWindow._childWindows.Add(this);

            DrawBackground();
        }

        public bool IsRootWindow {
            get { return ParentWindow == null; }
        }
        public bool HasExited {
            get { return _hasExited; }
        }
        public Window ParentWindow { get; private set; }
        public IReadOnlyCollection<Window> ChildWindows {
            get { return _childWindows.AsReadOnly(); }
        }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Point Origin { get; protected set; }
        public Point Center {
            get {
                return new Point {
                    X = Origin.X + (Width / 2),
                    Y = Origin.Y + (Height / 2)
                };
            }
        }
        public ConsoleColor BackgroundColor { get; set; }
        public IReadOnlyCollection<Control> Controls {
            get { return _controls.AsReadOnly(); }
        }
        public TuiApp ContainingApp { get; internal set; }
        protected Control CurrentlySelected {
            get { return _currentlySelected; }
            set {
                _currentlySelected = value;

                _controls.ForEach(c => c.Unselect());

                if (CurrentlySelected != null)
                    CurrentlySelected.Select();
            }
        }
        protected List<Control> _controls;
        private readonly List<Window> _childWindows = new List<Window>();
        private volatile bool _hasExited;
        private Control _currentlySelected;

        public virtual void Draw() {
            if (ContainingApp == null) return;

            DrawBackground();

            foreach (var control in _controls)
                control.Draw();

            if (CurrentlySelected == null)
                SelectFirstControl();
            else
                CurrentlySelected.Select();
        }

        public void AddControl(Control control) {
            if (control == null) return;
            control.ContainingApp = ContainingApp;
            _controls.Add(control);
            control.Draw();
        }

        public void RemoveControl(Control control) {
            if (control == null) return;
            if (!_controls.Contains(control)) return;
            if (CurrentlySelected == control) {
                var success = MoveToNextControl();
                if (!success) CurrentlySelected = null;
            }
            _controls.Remove(control);
            Draw();
        }

        public void RemoveControl(string id) {
            if (id == null) return;
            var targetList = Controls.Where(control => string.Equals(control.Id, id, StringComparison.Ordinal)).ToList();
            if (!targetList.Any()) return;
            RemoveControl(targetList.First());
        }

        public void Run() {
            if (HasExited) return;
            MainLoop();
        }

        public void SelectFirstControl() {
            if (_controls.All(x => !x.IsSelectable)) //No IsSelectable inputs on page
                return;

            CurrentlySelected = _controls.First(x => x.IsSelectable);

        }

        public void SelectControlById(string id) {
            if (_controls.All(x => !x.IsSelectable)) //No IsSelectable inputs on page
                return;

            var newSelectedInput = _controls.FirstOrDefault(x => x.Id == id);
            if (newSelectedInput == null) //No TuiControl with this ID
                return;

            CurrentlySelected = newSelectedInput;

        }

        public IControl GetControlById(string id) {
            return _controls.FirstOrDefault(x => x.Id == id);
        }

        public bool MoveToNextControl() {
            var next = GetNextControl(true);
            if (next == null) return false;
            CurrentlySelected = next;
            return true;
        }

        public bool MoveToPrevControl() {
            var prev = GetPrevControl(true);
            if (prev == null) return false;
            CurrentlySelected = prev;
            return true;
        }

        public void MovetoNextControlUp() {
            
        }

        public void MovetoNextControlDown() {
            
        }

        public void MovetoNextControlLeft() {
            
        }

        public void MovetoNextControlRight() {
            var control = GetNextControlRight(true);
            
        }

        public Control GetNextControl(bool selectableControlsOnly = false) {

            var idxOfThisControl = _controls.IndexOf(CurrentlySelected);
            var i = idxOfThisControl;
            do {
                i = (i + 1) % Controls.Count;
                if (selectableControlsOnly && !_controls[i].IsSelectable) 
                    continue;
                return _controls[i];
            } while (i != idxOfThisControl);

            return null;
        }

        public Control GetPrevControl(bool selectableControlsOnly = false) {
            
            var idxOfThisControl = _controls.IndexOf(CurrentlySelected);
            var i = idxOfThisControl;
            do {
                i = (i - 1);
                if (i < 0) i = Controls.Count - 1;
                if (selectableControlsOnly && !_controls[i].IsSelectable) 
                    continue;
                return _controls[i];
            } while (i != idxOfThisControl);

            return null;
        }

        public Control GetNextControlRight(bool selectableControlsOnly = false) {
            if (CurrentlySelected == null) return null;

            // Get all controls that collide with a rightward ray-trace the height of this control
            var rayPos = new Point { 
                X = CurrentlySelected.Origin.X + CurrentlySelected.Width, 
                Y = CurrentlySelected.Origin.Y 
            };
            var rayWidth = Width - (CurrentlySelected.Origin.X + CurrentlySelected.Width);
            var rayHeight = CurrentlySelected.Height;

            ContainingApp.DrawColorBlock(ConsoleColor.Magenta, rayPos.X, rayPos.Y, rayPos.X + rayWidth, rayPos.Y + rayHeight);
            
            var controlsInRay = from control in _controls
                                where DoAreasOverlap(
                                    rayPos.X, rayPos.Y, rayWidth, rayHeight, 
                                    control.Origin.X, control.Origin.Y, control.Width, control.Height)
                                select control;

            if (controlsInRay.Any()) {
                var s = 2;
            }
                

            // Return the closest one to the right edge of this control, If one exists

            // Else, return the next control in Controls list

            
            return null;
        }

        public Control GetNextControlLeft(bool selectableControlsOnly = false) {
            
            return null;
        }

        public Control GetNextControlUp(bool selectableControlsOnly = false) {
            
            return null;
        }

        public Control GetNextControlDown(bool selectableControlsOnly = false) {
            
            return null;
        }

        public void Close() {

            if (ParentWindow != null) {
                ParentWindow.Draw();
                if (ParentWindow._childWindows.Contains(this))
                    ParentWindow._childWindows.Remove(this);
            }

            _controls = null;
            ParentWindow = null;
            ContainingApp = null;
            _hasExited = true;
        }

        internal void ForwordContainingAppDownTuiGraph() {
            // Controls
            foreach (var control in Controls) {
                control.ContainingApp = ContainingApp;
                if (control.TextCursor != null)
                    control.TextCursor.ContainingApp = ContainingApp;
            }

            // Child Windows
            foreach (var window in ChildWindows) {
                window.ContainingApp = ContainingApp;
                window.ForwordContainingAppDownTuiGraph();
            }
        }

        private void MainLoop() {

            while (!HasExited) {
                var input = ReadKey();

                switch (input.Key) {
                    case ConsoleKey.Tab:
                        if (CurrentlySelected == null) break;
                        CurrentlySelected.Tab();
                        break;
                    case ConsoleKey.Enter:
                        if (CurrentlySelected == null) break;
                        CurrentlySelected.Enter();
                        break;
                    case ConsoleKey.Spacebar:
                        if (CurrentlySelected == null) break;
                        CurrentlySelected.Spacebar();
                        break;
                    case ConsoleKey.LeftArrow:
                        if (CurrentlySelected == null) break;
                        CurrentlySelected.CursorMoveLeft();
                        break;
                    case ConsoleKey.RightArrow:
                        if (CurrentlySelected == null) break;
                        CurrentlySelected.CursorMoveRight();
                        break;
                    case ConsoleKey.UpArrow:
                        if (CurrentlySelected == null) break;
                        CurrentlySelected.CursorMoveUp();
                        break;
                    case ConsoleKey.DownArrow:
                        if (CurrentlySelected == null) break;
                        CurrentlySelected.CursorMoveDown();
                        break;
                    case ConsoleKey.Backspace:
                        if (CurrentlySelected == null) break;
                        CurrentlySelected.BackSpace();
                        break;
                    default:
                        if (CurrentlySelected == null) break;
                        CurrentlySelected.AddLetter(input.KeyChar);
                        break;
                }
            }
        }

        private bool DoAreasOverlap(int areaOneX, int areaOneY, int areaOneWidth, int areaOneHeight, int areaTwoX, int areaTwoY, int areaTwoWidth, int areaTwoHeight) {
            var areaOneEndX = areaOneX + areaOneHeight - 1;
            var areaOneEndY = areaOneY + areaOneWidth - 1;
            var areaTwoEndX = areaTwoX + areaTwoHeight - 1;
            var areaTwoEndY = areaTwoY + areaTwoWidth - 1;

            var overlapsVertically = false;
            //Check if overlap vertically
            if (areaOneX >= areaTwoX && areaOneX < areaTwoEndX) //areaOne starts in areaTwo
                overlapsVertically = true;
            else if (areaOneEndX >= areaTwoX && areaOneEndX <= areaTwoEndX) //areaOne ends in areaTwo
                overlapsVertically = true;
            else if (areaOneX < areaTwoX && areaOneEndX >= areaTwoEndX) //areaOne start before and end after areaTwo
                overlapsVertically = true;
            //areaOne inside areaTwo is caught by first two statements

            if (!overlapsVertically) //If it does not overlap vertically, then it does not overlap.
                return false;

            var overlapsHorizontally = false;
            //Check if overlap Horizontally
            if (areaOneY >= areaTwoY && areaOneY < areaTwoEndY) //areaOne starts in areaTwo
                overlapsHorizontally = true;
            else if (areaOneEndY >= areaTwoY && areaOneEndY < areaTwoEndY) //areaOne ends in areaTwo
                overlapsHorizontally = true;
            else if (areaOneY <= areaTwoY && areaOneEndY >= areaTwoEndY) //areaOne starts before and ends after areaTwo
                overlapsHorizontally = true;
            //areaOne inside areaTwo is caught by first two statements

            if (!overlapsHorizontally) //If it does not overlap Horizontally, then it does not overlap.
                return false;

            return true; //it overlaps vertically and horizontally, thus areas must overlap
        }

        private int CalcPrevControlIndex(int index) {
            if (index == 0)
                return _controls.Count() - 1;

            return index - 1;
        }

        private static ConsoleKeyInfo ReadKey() {
            return Console.ReadKey(true);
        }

        private void DrawBackground() {
            ContainingApp.DrawColorBlock(BackgroundColor, Origin.X, Origin.Y, Origin.X + Width, Origin.Y + Height); // Main background box
        }

    }

}
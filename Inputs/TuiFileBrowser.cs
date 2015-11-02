using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows;
using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Inputs
{
    public class TuiFileBrowser : TuiControl
    {

        public TuiFileBrowser(int x, int y, int width, int height, String path, String id, TuiWindow parentWindow, bool includeFiles = false, string filterByExtension = "*") 
            : base(x, y, height, width, parentWindow, id)
        {
            CurrentPath = path;
            CurrentlySelectedFile = "";
            IncludeFiles = includeFiles;
            FilterByExtension = filterByExtension;
            Drives = Directory.GetLogicalDrives().ToList();

            GetFileNames();
            Selectable = true;
        }

        public String CurrentPath { get; private set; }
        public String CurrentlySelectedFile { get; private set; }
        private int CursorX { get { return cursorX; } set { cursorX = value; GetCurrentlySelectedFileName(); SetOffset(); } }

        public bool IncludeFiles;
        public String FilterByExtension = "*";

        public Action ChangeItem;
        public Action SelectFile;


        private List<String> FileNames = new List<String>();
        private List<String> Folders;
        private List<String> Drives;
        private ConsoleColor BackgroundColour = ConsoleColor.DarkGray;
        private ConsoleColor TextColour = ConsoleColor.Black;
        private ConsoleColor SelectedTextColour = ConsoleColor.White;
        private ConsoleColor SelectedBackgroundColour = ConsoleColor.Gray;

        private int cursorX;

        private int Offset = 0;
        private bool Selected = false;
        private bool AtRoot = false;
        private bool ShowingDrive = false;

        public void GetFileNames()
        {
            if (ShowingDrive) //Currently Showing drives. This function should not be called!
                return;

            try
            {
                if(IncludeFiles)
                    FileNames = Directory.GetFiles(CurrentPath, "*." + FilterByExtension).Select(path => System.IO.Path.GetFileName(path)).ToList();

                Folders = Directory.GetDirectories(CurrentPath).Select(path => System.IO.Path.GetFileName(path)).ToList();
                
                Folders.Insert(0, "..");

                if (Directory.GetParent(CurrentPath) != null)
                {
                    AtRoot = false;

                }
                else
                    AtRoot = true;

                if (CursorX > FileNames.Count() + Folders.Count())
                    CursorX = 0;
            }
            catch (UnauthorizedAccessException e)
            {
                throw e;
            }
        }

        override public void Draw()
        {
            if (ContainingApp == null) return;
            ParentWindow.ContainingApp.DrawColorBlock(BackgroundColour, X, Y, X + Height, Y + Width);

            if (!ShowingDrive)
            {
                var trimedPath = CurrentPath.PadRight(Width - 2, ' ');
                trimedPath = trimedPath.Substring(trimedPath.Count() - Width + 2, Width - 2);
                ContainingApp.WirteText(trimedPath, X, Y + 1, ConsoleColor.Gray, BackgroundColour);
            }
            else
                ContainingApp.WirteText("Drives", X, Y + 1, ConsoleColor.Gray, BackgroundColour);

            if (!ShowingDrive)
            {
                var i = Offset;
                while (i < Math.Min(Folders.Count, Height + Offset - 1))
                {
                    var folderName = Folders[i].PadRight(Width - 2, ' ').Substring(0, Width - 2);

                    if (i == CursorX)
                        if (Selected)
                            ContainingApp.WirteText(folderName, X + i - Offset + 1, Y + 1, SelectedTextColour, SelectedBackgroundColour);
                        else
                            ContainingApp.WirteText(folderName, X + i - Offset + 1, Y + 1, SelectedTextColour, BackgroundColour);
                    else
                        ContainingApp.WirteText(folderName, X + i - Offset + 1, Y + 1, TextColour, BackgroundColour);

                    i++;
                }

                while (i < Math.Min(Folders.Count + FileNames.Count, Height + Offset - 1))
                {
                    var fileName = FileNames[i - Folders.Count].PadRight(Width - 2, ' ').Substring(0, Width - 2);

                    if (i == CursorX)
                        if (Selected)
                            ContainingApp.WirteText(fileName, X + i - Offset + 1, Y + 1, SelectedTextColour, SelectedBackgroundColour);
                        else
                            ContainingApp.WirteText(fileName, X + i - Offset + 1, Y + 1, SelectedTextColour, BackgroundColour);
                    else
                        ContainingApp.WirteText(fileName, X + i - Offset + 1, Y + 1, TextColour, BackgroundColour);
                    i++;
                }
            }
            else
            {
                for (var i = 0; i < Drives.Count(); i++)
                {
                    if (i == CursorX)
                        if (Selected)
                            ContainingApp.WirteText(Drives[i], X + i - Offset + 1, Y + 1, SelectedTextColour, SelectedBackgroundColour);
                        else
                            ContainingApp.WirteText(Drives[i], X + i - Offset + 1, Y + 1, SelectedTextColour, BackgroundColour);
                    else
                        ContainingApp.WirteText(Drives[i], X + i - Offset + 1, Y + 1, TextColour, BackgroundColour);
                    
                }

            }

        }

        override public void Select()
        {
            if (!Selected)
            {
                Selected = true;
                Draw();
            }
        }

        override public void Unselect()
        {
            if (Selected)
            {
                Selected = false;
                Draw();
            }
        }

        override public void CursorMoveDown()
        {
            if (CursorX != Folders.Count + FileNames.Count - 1 && !ShowingDrive)
            {
                CursorX++;
                Draw();
            }
            else if (CursorX != Drives.Count - 1 && ShowingDrive)
            {
                CursorX++;
                Draw();
            }
            else
                ParentWindow.MovetoNextControlDown(X, Y, Width);    
        }

        override public void CursorMoveUp()
        {
            if (CursorX != 0)
            {
                CursorX--;
                Draw();
            }
            else
                ParentWindow.MovetoNextControlUp(X, Y, Width); 
        }

        override public void CursorMoveRight()
        {
            if (CursorX >= 1 && CursorX < Folders.Count && !ShowingDrive) //Folder is selected
                GoIntoFolder();
            else if (ShowingDrive)
                GoIntoDrive();
        } 

        override public void Enter()
        {
            if (CursorX >= 1 && CursorX < Folders.Count && !ShowingDrive) //Folder is selected
                GoIntoFolder();
            else if (cursorX == 0 && !AtRoot) //Back is selected
                GoToParentFolder();
            else if (SelectFile != null && !ShowingDrive) //File is selcted
                SelectFile();
            else if (cursorX == 0 && AtRoot && !ShowingDrive) //Back is selected and at root, thus show drives
                DisplayDrives();
            else if (ShowingDrive)
                GoIntoDrive();

            
        }

        override public void CursorMoveLeft()
        {
            if (!AtRoot)
                GoToParentFolder();
            else
                DisplayDrives();
        }

        override public void BackSpace()
        {
            if (!AtRoot)
                GoToParentFolder();
        }

        private void DisplayDrives()
        {
            ShowingDrive = true;
            CurrentPath = "";
            CursorX = 0;
            Draw();
        }

        private void GoIntoDrive()
        {
            CurrentPath = Drives[cursorX];

            try
            {
                ShowingDrive = false;
                GetFileNames();
                CursorX = 0;
                Draw();
            }
            catch (IOException e)
            {
                CurrentPath = ""; //Change Path back to nothing
                ShowingDrive = true;
                new TuiAlert(e.Message, ParentWindow, ConsoleColor.White);
            }
        
        }

        private void GoIntoFolder()
        {
            CurrentPath = Path.Combine(CurrentPath, Folders[cursorX]);
            
            try
            {
                GetFileNames();
                CursorX = 0;
                Draw();
            }
            catch (UnauthorizedAccessException e)
            {
                CurrentPath = Directory.GetParent(CurrentPath).FullName; //Change Path back to parent
                new TuiAlert("Access Denied", ParentWindow, ConsoleColor.White, "Error");
            }
        }

        private void GoToParentFolder()
        {
            CurrentPath = Directory.GetParent(CurrentPath).FullName;
            GetFileNames();
            CursorX = 0;
            Draw();
        }

        private void SetOffset()
        {
            while (CursorX - Offset > Height - 2)
                Offset++;

            while (CursorX - Offset < 0)
                Offset--;
        }

        private void GetCurrentlySelectedFileName()
        {
            if (cursorX >= Folders.Count()) //File is selected
            {
                CurrentlySelectedFile = FileNames[cursorX - Folders.Count];
                if (ChangeItem != null)
                    ChangeItem();
            }
            else
            {
                if (CurrentlySelectedFile != "")
                {
                    CurrentlySelectedFile = "";
                    if (ChangeItem != null)
                        ChangeItem();
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDraw.Windows.Base;
using ConsoleDraw.Inputs;
using ConsoleDraw.Inputs.TuiDropdown;
using ConsoleDraw.Windows;

namespace ConsoleDraw.Windows
{
    public class TuiLoadMenu : TuiPopupWindow
    {

        public TuiLoadMenu(String path, Dictionary<String, String> fileTypes, TuiWindow parentTuiWindow)
            : base("Load TuiMenu", Math.Min(6, Console.WindowHeight - 22), (Console.WindowWidth / 2) - 30, 60, 20, parentTuiWindow)
        {
            BackgroundColor = ConsoleColor.White;
            FileTypes = fileTypes;

            _tuiFileSelect = new TuiFileBrowser(X + 2, Y + 2, 56, 13, path, "_tuiFileSelect", this, true, "txt");
            _tuiFileSelect.ChangeItem = delegate() { UpdateCurrentlySelectedFileName(); };
            _tuiFileSelect.SelectFile = delegate() { LoadFile(); };

            var openLabel = new TuiLabel("Open", X + 16, Y + 2, "openLabel", this);
            openTxtBox = new TuiTextBox(X + 16, Y + 7, "openTxtBox", this, Width - 13) { Selectable = false };

            _fileTypeTuiDropdown = new TuiDropdown(X + 18, Y + 40, FileTypes.Select(x => x.Value).ToList(), "_fileTypeTuiDropdown", this, 17);
            _fileTypeTuiDropdown.OnUnselect = delegate() { UpdateFileTypeFilter(); };

            loadBtn = new TuiButton(X + 18, Y + 2, "Load", "loadBtn", this);
            loadBtn.Action = delegate() { LoadFile(); };
            cancelBtn = new TuiButton(X + 18, Y + 9, "Cancel", "cancelBtn", this);
            cancelBtn.Action = delegate() { Close(); };

            AddControl(_tuiFileSelect);
            AddControl(loadBtn);
            AddControl(cancelBtn);
            AddControl(openLabel);
            AddControl(openTxtBox);
            AddControl(_fileTypeTuiDropdown);
            
            CurrentlySelected = _tuiFileSelect;

            Draw();
        }

        private TuiButton loadBtn;
        private TuiButton cancelBtn;
        private TuiTextBox openTxtBox;
        private TuiFileBrowser _tuiFileSelect;
        public String FileNameLoaded;
        private TuiDropdown _fileTypeTuiDropdown;

        public Boolean DataLoaded;
        public String Data;
        public String PathOfLoaded;
        public Dictionary<String, String> FileTypes;

        private void UpdateCurrentlySelectedFileName()
        {
            var CurrentlySelectedFile = _tuiFileSelect.CurrentlySelectedFile;
            openTxtBox.SetText(CurrentlySelectedFile);
        }

        private void UpdateFileTypeFilter()
        {
            var filter = FileTypes.FirstOrDefault(x => x.Value == _fileTypeTuiDropdown.Text);
            var currentFilter = FileTypes.FirstOrDefault(x => x.Key == _tuiFileSelect.FilterByExtension);

            if (currentFilter.Key != filter.Key)
            {
                _tuiFileSelect.FilterByExtension = filter.Key;
                _tuiFileSelect.GetFileNames();
                _tuiFileSelect.Draw();
            }
        }

        private void LoadFile()
        {
            if (_tuiFileSelect.CurrentlySelectedFile == "")
            {
                new TuiAlert("No file Selected", this, "Warning");
                return;
            }

            var file = Path.Combine(_tuiFileSelect.CurrentPath, _tuiFileSelect.CurrentlySelectedFile);
            String text = System.IO.File.ReadAllText(file);

            /*var mainWindow = (MainWindow)parentTuiWindow;
            mainWindow.textArea.SetText(text);
            mainWindow.fileLabel.SetText(_tuiFileSelect.CurrentlySelectedFile);*/

            DataLoaded = true;
            Data = text;
            FileNameLoaded = _tuiFileSelect.CurrentlySelectedFile;
            PathOfLoaded = _tuiFileSelect.CurrentPath;

            Close();
        }
    }
}

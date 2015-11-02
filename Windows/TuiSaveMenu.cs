using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDraw.Windows.Base;
using ConsoleDraw.Inputs;
using ConsoleDraw.Windows;

namespace ConsoleDraw.Windows
{
    public class TuiSaveMenu : TuiPopupWindow
    {

        public TuiSaveMenu(String fileName, String path, String data, TuiWindow parentTuiWindow)
            : base("Save Menu", 6, (Console.WindowWidth / 2) - 30, 60, 20, parentTuiWindow)
        {
            BackgroundColor = ConsoleColor.White;
            Text = data;

            _tuiFileSelect = new TuiFileBrowser(X + 2, Y + 2, 56, 12, path, "FileSelect", this);

            var openLabel = new TuiLabel("Name", X + 16, Y + 2, "openLabel", this);
            openTxtBox = new TuiTextBox(X + 16, Y + 7, fileName, "openTxtBox", this, Width - 13) { Selectable = true };

            saveBtn = new TuiButton(X + 18, Y + 2, "Save", "loadBtn", this);
            saveBtn.Action = delegate() { SaveFile(); };
            cancelBtn = new TuiButton(X + 18, Y + 9, "Cancel", "cancelBtn", this);
            cancelBtn.Action = delegate() { Close(); };

            AddControl(_tuiFileSelect);
            AddControl(openLabel);
            AddControl(openTxtBox);
            AddControl(saveBtn);
            AddControl(cancelBtn);

            CurrentlySelected = saveBtn;

            Draw();
        }

        public Boolean FileWasSaved;
        public String FileSavedAs;
        public String PathToFile;
        private TuiButton saveBtn;
        private TuiButton cancelBtn;
        private TuiTextBox openTxtBox;
        private TuiFileBrowser _tuiFileSelect;
        private String Text;


        private void SaveFile()
        {
            var path = _tuiFileSelect.CurrentPath;
            var filename = openTxtBox.GetText();

            var fullFile = Path.Combine(path, filename);

            try
            {
                StreamWriter file = new StreamWriter(fullFile);

                file.Write(Text);

                file.Close();

                FileWasSaved = true;
                FileSavedAs = filename;
                PathToFile = path;

                Close();
            }
            catch
            { 
                new TuiAlert("You do not have access", this, "Error");
            }

            
        }
    }
}

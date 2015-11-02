using TextUI.Structs;
using TextUI.Windows.Base;

namespace TextUI {

    public interface ITuiElement {

        TuiWindow ParentWindow { get; }
        TuiApp ContainingApp { get; }
        int Width { get; }
        int Height { get; }
        Point Origin { get; }
        Point Center { get; }
        void Draw();
        
    }

}
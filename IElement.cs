using TextUI.Structs;
using TextUI.Windows.Base;

namespace TextUI {

    public interface IElement {

        Window ParentWindow { get; }
        TuiApp ContainingApp { get; }
        int Width { get; }
        int Height { get; }
        Point Origin { get; }
        Point Center { get; }
        void Draw();
        
    }

}
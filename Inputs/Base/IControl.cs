namespace TextUI.Inputs.Base {

    public interface IControl : IElement {

        bool IsSelectable { get; set; }
        string Id { get; }

        void Select();

        void Unselect();

        void AddLetter(char letter);

        void BackSpace();

        void CursorMoveLeft();

        void CursorMoveUp();

        void CursorMoveDown();

        void CursorMoveRight();

        void CursorToStart();

        void CursorToEnd();

        void Enter();

        void Spacebar();

        void Tab();

    }

}
namespace Common
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}

namespace HelpDesk.Models.Abstractions
{
    public interface IDataContext
    {
        DataState CurrentState { get; set; }

        void SaveState();
    }
}
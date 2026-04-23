namespace SMSSender.Interfaces
{
    public interface IMessageService
    {
        bool GetMessageFiltered(string? provider, string message);
    }
}

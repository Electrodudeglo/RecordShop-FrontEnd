namespace RecordShop_FrontEnd.Interfaces
{
    public interface IToastService
{
        event Action? OnChange;
        IReadOnlyList<ToastMessageClass> Messages { get; }
        void Show(string text, ToastEnum type = ToastEnum.Info, TimeSpan? duration=null);
        void Remove(ToastMessageClass message);
}
}

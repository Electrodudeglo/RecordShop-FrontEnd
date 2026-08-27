using RecordShop_FrontEnd.Components.UI;
using RecordShop_FrontEnd.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace RecordShop_FrontEnd.Services
{
    public class ToastService : IToastService
{
        private readonly List<ToastMessageClass> _messages = new();
        public event Action? OnChange;
        public IReadOnlyList<ToastMessageClass> Messages => _messages;

        public ToastService()
        {
            _messages.Clear();
        }

        public void Show(string text, ToastEnum type = ToastEnum.Info, TimeSpan? duration = null)
        {
            var toast = new ToastMessageClass
            {
                Text = text,
                Type = type,
                Duration = duration ?? TimeSpan.FromSeconds(3)
            };
            _messages.Add(toast);

            if (_messages.Count > 1)
            {
                _messages.RemoveRange(0, _messages.Count - 1);
            }

            OnChange?.Invoke();
            _ = AutoRemoveAsync(toast);
        }

        public void Clear()
        {
            _messages.Clear();
            OnChange?.Invoke();
        }
        public void Remove(ToastMessageClass message)
        {
            if (_messages.Remove(message)) OnChange?.Invoke();
        }

        private async Task AutoRemoveAsync(ToastMessageClass toast)
        {
            await Task.Delay(toast.Duration);
            Remove(toast);
        }
    }
}

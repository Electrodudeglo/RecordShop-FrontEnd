namespace RecordShop_FrontEnd
{
    public class ToastMessageClass
{
        public string Text { get; set; } = "";
        public ToastEnum Type { get; set; } = ToastEnum.Info;
        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(3);

}
}

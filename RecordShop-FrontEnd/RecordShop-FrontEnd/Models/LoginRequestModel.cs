namespace RecordShop_FrontEnd.Models
{
    public enum LoginResultEnum
    {
        Success,
        InvalidCredentials,
        ServerError
    }

  public class LoginRequestModel
{
        public string UserName { get; set; } = String.Empty;

        public string Password { get; set; } = String.Empty;    

}


}

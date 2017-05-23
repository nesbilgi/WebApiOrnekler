namespace Nes.RestApi.CSharp.Example.Model
{
    #region Token
    public class TokenRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    #endregion

    #region Account
    public class GetTemplateRequest
    {
        public string Title { get; set; }
        public GetTemplateType TemplateType { get; set; }
    }
    #endregion
}

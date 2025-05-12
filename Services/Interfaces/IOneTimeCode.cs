namespace NajdiSpolubydliciRazor.Services.Interfaces
{
    public interface IOneTimeCode
    {
        public string GenerateCode();

        public bool IsTooOld(DateTime lastOperation);
    }
}

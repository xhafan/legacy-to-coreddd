using CoreDddShared.Domain;
using System.Threading.Tasks;

namespace ServiceApp
{
    public class InternationalMaritimeOrganizationVerifier : IInternationalMaritimeOrganizationVerifier
    {
#pragma warning disable 1998
        public async Task<bool> IsImoNumberValid(string imoNumber)
#pragma warning restore 1998
        {
            // implement ship verification using International Maritime Organization web api
            return false;
        }
}
}
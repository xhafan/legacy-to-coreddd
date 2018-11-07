using System.Threading.Tasks;

namespace CoreDddShared.Domain
{
    public interface IInternationalMaritimeOrganizationVerifier
    {
        Task<bool> IsImoNumberValid(string imoNumber);
    }
}
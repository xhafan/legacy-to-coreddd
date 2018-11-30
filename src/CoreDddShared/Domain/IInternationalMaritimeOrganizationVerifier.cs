#if NETSTANDARD
using System.Threading.Tasks;
#endif

namespace CoreDddShared.Domain
{
    public interface IInternationalMaritimeOrganizationVerifier
    {
#if NETSTANDARD
        Task<bool> IsImoNumberValid(string imoNumber);
#endif
#if NET40
        bool IsImoNumberValid(string imoNumber);
#endif
    }
}
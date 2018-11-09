#if NET40
using System.Threading;
#endif 

#if NETSTANDARD2_0
using System.Threading;
using System.Threading.Tasks;
#endif

namespace CoreDddShared.Domain
{
    public class InternationalMaritimeOrganizationVerifier : IInternationalMaritimeOrganizationVerifier
    {
#if NETSTANDARD2_0
#pragma warning disable 1998
        public async Task<bool> IsImoNumberValid(string imoNumber)
#pragma warning restore 1998
        {
            // implement ship verification using International Maritime Organization web api
            await Task.Delay(4000); // sleep 4 seconds to simulate slow web request
            return true;
        }
#endif
#if NET40
        public bool IsImoNumberValid(string imoNumber)
        {
            // implement ship verification using International Maritime Organization web api
            Thread.Sleep(4000); // sleep 4 seconds to simulate slow web request        
            return true;
        }
#endif
    }
}
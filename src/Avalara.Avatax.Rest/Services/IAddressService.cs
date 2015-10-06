using System.Threading.Tasks;
using Avalara.Avatax.Rest.Data;

namespace Avalara.Avatax.Rest.Services
{
    public interface IAddressService
    {
        Task<ValidateResult> Validate(Address address);
    }
}
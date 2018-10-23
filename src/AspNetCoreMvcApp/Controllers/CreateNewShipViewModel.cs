using AspNetCoreMvcApp.Commands;

namespace AspNetCoreMvcApp.Controllers
{
    public class CreateNewShipViewModel
    {
        public int? LastCreatedShipId { get; set; }
        public CreateNewShipCommand CreateNewShipCommand { get; set; }
    }
}
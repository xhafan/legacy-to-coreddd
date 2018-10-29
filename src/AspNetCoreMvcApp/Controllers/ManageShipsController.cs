using System.Diagnostics;
using System.Threading.Tasks;
using AspNetCoreMvcApp.Models;
using CoreDdd.Commands;
using CoreDdd.Queries;
using CoreDddShared.Commands;
using CoreDddShared.Dtos;
using CoreDddShared.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvcApp.Controllers
{
    public class ManageShipsController : Controller
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor;

        public ManageShipsController(
            ICommandExecutor commandExecutor,
            IQueryExecutor queryExecutor
            )
        {
            _queryExecutor = queryExecutor;
            _commandExecutor = commandExecutor;
        }

        public async Task<IActionResult> Index()
        {
            var shipDtos = await _queryExecutor.ExecuteAsync<GetAllShipsQuery, ShipDto>(new GetAllShipsQuery());
            return View(shipDtos);
        }

        public IActionResult CreateNewShip()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewShip(CreateNewShipCommand createNewShipCommand)
        {
            var generatedShipId = 0;
            _commandExecutor.CommandExecuted += args => generatedShipId = (int) args.Args;
            await _commandExecutor.ExecuteAsync(createNewShipCommand);

            return RedirectToAction("CreateNewShip", new { lastCreatedShipId = generatedShipId });
        }

        public IActionResult UpdateShip()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateShip(UpdateShipCommand updateShipCommand)
        {
            await _commandExecutor.ExecuteAsync(updateShipCommand);

            return RedirectToAction("UpdateShip");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
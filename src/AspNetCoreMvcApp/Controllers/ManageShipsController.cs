using System.Diagnostics;
using System.Threading.Tasks;
using AspNetCoreMvcApp.Commands;
using AspNetCoreMvcApp.Dtos;
using AspNetCoreMvcApp.Models;
using AspNetCoreMvcApp.Queries;
using CoreDdd.Commands;
using CoreDdd.Queries;
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
            await _commandExecutor.ExecuteAsync(createNewShipCommand);
            return RedirectToAction("Index");
        }

        public IActionResult UpdateShip()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateShip(UpdateShipCommand updateShipCommand)
        {
            await _commandExecutor.ExecuteAsync(updateShipCommand);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
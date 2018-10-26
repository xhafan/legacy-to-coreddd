using System.Reflection;
using System.Threading.Tasks;
using AspNetCoreMvcApp.Commands;
using AspNetCoreMvcApp.Domain;
using CoreDdd.Domain.Repositories;
using FakeItEasy;
using NUnit.Framework;
using Shouldly;

namespace AspNetCoreMvcApp.Tests.Commands.CreateNewShipCommandHandlers.LondonStyleTestsNotUseful
{
    [TestFixture]
    public class when_creating_new_ship
    {
        private int _generatedShipId;
        private IRepository<Ship> _shipRepository;

        [SetUp]
        public async Task Context()
        {
            var createNewShipCommand = new CreateNewShipCommand
            {
                ShipName = "ship name",
                Tonnage = 23.45678m
            };
            _shipRepository = A.Fake<IRepository<Ship>>();
            A.CallTo(() => _shipRepository.SaveAsync(A<Ship>._)).Invokes(x =>
            {
                var shipPassedAsParameter = x.GetArgument<Ship>(0);
                shipPassedAsParameter.SetPrivateProperty("Id", 23);
            });
            var createNewShipCommandHandler = new CreateNewShipCommandHandler(_shipRepository);
            createNewShipCommandHandler.CommandExecuted += args => _generatedShipId = (int) args.Args;
            await createNewShipCommandHandler.ExecuteAsync(createNewShipCommand);
        }

        [Test]
        public void ship_is_saved_with_correct_data()
        {
            A.CallTo(() => _shipRepository.SaveAsync(A<Ship>.That.Matches(p => _MatchingShip(p)))).MustHaveHappened();
        }

        private bool _MatchingShip(Ship p)
        {
            p.Name.ShouldBe("ship name");
            p.Tonnage.ShouldBe(23.45678m);
            return true;
        }

        [Test]
        public void command_executed_event_is_raised_with_stubbed_ship_id()
        {
            _generatedShipId.ShouldBe(23);
        }
    }
    public static class ObjectExtensions
    {
        public static void SetPrivateProperty(this object obj, string propertyName, object value)
        {
            obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).SetValue(obj, value, null);
        }
    }
}
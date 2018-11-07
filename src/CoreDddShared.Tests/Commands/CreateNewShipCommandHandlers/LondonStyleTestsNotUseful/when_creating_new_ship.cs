using System.Reflection;
using CoreDdd.Domain.Events;
using CoreDdd.Domain.Repositories;
using CoreDddShared.Commands;
using CoreDddShared.Domain;
using FakeItEasy;
using NUnit.Framework;
using Shouldly;

namespace CoreDddShared.Tests.Commands.CreateNewShipCommandHandlers.LondonStyleTestsNotUseful
{
    [TestFixture]
    public class when_creating_new_ship
    {
        private int _generatedShipId;
        private IRepository<Ship> _shipRepository;

        [SetUp]
        public void Context()
        {
            DomainEvents.Initialize(A.Fake<IDomainEventHandlerFactory>());

            var createNewShipCommand = new CreateNewShipCommand
            {
                ShipName = "ship name",
                Tonnage = 23.45678m,
                ImoNumber = "IMO 12345"
            };
            _shipRepository = A.Fake<IRepository<Ship>>();
#if NET40
            A.CallTo(() => _shipRepository.Save(A<Ship>._)).Invokes(x =>
#endif
#if !NET40
            A.CallTo(() => _shipRepository.SaveAsync(A<Ship>._)).Invokes(x =>
#endif
            {
                // when shipRepository.Save() is called, simulate NHibernate assigning Id to the Ship entity
                var shipPassedAsParameter = x.GetArgument<Ship>(0);
                shipPassedAsParameter.SetPrivateProperty("Id", 23);
            });
            var createNewShipCommandHandler = new CreateNewShipCommandHandler(_shipRepository);
            createNewShipCommandHandler.CommandExecuted += args => _generatedShipId = (int)args.Args;

#if NET40
            createNewShipCommandHandler.Execute(createNewShipCommand);
#endif
#if !NET40
            createNewShipCommandHandler.ExecuteAsync(createNewShipCommand).Wait();
#endif
        }

        [Test]
        public void ship_is_saved_with_correct_data()
        {
#if NET40
            A.CallTo(() => _shipRepository.Save(A<Ship>.That.Matches(p => _MatchingShip(p)))).MustHaveHappened();
#endif
#if !NET40
            A.CallTo(() => _shipRepository.SaveAsync(A<Ship>.That.Matches(p => _MatchingShip(p)))).MustHaveHappened();
#endif
        }

        private bool _MatchingShip(Ship p)
        {
            p.Name.ShouldBe("ship name");
            p.Tonnage.ShouldBe(23.45678m);
            p.ImoNumber.ShouldBe("IMO 12345");
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
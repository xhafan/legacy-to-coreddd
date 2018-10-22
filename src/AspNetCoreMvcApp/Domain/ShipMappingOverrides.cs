using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace AspNetCoreMvcApp.Domain
{
    public class ShipMappingOverrides : IAutoMappingOverride<Ship>
    {
        public void Override(AutoMapping<Ship> mapping)
        {
            mapping.Id(x => x.Id).Column("ShipId").GeneratedBy.Identity();
            mapping.Map(x => x.Name).Column("ShipName");
        }
    }
}
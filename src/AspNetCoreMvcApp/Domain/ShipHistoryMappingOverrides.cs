using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace AspNetCoreMvcApp.Domain
{
    public class ShipHistoryMappingOverrides : IAutoMappingOverride<ShipHistory>
    {
        public void Override(AutoMapping<ShipHistory> mapping)
        {
            mapping.Id(x => x.Id).Column("ShipHistoryId").GeneratedBy.Identity();
            mapping.Map(x => x.Name).Column("ShipName");
        }
    }
}
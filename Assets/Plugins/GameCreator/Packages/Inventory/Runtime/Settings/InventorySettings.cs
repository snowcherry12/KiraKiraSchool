using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Inventory
{
    public class InventorySettings : AssetRepository<InventoryRepository>
    {
        public override IIcon Icon => new IconItem(ColorTheme.Type.TextLight);
        public override string Name => "Inventory";
    }
}

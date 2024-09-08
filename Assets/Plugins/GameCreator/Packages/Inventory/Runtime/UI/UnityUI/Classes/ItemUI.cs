using System;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Serializable]
    public class ItemUI : TItemUI
    {
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void RefreshUI(Bag bag, Item item, bool forceSingleChunk)
        {
            this.RefreshItemUI(bag, item, forceSingleChunk);

            RuntimeItem dummyRuntimeItem = new RuntimeItem(item);
            this.RefreshRuntimeItemUI(bag, dummyRuntimeItem);
        }
    }
}
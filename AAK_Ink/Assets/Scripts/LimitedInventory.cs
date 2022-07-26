using AdventureCore;

public class LimitedInventory : ListedInventory
{
    public int Capacity;

    public override bool CanAddItems(ItemQuantity itemQuantity)
    {
        return base.CanAddItems(itemQuantity) && GetQuantity(itemQuantity.Item) + itemQuantity.Quantity <= Capacity;
    }
}
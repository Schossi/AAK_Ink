using AdventureCore;
using UnityEngine;

/// <summary>
/// special version of a list inventory that only holds a limited number per item
/// </summary>
public class LimitedInventory : ListedInventory
{
    [Tooltip("per item limit")]
    public int Capacity;

    public override bool CanAddItems(ItemQuantity itemQuantity)
    {
        return base.CanAddItems(itemQuantity) && GetQuantity(itemQuantity.Item) + itemQuantity.Quantity <= Capacity;
    }
}
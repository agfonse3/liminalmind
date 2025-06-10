using UnityEngine;

public class SlotClass 
{
    private ItemClass itemClass;
    private int quantity;

    public SlotClass (ItemClass _itemClass, int _quantity)
    {
        this.itemClass = _itemClass;
        this.quantity = _quantity;
    }
}

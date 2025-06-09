using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    public string itemName;
    public Sprite itemsprite;

    public abstract ItemClass GetItem();
    public abstract NoteClass GetNote();
    public abstract ClueClass GetClue();
    public abstract KeyClass GetKey();
}

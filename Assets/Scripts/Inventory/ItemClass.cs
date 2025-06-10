using UnityEngine;

public abstract class ItemClass : ScriptableObject // clase padre de todos los items
{
    public string id;
    public string itemName;
    public Sprite itemsprite;
    public GameObject itemprefab;

    public abstract ItemClass GetItem();
    public abstract NoteClass GetNote();
    public abstract ClueClass GetClue();
    public abstract KeyClass GetKey();
}

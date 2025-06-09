using UnityEngine;

[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "item/clue")]
public class ClueClass : ItemClass
{
    public override ItemClass GetItem() { return this; }
    public override NoteClass GetNote() { return null; }
    public override ClueClass GetClue() { return this; }
    public override KeyClass GetKey() { return null; }
}

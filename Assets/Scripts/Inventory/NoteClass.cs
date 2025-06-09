using UnityEngine;
[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "item/Note")]
public class NoteClass : ItemClass
{
    public override ItemClass GetItem() { return this; }
    public override NoteClass GetNote() { return this; }
    public override ClueClass GetClue() { return null; }
    public override KeyClass GetKey() { return null; }
}

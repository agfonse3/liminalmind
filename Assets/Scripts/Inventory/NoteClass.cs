using UnityEngine;
[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "item/Note")]
public class NoteClass : ItemClass // scriptable object nota
{
    //int numberOfRecolectedNotesPieces; // numero de piezas recolectadas de la nota
    public Sprite[] noteInPieces = new Sprite[4]; //sprites de la nota

    public override ItemClass GetItem() { return this; }
    public override NoteClass GetNote() { return this; }
    public override ClueClass GetClue() { return null; }
    public override KeyClass GetKey() { return null; }
}

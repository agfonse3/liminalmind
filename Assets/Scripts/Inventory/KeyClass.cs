using UnityEngine;

[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "item/key")]
public class KeyClass : ItemClass
{
    public KeyType keyType ;

    public enum KeyType 
    {
        yellowDoorKey,
        blueDoorKey,
        FinalKey
    }
    public override ItemClass GetItem() { return this; }
    public override NoteClass GetNote() { return null; }
    public override ClueClass GetClue() { return null; }
    public override KeyClass GetKey() { return this; }
}

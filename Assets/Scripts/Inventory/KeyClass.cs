using UnityEngine;

[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "item/key")]
public class KeyClass : ItemClass   // scriptable object key
{
    public KeyType keyType ; // tipo de llave
    public enum KeyType 
    {
        greenDoorKey,
        blueDoorKey,
        blueskydoorKey,
        violetDoorKey,
        yellowDoorKey,
        redDoorKey,
        FinalKey
    }
    public override ItemClass GetItem() { return this; }
    public override NoteClass GetNote() { return null; }
    public override ClueClass GetClue() { return null; }
    public override KeyClass GetKey() { return this; }
}

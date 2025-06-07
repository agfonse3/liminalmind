#if UNITY_EDITOR
    using UnityEditor;
#endif
using UnityEngine;

public class PanelBasic : MonoBehaviour, IPaneles
{
   
    //Metodo que permite activar el mouse en pantalla
    virtual public void MouseActivatedInPanel()
    {
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
#else
        Cursor.lockState = CursorLockMode.None;
        //Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
#endif

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    //Metodo que permite desactivar el mouse en pantalla
    virtual public void MouseDesctivatedOutOfPanel()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

using UnityEngine;

public class PanelBasic : MonoBehaviour, IPaneles
{
   
    //Metodo que permite activar el mouse en pantalla
    virtual public void MouseActivatedInPanel()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    //Metodo que permite desactivar el mouse en pantalla
    virtual public void MouseDesctivatedOutOfPanel()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
}

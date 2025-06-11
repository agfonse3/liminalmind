using UnityEngine;

public class InventoryPanel : PanelBasic
{
    [SerializeField] GameObject InventoryPannel; //panel activo
    private void OnEnable()
    {

        MouseActivatedInPanel();
    }

    public void ExitPanel()
    {
        InventoryPannel.SetActive(false);
    }
}

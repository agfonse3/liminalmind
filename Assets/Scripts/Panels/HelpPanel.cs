using UnityEngine;

public class HelpPanel : PanelBasic
{
    [SerializeField] GameObject helpPannel; //panel activo

    private void OnEnable()
    {
        MouseActivatedInPanel();
    }

    public void ExitPanel()
    {
        helpPannel.SetActive(false);
    }
}

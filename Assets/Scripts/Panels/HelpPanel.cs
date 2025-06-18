using UnityEngine;

public class HelpPanel : PanelBasic
{
    [SerializeField] GameObject helpPannel; //panel activo

    private void OnEnable()
    {
        MouseActivatedInPanel();
    }

    private void OnDisable()
    {
        if (GameManager.Instance.GetGamePause())
        {
            MouseDesctivatedOutOfPanel();
            GameManager.Instance.SetGameActive();
        }

    }

    public void ExitPanel()
    {
        Time.timeScale = 1.0f;
        helpPannel.SetActive(false);
    }
}

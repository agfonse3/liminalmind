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
        if (GameManager.Instance.isGamePaused)
        {
            MouseDesctivatedOutOfPanel();
            GameManager.Instance.isGamePaused = false;
        }

    }

    public void ExitPanel()
    {
        Time.timeScale = 1.0f;
        helpPannel.SetActive(false);
    }
}

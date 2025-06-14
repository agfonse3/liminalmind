using System.Collections;
using UnityEngine;

public class InventoryPanel : PanelBasic
{
    [SerializeField] GameObject InventoryPannel; //panel activo
    private InventoryManager inventoryManager;
    [SerializeField] private GameObject inventory;
    private bool isOnNote = false;

    private void Start()
    {
        inventoryManager = inventory.GetComponent<InventoryManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //si click
        {
            isOnNote=inventoryManager.IsClickedNearNote(Input.mousePosition);
            if (isOnNote) 
            {
                ShowNote();
                  AudiomanagerTemp.Instance.PlaySFX(AudiomanagerTemp.Instance.sfxPapel);
                StartCoroutine(DontShowNote());
            }
        }    
    }

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
        InventoryPannel.SetActive(false);
    }

    public void ShowNote() 
    {
        int numberOfNotes = inventoryManager.quantityOfNotes;
        for (int i = 0; i < numberOfNotes ; i++)
        {
            transform.GetChild(4).gameObject.transform.GetChild(i).gameObject.SetActive(true);
        }
        transform.GetChild(4).gameObject.SetActive(true);
    }

    IEnumerator DontShowNote() 
    {
        yield return new WaitForSeconds(3);
        transform.GetChild(4).gameObject.SetActive(false); 
    }




}

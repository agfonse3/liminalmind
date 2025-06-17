using UnityEngine;
using TMPro;

public class Interactuable : MonoBehaviour
{     
    public float distanciaInteractuar = 3f;
    public GameObject panelInteractuar;
   [SerializeField]  public GameObject panelUI;
    public GameObject textInteractuable;
    private Transform jugador;
    private bool dentroRango = false;
    public Light luzInteractuable;
    public GameObject inventary;
    InventoryManager inventoryManager;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        if (panelInteractuar != null) panelInteractuar.SetActive(false);
        if (textInteractuable != null) textInteractuable.SetActive(false);
        if (panelUI != null) panelUI.SetActive(false);

        if (inventary != null)
        {
            inventoryManager = inventary.GetComponent<InventoryManager>();
           
        }
        else
        {
            Debug.LogError("Inventory GameObject no asignado en " + gameObject.name);
        }
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector3.Distance(jugador.position, GetComponent<Collider>().bounds.center);

        if (distancia <= distanciaInteractuar)
        {
            if (!dentroRango)
            {
                dentroRango = true;
                MostrarIndicador(true);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Interactuar();
            }
        }
        else
        {
            if (dentroRango)
            {
                dentroRango = false;
                CerrarPanelUI();
            }
        }
    }

    void MostrarIndicador(bool mostrar)
    {
        if (panelInteractuar != null) panelInteractuar.SetActive(mostrar);
        if (textInteractuable != null) textInteractuable.SetActive(mostrar); 
        if (luzInteractuable != null) luzInteractuable.enabled = mostrar;
    }

    void Interactuar()
    {
        if (panelUI != null)
        {
            panelUI.SetActive(!panelUI.activeSelf);
            Time.timeScale = 1.0f;
            Cursor.lockState = panelUI.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = panelUI.activeSelf;

            if (textInteractuable != null)
            {
                textInteractuable.SetActive(false);
            }

            if (panelInteractuar != null)
            {
                panelInteractuar.SetActive(false);
            }
        }

        if (gameObject.CompareTag("Recolectable"))
        {
            if (inventoryManager != null)
            {
                ItemClass dataitem = gameObject.GetComponent<PickUp>()?.itemdata;
                if (dataitem != null)
                {
                    inventoryManager.AddItemsToInventory(dataitem);
                    AudiomanagerTemp.Instance.PlaySFX(AudiomanagerTemp.Instance.sfxRecolectar);
                    Debug.Log("Objeto agregado al inventario: " + dataitem.itemName);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogError("No se encontró `itemdata` en el objeto recolectable: " + gameObject.name);
                }
            }
            else
            {
                Debug.LogError("InventoryManager no está asignado en " + gameObject.name);
            }
        }
    }

    void CerrarPanelUI()
    {
        if (panelUI != null) panelUI.SetActive(false);
        if (panelInteractuar != null) panelInteractuar.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaInteractuar);
    }
}


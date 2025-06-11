using UnityEngine;
using UnityEngine.UI;

public class Interactuable : MonoBehaviour
{   
    public float distanciaInteractuar = 3f;
    public GameObject panelInteractuar;
    public GameObject panelUI;
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
        if (textInteractuable != null) textInteractuable.SetActive(true);
        if (panelUI != null) panelUI.SetActive(false);

        inventoryManager = inventary.GetComponent<InventoryManager>();
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
                MostrarIndicador(false);
            }
        }
    }

    void MostrarIndicador(bool mostrar)
    {
        if (panelInteractuar != null) panelInteractuar.SetActive(mostrar);
        if (luzInteractuable != null) luzInteractuable.enabled = mostrar;
        Cursor.lockState = mostrar ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = mostrar;
    }

    void Interactuar()
    {
        if (panelUI != null)
        {
            panelUI.SetActive(!panelUI.activeSelf);
            textInteractuable.SetActive(true);

            Cursor.lockState = panelUI.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = panelUI.activeSelf;
        }

        if (gameObject.CompareTag("Recolectable"))
        {
            ItemClass dataitem = gameObject.GetComponentInChildren<PickUp>().itemdata;
            if (dataitem != null)
            {
                inventoryManager.AddItemsToInventory(dataitem);
            }
        }
    }

    void CerrarPanelUI()
    {
        if (panelUI != null)
        {
            panelUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaInteractuar);
    }
}

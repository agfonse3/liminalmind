using UnityEngine;
using UnityEngine.UI;

public class Interactuable : MonoBehaviour
{    
    public float distanciaInteractuar = 3f;
    public GameObject panelInteractuar;
    public GameObject panelUI;
    public GameObject textInteractuable; // This is the text you want to show *before* interaction
    private Transform jugador;
    private bool dentroRango = false;
    public Light luzInteractuable;
    public GameObject inventary;
    InventoryManager inventoryManager;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        if (panelInteractuar != null) panelInteractuar.SetActive(false);
        // textInteractuable should also be initially false, and then activated by MostrarIndicador
        if (textInteractuable != null) textInteractuable.SetActive(false); 
        if (panelUI != null) panelUI.SetActive(false);

        // Ensure inventoryManager is properly assigned. Handle null case if 'inventary' might be null.
        if (inventary != null)
        {
            inventoryManager = inventary.GetComponent<InventoryManager>();
        }
        else
        {
            Debug.LogError("Inventory GameObject not assigned in Interactuable script on " + gameObject.name);
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
                MostrarIndicador(true); // Show interaction indicator and text
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
                CerrarPanelUI(); // Close UI panel if open
                MostrarIndicador(false); // Hide interaction indicator and text
            }
        }
    }

    void MostrarIndicador(bool mostrar)
    {
        if (panelInteractuar != null) panelInteractuar.SetActive(mostrar);
        if (textInteractuable != null) textInteractuable.SetActive(mostrar); // Activate/Deactivate text here
        if (luzInteractuable != null) luzInteractuable.enabled = mostrar;
        
        // Only unlock cursor if no other UI panel is active (like panelUI)
        // This prevents the cursor from unlocking if you interact with an object that brings up a panel
        if (!panelUI.activeSelf) 
        {
            Cursor.lockState = mostrar ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = mostrar;
        }
    }

    void Interactuar()
    {
        if (panelUI != null)
        {
            // Toggle panelUI active state
            panelUI.SetActive(!panelUI.activeSelf); 
            
            // Manage cursor based on panelUI's new state
            Cursor.lockState = panelUI.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = panelUI.activeSelf;

            // When panelUI is active, you might want to hide the "Press E" text
            // and show it again when panelUI is closed.
            if (textInteractuable != null)
            {
                textInteractuable.SetActive(!panelUI.activeSelf);
            }
        }

        if (gameObject.CompareTag("Recolectable"))
        {
            // Ensure inventoryManager is not null before trying to use it
            if (inventoryManager != null)
            {
                ItemClass dataitem = gameObject.GetComponentInChildren<PickUp>()?.itemdata;
                if (dataitem != null)
                {
                    inventoryManager.AddItemsToInventory(dataitem);
                    // Optionally, deactivate or destroy the collectible object after picking up
                    // gameObject.SetActive(false); 
                     Destroy(gameObject);
                }
            }
            else
            {
                Debug.LogWarning("InventoryManager is null. Cannot add item to inventory.");
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
        // When closing the UI panel, ensure the interaction text reappears if still in range
        if (dentroRango) 
        {
            MostrarIndicador(true); 
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaInteractuar);
    }
}
using UnityEngine;
using UnityEngine.UI; // Necesario para manejar UI
public class Interactuable : MonoBehaviour
{
    public float distanciaInteractuar = 3f;
    public GameObject panelInteractuar;
    public GameObject panelUI;
    //public string textoInteractuar = "Presiona E para interactuar";
    public GameObject textInteractuable;
    //public TMP_Text textoUI; 
    private Transform jugador;
    private bool dentroRango = false;
    public Light luzInteractuable;
    InventoryManager inventoryManager;
    //Inventario inventario;
    void Start()
    {
        //inventario = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventario>();
        jugador = GameObject.FindGameObjectWithTag("Player").transform; // Asegúrate de que tu jugador tenga el tag "Player"
        if (panelInteractuar != null)
        {
            panelInteractuar.SetActive(false);
            //if (textoUI != null)
            //{
            //    textoUI.text = textoInteractuarES;
            //}
            if (textInteractuable != null)
            {
                //textoUI.text = textoInteractuarES;
                textInteractuable.SetActive(true);
            }

        }
        if (panelUI != null)
        {
            panelUI.SetActive(false);
        }
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector3.Distance(jugador.position, GetComponent<Collider>().bounds.center);

        // Debug.Log("Distancia al objeto: " + distancia);

        if (distancia <= distanciaInteractuar)
        {
            if (!dentroRango)
            {
                dentroRango = true;
                //Debug.Log("Jugador dentro del rango de interacción.");
                MostrarIndicador(true); // Activar el indicador y luz
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Tecla E presionada.");
                Interactuar();
            }
        }
        else
        {
            if (dentroRango)
            {
                dentroRango = false;
                CerrarPanelUI();
                MostrarIndicador(false); // Desactivar el indicador y luz
                Debug.Log("Jugador salió del rango.");
             
            }
        }
    }

    void MostrarIndicador(bool mostrar)
    {
        if (panelInteractuar != null)
        {
            panelInteractuar.SetActive(mostrar);
        }
        if (luzInteractuable != null)
    {
        luzInteractuable.enabled = mostrar;
    }
        // Activar/desactivar el cursor cuando el panel cambia de estado
        Cursor.lockState = mostrar ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = mostrar;
    }

    void Interactuar()
    {
        if (panelUI != null)
        {
            panelUI.SetActive(!panelUI.activeSelf); // Alternar el panel UI
            textInteractuable.SetActive(true);

            // Desbloquear o bloquear el cursor dependiendo del estado del panel
            Cursor.lockState = panelUI.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = panelUI.activeSelf;
        }
        // Verificar si el objeto tiene el tag "Recolectable" antes de añadirlo al inventario
       if (gameObject.CompareTag("Recolectable"))
       {
            ItemClass dataitem = gameObject.GetComponentInChildren<PickUp>().itemdata;
            if (dataitem != null) 
            {
                inventoryManager.AddItemsToInventory(dataitem);
            }
            //GameManager.Instance.AgregarObjetoAlInventario(gameObject);
            Debug.Log("Objeto recogido: " + gameObject.name);
            //GameManager.Instance.MostrarInventario(); // Muestra el inventario después de recoger
            Destroy(gameObject); // Elimina el objeto de la escena
        }
         
        Debug.Log("Interacción con " + gameObject.name);
    }
    void CerrarPanelUI()
    {
        if (panelUI != null)
        {
            panelUI.SetActive(false); // Cerrar el panel
            Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor de nuevo
        Cursor.visible = false;
        }
    }
    // Para dibujar la esfera de interacción en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaInteractuar);
    }
}

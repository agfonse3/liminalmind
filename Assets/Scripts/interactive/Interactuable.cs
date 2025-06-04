using UnityEngine;
using UnityEngine.UI; // Necesario para manejar UI
using TMPro;
public class Interactuable : MonoBehaviour
{
    public float distanciaInteractuar = 3f;
    public GameObject panelInteractuar;
    public GameObject panelUI;
    public string textoInteractuar = "Presiona E para interactuar";
    public TMP_Text textoUI; // Si usas TextMeshPro, cambia a Text si usas el Text UI normal

    private Transform jugador;
    private bool dentroRango = false;
    //Inventario inventario;
    void Start()
    {
        //inventario = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventario>();
        jugador = GameObject.FindGameObjectWithTag("Player").transform; // Asegúrate de que tu jugador tenga el tag "Player"
        if (panelInteractuar != null)
        {
            panelInteractuar.SetActive(false);
            if (textoUI != null)
            {
                textoUI.text = textoInteractuar;
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
            Debug.Log("Jugador dentro del rango de interacción.");
            MostrarIndicador(true);
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
            Debug.Log("Jugador salió del rango.");
            MostrarIndicador(false);
        }
    }
    }

    void MostrarIndicador(bool mostrar)
    {
        if (panelInteractuar != null)
        {
            panelInteractuar.SetActive(mostrar);
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

            // Desbloquear o bloquear el cursor dependiendo del estado del panel
            Cursor.lockState = panelUI.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = panelUI.activeSelf;
        }
        // Verificar si el objeto tiene el tag "Recolectable" antes de añadirlo al inventario
    if (gameObject.CompareTag("Recolectable"))
    {
        //if (inventario != null) // Evitar el NullReferenceException
        //{
        //    inventario.Cantidad += 1;
        //    Debug.Log("Objeto recogido: " + gameObject.name);
        //    Destroy(gameObject);
        //}
        //else
        //{
        //    Debug.LogError("Inventario no está inicializado.");
        //}
    }
        // Aquí puedes añadir más lógica de interacción
        Debug.Log("Interacción con " + gameObject.name);
    }

    // Para dibujar la esfera de interacción en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaInteractuar);
    }
}

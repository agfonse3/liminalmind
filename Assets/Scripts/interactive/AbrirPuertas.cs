using UnityEngine;
using TMPro;
public class AbrirPuertas : MonoBehaviour
{
    private Animator animator;
    private bool jugadorEnZona = false;

    public GameObject panelUI; // Panel que muestra el mensaje
    public TextMeshProUGUI textoMensaje; // Texto dentro del panel
    public string mensaje = "Presiona la E para abrir la puerta"; // Mensaje a mostrar

    void Start()
    {
        animator = GetComponent<Animator>();

        if (panelUI != null)
        {
            panelUI.SetActive(false); // Asegura que el mensaje no aparezca hasta que el jugador entre en la zona
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnZona = true;
            MostrarTexto(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnZona = false;
            MostrarTexto(false);
        }
    }

    void Update()
    {
        if (jugadorEnZona && Input.GetKeyDown(KeyCode.E))
        {
            animator.Play("puertasinllave");
            MostrarTexto(false); // Oculta el mensaje cuando la puerta se abre
            Debug.Log("Puerta abierta");
        }
    }

    private void MostrarTexto(bool activar)
    {
        if (panelUI != null)
        {
            textoMensaje.text = mensaje;
            panelUI.SetActive(activar);
        }
    }
    
}

using UnityEngine;
using TMPro;
public class AbrirPuertaSencillaLLave : MonoBehaviour
{
     private Animator animator;
    private bool jugadorEnZona = false;

    public GameObject panelUI; // Panel que muestra el mensaje
    public TextMeshProUGUI textoMensaje; // Texto dentro del panel
    public GameObject textInteractuable; // GameObject del texto interactivo
    public GameObject puerta; // Referencia a la puerta que requiere llave
    public GameObject mensajeSinLlave; // Objeto que muestra el mensaje cuando falta la llave

    void Start()
    {
        animator = GetComponent<Animator>();

        if (panelUI != null)
        {
            panelUI.SetActive(false); // Oculta el mensaje hasta que el jugador entre en la zona
        }

        if (mensajeSinLlave != null)
        {
            mensajeSinLlave.SetActive(false); // Asegura que el mensaje esté oculto al inicio
        }
    }

    void Update()
    {
        if (jugadorEnZona && Input.GetKeyDown(KeyCode.E))
        {
            bool tieneLlave = puerta.GetComponent<DoorRequirement>().HasKeyToOpen(); // Comprueba si el jugador tiene la llave

            if (tieneLlave)
            {
                AbrirPuerta();
            }
            else
            {
                MostrarMensajeSinLlave();
            }
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

    private void MostrarTexto(bool activar)
{
    if (panelUI != null)
    {
        panelUI.SetActive(activar);
    }

    if (textInteractuable != null)
    {
        textInteractuable.SetActive(activar);
    }

    if (!activar && mensajeSinLlave != null)
    {
        mensajeSinLlave.SetActive(false); // Asegura que el mensaje de falta de llave se oculta cuando el jugador deja la zona
    }
}


    private void MostrarMensajeSinLlave()
    {
         if (mensajeSinLlave != null)
    {
        mensajeSinLlave.SetActive(true); // Activa el mensaje de falta de llave
    }

    if (textInteractuable != null)
    {
        textInteractuable.SetActive(false); // Oculta el texto interactivo cuando falta la llave
    }
        Debug.Log("El jugador intentó abrir la puerta sin la llave.");
    }

    private void OcultarMensajeSinLlave()
    {
        if (mensajeSinLlave != null)
        {
            mensajeSinLlave.SetActive(false);
        }
    }

    private void AbrirPuerta()
    {
        animator.Play("puertasencilla"); // Reproduce la animación
        panelUI.SetActive(false);
        textInteractuable.SetActive(false);
        Debug.Log("La puerta se ha abierto.");
    }
}



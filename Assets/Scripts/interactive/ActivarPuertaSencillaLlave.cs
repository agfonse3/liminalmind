using UnityEngine;
using TMPro;

public class AbrirPuertaSencillaLLave : MonoBehaviour
{
    private Animator animator;
    private bool jugadorEnZona = false;

    public GameObject panelUI;
    public TextMeshProUGUI textoMensaje;
    public GameObject textInteractuable;
    public GameObject puerta;
    public GameObject mensajeSinLlave;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (panelUI != null) panelUI.SetActive(false);
        if (mensajeSinLlave != null) mensajeSinLlave.SetActive(false);
    }

    void Update()
    {
        if (jugadorEnZona && Input.GetKeyDown(KeyCode.E))
        {
            bool tieneLlave = puerta.GetComponent<DoorRequirement>().HasKeyToOpen();

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
            OcultarTextos();
        }
    }

    private void MostrarTexto(bool activar)
    {
        if (panelUI != null) panelUI.SetActive(activar);

        if (textInteractuable != null)
        {
            textInteractuable.SetActive(activar);
        }

        if (!activar && mensajeSinLlave != null)
        {
            mensajeSinLlave.SetActive(false);
        }
    }

    private void MostrarMensajeSinLlave()
    {
        if (mensajeSinLlave != null)
        {
            mensajeSinLlave.SetActive(true);
        }

        if (textInteractuable != null)
        {
            textInteractuable.SetActive(false);
        }

        Debug.Log("El jugador intentó abrir la puerta sin la llave.");
    }

    private void OcultarTextos()
    {
        if (mensajeSinLlave != null) mensajeSinLlave.SetActive(false);
        if (textInteractuable != null) textInteractuable.SetActive(true);
    }

    private void AbrirPuerta()
    {
        animator.Play("puertasencilla");
        panelUI.SetActive(false);
        textInteractuable.SetActive(false);
        Debug.Log("La puerta se ha abierto.");
    }
}

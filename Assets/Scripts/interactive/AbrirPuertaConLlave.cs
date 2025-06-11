using UnityEngine;
using TMPro;
public class AbrirPuertaConLlave : MonoBehaviour
{
    private Animator animator;
    private bool jugadorEnZona = false;

    public GameObject panelUI; // Panel que muestra el mensaje
    //public TextMeshProUGUI textoMensaje; // Texto dentro del panel
    public GameObject textInteractuable; // gameobject del texto
    public GameObject textLlave; // gameobject del texto de la llave

    //public string mensaje = "Presiona la E para abrir la puerta"; // Mensaje a mostrar
    //public GameObject llaveNecesaria; // Referencia directa al objeto
    public GameObject puerta;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontró un Animator en el objeto: " + gameObject.name);
        }

        if (panelUI != null)
        {
            panelUI.SetActive(false); // Oculta el mensaje hasta que el jugador entre en la zona
        }
    }

    void Update()
    {
        if (jugadorEnZona && Input.GetKeyDown(KeyCode.E))
        {
            //if (llaveNecesaria == null)
            //{
            //    Debug.LogError("La referencia a 'llaveNecesaria' no está asignada en " + gameObject.name);
            //    return;
            //}
            bool hasKey = puerta.GetComponent<DoorRequirement>().HasKeyToOpen(); // rectifica si tiene la llave que requiere la puerta


            Debug.Log("Intentando abrir la puerta con: " + puerta.GetComponent<DoorRequirement>().itemdata.itemName);

            //if (GameManager.Instance != null && GameManager.Instance.TieneObjeto(llaveNecesaria))
            //{
            //    AbrirPuerta();
            //}
            //else
            //{
            //    Debug.Log("No tienes el objeto necesario.");
            //}

            if (hasKey)
            {
                AbrirPuerta();
            }
            else
            {
                Debug.Log("No tienes el objeto necesario.");
                textLlave.SetActive(true);
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
            if (activar)
            {
                //textoMensaje.text = mensaje;
                textInteractuable.SetActive(true);
            }
            else 
            {
                textInteractuable.SetActive(false);
            }
        }
    }

    void AbrirPuerta()
    {
        animator.Play("puerta azul"); // Reproduce la animación de apertura
        if (panelUI != null)
        {
            panelUI.SetActive(false);
            textInteractuable.SetActive(false);
        }
        Debug.Log("La puerta se ha abierto.");
    }
}

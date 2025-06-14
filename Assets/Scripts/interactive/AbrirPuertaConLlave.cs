using UnityEngine;
using TMPro;

public class AbrirPuertaConLlave : MonoBehaviour
{
    
 private Animator animator;
    private bool jugadorEnZona = false;

    public GameObject panelUI;
    public GameObject textInteractuable;
    public GameObject textLlave;
    public GameObject puerta;
    public Animator puertaAnimator; // Se asigna el Animator desde el Inspector

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("No se encontró un Animator en el objeto: " + gameObject.name);
        }

        if (panelUI != null) panelUI.SetActive(false);
        if (textLlave != null) textLlave.SetActive(false);
    }

    void Update()
    {
        if (jugadorEnZona && Input.GetKeyDown(KeyCode.E))
        {
            bool hasKey = puerta.GetComponent<DoorRequirement>().HasKeyToOpen();

            Debug.Log("Intentando abrir la puerta con: " + puerta.GetComponent<DoorRequirement>().itemdata.itemName);

            if (hasKey)
            {
                AbrirPuerta();
            }
            else
            {
                AudiomanagerTemp.Instance.PlaySFX(AudiomanagerTemp.Instance.sfxError);
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
            textInteractuable.SetActive(activar && (textLlave == null || !textLlave.activeSelf));
            Debug.Log("Estado de textInteractuable: " + textInteractuable.activeSelf);
        }

        if (!activar && textLlave != null)
        {
            textLlave.SetActive(false);
        }
    }

    private void MostrarMensajeSinLlave()
    {
        if (textLlave != null)
        {
            textLlave.SetActive(true);
        }

        if (textInteractuable != null)
        {
            textInteractuable.SetActive(false);
        }

        Debug.Log("No tienes la llave para abrir la puerta.");
    }

    private void OcultarTextos()
    {
        if (textInteractuable != null) textInteractuable.SetActive(false);
        if (textLlave != null) textLlave.SetActive(false);
        if (panelUI != null) panelUI.SetActive(false);
    }

    void AbrirPuerta()
    {
        if (puertaAnimator != null)
        {
            puertaAnimator.SetTrigger("Abrir"); // Dispara la animación con un Trigger desde el AnimatorController
            Debug.Log("Activando animación en puerta: " + puertaAnimator.gameObject.name);
            AudiomanagerTemp.Instance.PlaySFX(AudiomanagerTemp.Instance.sfxPuertaAbrir);
        }
        else
        {
            Debug.LogError("No se ha asignado un Animator para esta puerta.");
        }

        OcultarTextos();
        Debug.Log("La puerta se ha abierto.");
    }
/*    private Animator animator;
        private bool jugadorEnZona = false;

        public GameObject panelUI;
        public GameObject textInteractuable;
        public GameObject textLlave;
        public GameObject puerta;

        private void Start()
        {
            animator = GetComponent<Animator>();

            if (panelUI != null) panelUI.SetActive(false);
            if (textLlave != null) textLlave.SetActive(false);
        }

        void Update()
        {
            if (jugadorEnZona && Input.GetKeyDown(KeyCode.E))
            {
                bool hasKey = puerta.GetComponent<DoorRequirement>().HasKeyToOpen();

                Debug.Log("Intentando abrir la puerta con: " + puerta.GetComponent<DoorRequirement>().itemdata.itemName);

                if (hasKey)
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
                textInteractuable.SetActive(activar && (textLlave == null || !textLlave.activeSelf));
                Debug.Log("Estado de textInteractuable: " + textInteractuable.activeSelf);
            }

            if (!activar && textLlave != null)
            {
                textLlave.SetActive(false);
            }
        }

        private void MostrarMensajeSinLlave()
        {
            if (textLlave != null)
            {
                textLlave.SetActive(true);
            }

            if (textInteractuable != null)
            {
                textInteractuable.SetActive(false);
            }

            Debug.Log("No tienes la llave para abrir la puerta.");
        }

        private void OcultarTextos()
        {
            if (textInteractuable != null) textInteractuable.SetActive(false);
            if (textLlave != null) textLlave.SetActive(false);
            if (panelUI != null) panelUI.SetActive(false);
        }

        void AbrirPuerta()
        {
            animator.Play("puerta azul");

            OcultarTextos();

            Debug.Log("La puerta se ha abierto.");
        }*/
}

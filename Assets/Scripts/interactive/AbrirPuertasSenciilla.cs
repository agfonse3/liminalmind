    using UnityEngine;
    using TMPro;
    public class AbrirPuertasSencilla : MonoBehaviour
    {
        private Animator animator;
        private bool jugadorEnZona = false;

        public GameObject panelUI;
        public GameObject textInteractuable;
        public Animator puertaAnimator; // Se asigna el Animator desde el Inspector

        private void Start()
        {
            animator = GetComponent<Animator>();

            if (animator == null)
            {
                Debug.LogError("No se encontró un Animator en el objeto: " + gameObject.name);
            }

            if (panelUI != null) panelUI.SetActive(false);
        }

        void Update()
        {
            if (jugadorEnZona && Input.GetKeyDown(KeyCode.E))
            {
                AbrirPuerta();
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
                Debug.Log("Estado de textInteractuable: " + textInteractuable.activeSelf);
            }
        }

        private void OcultarTextos()
        {
            if (textInteractuable != null) textInteractuable.SetActive(false);
            if (panelUI != null) panelUI.SetActive(false);
        }

        void AbrirPuerta()
        {
            if (puertaAnimator != null)
            {
                puertaAnimator.SetTrigger("Abrir"); // Dispara la animación con un Trigger
                Debug.Log("Activando animación en puerta: " + puertaAnimator.gameObject.name);
            }
            else
            {
                Debug.LogError("No se ha asignado un Animator para esta puerta.");
            }

            OcultarTextos();
            Debug.Log("La puerta se ha abierto.");
        }
    }

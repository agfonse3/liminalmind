using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneInitializer : MonoBehaviour
{
    void Start()
    {
        //busca en la escena actual desde la raiz el panel de GameOver
        Scene escenaActual = SceneManager.GetActiveScene();
        GameObject[] objetosRaiz = escenaActual.GetRootGameObjects();

        foreach (GameObject raiz in objetosRaiz)
        {
            Transform panelEncontrado = BuscarRecursivamente(raiz.transform, "GameOverPanel");

            if (panelEncontrado != null)
            {
                GameManager.Instance.SetGameOverPanelReference(panelEncontrado.gameObject);
                break;
            }
        }
    }

    //Busca un hijo por nombre en toda la jerarquía de la escena activa (sin importar el nivel de anidación)
    private Transform BuscarRecursivamente(Transform padre, string nombreBuscado)
    {
        foreach (Transform hijo in padre)
        {
            if (hijo.name == nombreBuscado)
                return hijo;

            Transform resultado = BuscarRecursivamente(hijo, nombreBuscado);
            if (resultado != null)
                return resultado;
        }
        return null;
    }
}

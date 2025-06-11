using UnityEngine;

public class ElevatorPanelController : MonoBehaviour
{
    public void OfficePressed()
    {
        AudiomanagerTemp.Instance.PlaySFX(AudiomanagerTemp.Instance.sfxBotonAscensor);
        Debug.Log("Botón Oficina presionado");

    }

    public void AppartmentPressed()
    {
        AudiomanagerTemp.Instance.PlaySFX(AudiomanagerTemp.Instance.sfxBotonAscensor);
        Debug.Log("Botón Apartamento presionado");
       
    }
}
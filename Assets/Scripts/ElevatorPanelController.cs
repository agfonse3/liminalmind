using UnityEngine;

public class ElevatorPanelController : MonoBehaviour
{

    public void OfficePressed()
    { 
        AudiomanagerTemp.Instance.PlayElevatorButton(); // sonido de botón
        AudiomanagerTemp.Instance.PlayOfficeAmbience(); // ambiente oficina
    }

    public void AppartmentPressed()
    { 
        AudiomanagerTemp.Instance.PlayElevatorButton(); // sonido de botón
        AudiomanagerTemp.Instance.PlayApartmentAmbience(); // ambiente apartamento
    }


}

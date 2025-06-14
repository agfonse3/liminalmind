using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlaySoundOnClick : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (AudiomanagerTemp.Instance != null)
        {
            AudiomanagerTemp.Instance.PlaySFX(AudiomanagerTemp.Instance.sfxBotonMenu);
        }
    }
}
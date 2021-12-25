using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Gun gun;
    private void Start()
    {
        gun = FindObjectOfType<Gun>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gun.UpdateHovered(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gun.UpdateHovered(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] PlayerController player;

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.dragging) {
            if (CheckIfTouchIsLeftSide(eventData.position) && CheckIfTouchIsBottomHalf(eventData.position)) {
                player.Dive(false);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CheckIfTouchIsLeftSide(eventData.pointerCurrentRaycast.screenPosition)) {
            if (CheckIfTouchIsBottomHalf(eventData.pointerCurrentRaycast.screenPosition)) {
                player.Dive(false);
            } else {
                player.Jump(false);
            }
        } else {
            player.Shoot(false);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    { 
        if (CheckIfTouchIsLeftSide(eventData.pointerCurrentRaycast.screenPosition)) {
            if (CheckIfTouchIsBottomHalf(eventData.pointerCurrentRaycast.screenPosition)) {
                player.StopDive(false);
            } else {
                player.PointerUp();
            }
        }
    }

    private bool CheckIfTouchIsLeftSide(Vector2 touch)
    {
        return (touch.x <= (Screen.width / 2));
    }

    private bool CheckIfTouchIsBottomHalf(Vector2 touch)
    {
        return (touch.y <= (Screen.height / 2) - 200);
    }
}

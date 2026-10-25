using UnityEngine;

public class TouchHold : MonoBehaviour, UnityEngine.EventSystems.IPointerDownHandler, UnityEngine.EventSystems.IPointerUpHandler, UnityEngine.EventSystems.IPointerExitHandler
{
    public string key; public static readonly HashSet<string> Held=new HashSet<string>();
    public void OnPointerDown(UnityEngine.EventSystems.PointerEventData e){Held.Add(key);} public void OnPointerUp(UnityEngine.EventSystems.PointerEventData e){Held.Remove(key);} public void OnPointerExit(UnityEngine.EventSystems.PointerEventData e){Held.Remove(key);}
}

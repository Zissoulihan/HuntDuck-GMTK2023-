using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TaroH : MonoBehaviour
{
    // Credit: Tarodev
    //  https://www.youtube.com/watch?v=JOABOQMurZo

    //Cache Main Camera reference
    private static Camera _camera;
    public static Camera Camera
    {
        get
        {
            if (_camera == null) _camera = Camera.main;
            return _camera;
        }
    }

    //WaitForSeconds NonAlloc
    //  Prevents garbage buildup on redundant WaitForSeconds
    private static readonly Dictionary<float, WaitForSeconds> _waitDictionary = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWait(float time)
    {
        if (_waitDictionary.TryGetValue(time, out WaitForSeconds wait)) return wait;

        _waitDictionary[time] = new WaitForSeconds(time);
        return _waitDictionary[time];
    }

    //Is Pointer Over UI?
    //  May not work with "new" input system? We'll see...

    private static PointerEventData _pointerEventData;
    private static List<RaycastResult> _results;
    public static bool IsPointerOverUI()
    {
        _pointerEventData = new PointerEventData(EventSystem.current) { position = Mouse.current.position.ReadValue() };
        _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_pointerEventData, _results);
        return _results.Count > 0;
    }

    //Find World Point of Canvas Element
    //  Good fucking god did we need this years ago

    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera, out var result);
        return result;
    }

    //Commit Prolicide
    //  Technically not, since this method destroys the children of any transform, not exclusively its own
    //  The correct term would be "pedicide" but ew
    //  Also weirdly "filicide" also refers to the act of murdering one's own children, but can also be used
    //   as a noun to refer to one who commits said act, eg. the convicted filicide was hanged
    //  But anyway

    public static void DeleteChildren(Transform t)
    {
        foreach (Transform child in t) {
            Destroy(child.gameObject);
        }
    }


}

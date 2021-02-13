using UnityEngine;

public class AttachToMouse : MonoBehaviour
{
    private void Update()
    {
        transform.position = GetMouseInWorldPosition();
    }

    private Vector3 GetMouseInWorldPosition()
    {
        Vector3 cursorInScreen = Input.mousePosition;
        Vector3 cursorInWorld = Camera.main.ScreenToWorldPoint(cursorInScreen);
        cursorInWorld.z = 0.0f;
        return cursorInWorld;
    }
}

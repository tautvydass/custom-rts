using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    private Vector2 startingMousePosition;
    private bool selecting = false;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startingMousePosition = Input.mousePosition;
            selecting = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            selecting = false;
        }
    }

    private void OnGUI()
    {
        if(selecting)
        {
            var rect = Utils.GetScreenRect(startingMousePosition, Input.mousePosition);
            Utils.DrawScreenRectBorder(rect, 1, Color.green);
        }
    }
}

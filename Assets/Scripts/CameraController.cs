using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float speed;

    private void Update()
    {
        var delta = Vector3.zero;
        var speed = this.speed * Time.deltaTime;

        if(Input.mousePosition.x < 1)
        {
            delta.x -= speed;
        }
        if(Input.mousePosition.x > Screen.width - 1)
        {
            delta.x += speed;
        }

        if (Input.mousePosition.y < 1)
        {
            delta.z -= speed;
        }
        if (Input.mousePosition.y > Screen.height - 1)
        {
            delta.z += speed;
        }

        target.position += delta;
    }
}

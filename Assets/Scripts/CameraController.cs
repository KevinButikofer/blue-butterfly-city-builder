using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float panSpeed = 15f;
    [SerializeField]
    private float panBorderThickness = 40f;
    [SerializeField]
    private Vector2 panLimit;

    [SerializeField]
    private float scrollsSpeed = 20f;
    [SerializeField]
    private float minY = 20f;
    [SerializeField]
    private float maxY = 50f;

    [SerializeField]
    private float rotSpeed = 5f;
    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        float rot = 0f;
        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            rot = 1.0f * rotSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rot = -1.0f * rotSpeed * Time.deltaTime;
        }


        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollsSpeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        //transform.localEulerAngles.Set()
        transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + rot, transform.localEulerAngles.z);
        transform.position = pos;
    }
}

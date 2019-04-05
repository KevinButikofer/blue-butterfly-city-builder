using UnityEngine;
using UnityEngine.EventSystems;


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
    private Vector3 originalPos;

    Vector3 fixedPoint;
    bool rotateLeft = false;
    bool rotateRight = false;
    // Update is called once per frame

    private void Start()
    {
        originalPos = transform.position;
    }
    void Update()
    {
        RaycastHit hit;
        Ray ray = this.transform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        Vector3 pos = transform.position;
        float y = pos.y;
        float rot = 0f;
        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos += Camera.main.transform.forward * panSpeed * Time.deltaTime;
            pos.y = y;
        }
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThickness)
        {
            pos -= Camera.main.transform.forward * panSpeed * Time.deltaTime;
            pos.y = y;
        }
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThickness)
        {
            pos -= Camera.main.transform.right * panSpeed * Time.deltaTime;
            pos.y = y;
        }
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos -= Camera.main.transform.right * panSpeed * Time.deltaTime * -1;
            pos.y = y;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!rotateLeft)
            {
                Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo);
                fixedPoint = hitInfo.point;
                rotateLeft = true;
            }

            pos -= Camera.main.transform.right * panSpeed * Time.deltaTime;
            pos.y = y;
            transform.LookAt(fixedPoint);
        }
        else
        {
            rotateLeft = false;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (!rotateRight)
            {
                Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo);
                fixedPoint = hitInfo.point;
                rotateRight = true;
            }

            pos += Camera.main.transform.right * panSpeed * Time.deltaTime;
            pos.y = y;
            transform.LookAt(fixedPoint);
        }
        else
        {
            rotateRight = false;
        }


        //float scroll = Input.GetAxis("Mouse ScrollWheel");
        //pos.y -= scroll * scrollsSpeed * 100f * Time.deltaTime;

        //if (pos.y > minY && pos.y < maxY)
        //pos.z += scroll * scrollsSpeed * 100f * Time.deltaTime;


        if (Input.GetAxis("Mouse ScrollWheel") > 0 && pos.y > minY && !EventSystem.current.IsPointerOverGameObject() || Input.GetAxis("Mouse ScrollWheel") < 0 && pos.y < maxY && !EventSystem.current.IsPointerOverGameObject()) // Zoom
        {

            Vector3 desiredPosition;

            if (Physics.Raycast(ray, out hit))
            {
                desiredPosition = hit.point;
            }
            else
            {
                desiredPosition = transform.position;
            }
            float distance = Vector3.Distance(desiredPosition, transform.position);
            Vector3 direction = Vector3.Normalize(desiredPosition - transform.position) * (distance * Input.GetAxis("Mouse ScrollWheel"));

            transform.position += direction;
        }
        else // Déplacement
        {
            pos.x = Mathf.Clamp(pos.x, -panLimit.x + originalPos.x, panLimit.x + originalPos.x);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            pos.z = Mathf.Clamp(pos.z, -panLimit.y + originalPos.z, panLimit.y + originalPos.z);

            transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + rot, transform.localEulerAngles.z);
            transform.position = pos;
        }



    }
}

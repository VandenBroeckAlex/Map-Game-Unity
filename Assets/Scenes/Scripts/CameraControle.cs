using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 1.0f;
    public float ScrollSpeed = 10.0f;

    public Vector2 xLimits = new Vector2(0, 1000);
    public Vector2 zLimits = new Vector2(-1000, 0);
    public Vector2 yLimits = new Vector2(8, 250); 
    private float horizontalInput;
    private float verticalInput;
    private float scrollInput;

    private void Start()
    {
        // x => + y => -
        
        transform.position = new Vector3(MapLoader.canvaWidth, MapLoader.canvaHeight/2, MapLoader.canvaHeight);
    }


    void Update()
    {
        // Get inputs
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Move camera
        transform.Translate(Vector3.forward * Time.deltaTime * MovementSpeed * verticalInput, Space.World);
        transform.Translate(Vector3.right * Time.deltaTime * MovementSpeed * horizontalInput);
        transform.Translate(Vector3.up * Time.deltaTime * ScrollSpeed * -scrollInput, Space.World);

        // Clamp position
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, xLimits.x, xLimits.y);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, zLimits.x, zLimits.y);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, yLimits.x, yLimits.y); // Optional zoom clamping
        transform.position = clampedPosition;
    }
}

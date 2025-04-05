using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 1.0f;
    public float ScrollSpeed = 10.0f;
    private float horizontalInput;
    private float verticalInput;
    private float scrollInput;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //get player inputs
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Translate 
        transform.Translate(Vector3.forward * Time.deltaTime * MovementSpeed * verticalInput, Space.World);
        // Translate 
        transform.Translate(Vector3.right * Time.deltaTime * MovementSpeed * horizontalInput);
        //scroll zoom
        transform.Translate(Vector3.up * Time.deltaTime * ScrollSpeed *  - scrollInput, Space.World);



    }
}

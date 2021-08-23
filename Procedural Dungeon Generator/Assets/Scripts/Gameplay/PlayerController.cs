using UnityEngine;

// The script is for handling the player's input  
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("How fast the player can move")]
    [SerializeField] private Vector2 m_speed = new Vector2(50, 50);

    [Header("Axis")]
    [Tooltip("Horizontal Axis Name")]
    [SerializeField] private string m_horizontalAxis = "Horizontal";

    [Tooltip("Vertical Axis Name")]
    [SerializeField] private string m_verticalAxis = "Vertical";

    // The player's start position
    private Vector3 m_standardPos;

    private void Start()
    {
        m_standardPos = transform.position;
    }

    private void Update()
    {
        Movement();
    }

    public void Reset()
    {
        transform.position = m_standardPos;
    }

    // Move horizontally && vertically
    private void Movement()
    {
        float inputX = Input.GetAxis(m_horizontalAxis);
        float inputY = Input.GetAxis(m_verticalAxis);
        Vector3 movement = new Vector3(m_speed.x * inputX, m_speed.y * inputY);
        movement *= Time.deltaTime;
        transform.Translate(movement);
    }
}

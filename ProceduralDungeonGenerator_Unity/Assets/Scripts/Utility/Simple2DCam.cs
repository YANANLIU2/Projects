using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A simple script to control a 2D camera movement
// Reference for 2D camera zooming: https://answers.unity.com/questions/827105/smooth-2d-camera-zoom.html
public class Simple2DCam : MonoBehaviour
{
    [Tooltip("Camera Movement Speed")]
    [SerializeField] private float m_moveSpeed = 0;

    [Tooltip("If true the camera cannot move horizontally")]
    [SerializeField] private bool m_isLockXAxis = false;

    [Tooltip("If true the camera cannot move vertically")]
    [SerializeField] private bool m_isLockYAxis = false;

    [Tooltip("If true the camera can only move with in the box")]
    [SerializeField] private bool m_isUsingRangeBox = false;

    [Tooltip("The size of the box")]
    [SerializeField] private Box m_rangeBox;

    [Tooltip("The axis name of mouse scroll")]
    [SerializeField] private string m_scrollAxisName = "Mouse ScrollWheel";

    [Tooltip("Zooming speed")]
    [SerializeField] private float m_zoomSpeed = 1;
    
    [Tooltip("The smooth speed of camera zooming")]
    [SerializeField] private float m_smoothSpeed = 2.0f;

    [Tooltip("The minimum orthographic size of the camera")]
    [SerializeField] private float m_minOrtho = 1.0f;

    [Tooltip("The maximum orthographic size of the camera")]
    [SerializeField] private float m_maxOrtho = 20.0f;

    // standard orthographic size of the camera
    private float m_targetOrtho;
    private Vector3 m_targetPosition;

    [System.Serializable]
    private struct Box
    {
        [SerializeField] public float Top;
        [SerializeField] public float Bot;
        [SerializeField] public float Left;
        [SerializeField] public float Right;
    }

    private void Start()
    {
        m_targetOrtho = Camera.main.orthographicSize;
        m_targetPosition = transform.position;
    }
    // Called every frame
    void Update()
    {
        Movement();
        if (m_isUsingRangeBox)
        {
            CheckMovementRange();
        }
        Zoom();
    }

    // Right, left, up and down
    void Movement()
    {
        if(!m_isLockYAxis)
        {
            if (Input.GetKey(KeyCode.W))
            {
                Move(Vector3.up);
            }
            if (Input.GetKey(KeyCode.S))
            {
                Move(-Vector3.up);
            }
        }

        if (!m_isLockXAxis)
        {
            if (Input.GetKey(KeyCode.A))
            {
                Move(-Vector3.right);
            }
            if (Input.GetKey(KeyCode.D))
            {
                Move(Vector3.right);
            }
        }
    }

    // Move to the direction
    private void Move(Vector3 direction)
    {
        transform.position += direction * m_moveSpeed * Time.deltaTime;
    }

    // Restrain the camera's position inside a box
    private void CheckMovementRange()
    {
        Vector3 position = transform.position;
        if (position.y < m_rangeBox.Bot)
        {
            position.y = m_rangeBox.Bot;
        }
        else if (position.y > m_rangeBox.Top)
        {
            position.y = m_rangeBox.Top;
        }
        if (position.x < m_rangeBox.Left)
        {
            position.x = m_rangeBox.Left;
        }
        else if (position.x > m_rangeBox.Right)
        {
            position.x = m_rangeBox.Right;
        }
        transform.position = position;
    }

    // Camera zoom in and out
    private void Zoom()
    {
        float scroll = Input.GetAxis(m_scrollAxisName);
        if (scroll != 0.0f)
        {
            m_targetOrtho -= scroll * m_zoomSpeed;
            m_targetOrtho = Mathf.Clamp(m_targetOrtho, m_minOrtho, m_maxOrtho);
        }
        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, m_targetOrtho, m_smoothSpeed * Time.deltaTime);
    }

    // Reset the camera to the original state
    public void Reset()
    {
        transform.position = m_targetPosition;
        Camera.main.orthographicSize = m_targetOrtho;
    }
}

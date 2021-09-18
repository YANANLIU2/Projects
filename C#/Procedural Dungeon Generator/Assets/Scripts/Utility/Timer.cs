using UnityEngine;
using UnityEngine.UI;

// The script is for providing a simple timer to any class needs it
public class Timer 
{
    // The total time
    private float m_duration;

    // The time has passed since counting down
    private float m_elapsedTime;

    // The the timer counting down
    private bool m_isActive;

    // Is is a looping timer
    private bool m_isLoop;

    // Getters & Setters
    public bool IsActive
    {
        get => m_isActive;
        set => m_isActive = value;
    }

    public bool IsLoop
    {
        set => m_isLoop = value;
    }

    public float Duration
    {
        set => m_duration = value;
    }

    // Constructor
    public Timer(float duration, float elapsedTime = 0, bool isLoop = false)
    {
        m_duration = duration;
        m_elapsedTime = elapsedTime;
        m_isActive = false;
        m_isLoop = isLoop;
    }

    // Update
    public void Update()
    {
        if (m_isActive)
        {
            // If the timer is not finished yet
            if (m_elapsedTime < m_duration)
            {
                m_elapsedTime += Time.deltaTime;
            }
            // Otherwise reset the timer
            else
            {
                m_elapsedTime -= m_duration;
                m_isActive = m_isLoop; // Loop or not
            }
        }
    }
}

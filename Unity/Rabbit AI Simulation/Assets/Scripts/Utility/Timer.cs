using UnityEngine;

[System.Serializable]
public class Timer 
{
    [SerializeField] private float duration_;
    private float elapsed_time_;
    private bool b_is_finished_;
    
    // Getters & Setters
    public float Duration { get => duration_; set => duration_ = value; }
    public float ElapsedTime { get => elapsed_time_; }
    public bool IsFinished { get => b_is_finished_; }

    // Constructors
    public Timer() { }
    public Timer(float duration, float elapsed_time = 0, bool is_finished = false)
    {
        duration_ = duration;
        elapsed_time_ = elapsed_time;
        b_is_finished_ = is_finished;
    }
    
    // Update the timer, and return true if the timer is finished
    public bool Update()
    {
        if(!b_is_finished_)
        {
            elapsed_time_ += Time.deltaTime;
            if(elapsed_time_ > duration_)
            {
                elapsed_time_ -= duration_;
                b_is_finished_ = true;
            }
        }
        return b_is_finished_;
    }
}

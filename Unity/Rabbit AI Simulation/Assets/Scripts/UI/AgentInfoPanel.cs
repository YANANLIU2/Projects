using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentInfoPanel : MonoBehaviour
{
    [SerializeField] private Agent agent_ = null;
    [SerializeField] private Slider hunger_ = null;
    [SerializeField] private Slider energy_ = null;
    [SerializeField] private Slider bladder_ = null;
    [SerializeField] private Slider thirst_ = null;
    [SerializeField] private Text action_text_ = null;
    [SerializeField] private GameObject panel_;

    private void Update()
    {
        if(!agent_)
        {
            panel_.SetActive(false);
            return;
        }    
        hunger_.value = agent_.Hunger.Value * 100;
        energy_.value = agent_.Energy.Value * 100;
        bladder_.value = agent_.Bladder.Value * 100;
        thirst_.value = agent_.Thirst.Value * 100;
        Action action = agent_.CurrentAction;
        if(action != null)
        {
            string action_info = string.Format("Action: {0} \nTarget: {1}", action.Name, action.Target);
            action_text_.text = action_info;
        }
    }

    public void ChangeEnergy() { agent_.Energy.Value = energy_.value * 0.01f; }
    public void ChangeHunger() { agent_.Hunger.Value = hunger_.value * 0.01f; }
    public void ChangeThirst() { agent_.Thirst.Value = thirst_.value * 0.01f; }
    public void ChangeBladder() { agent_.Bladder.Value = bladder_.value * 0.01f; }
}

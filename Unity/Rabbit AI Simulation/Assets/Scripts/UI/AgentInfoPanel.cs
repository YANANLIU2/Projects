using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// For the UI panel displaying the selected agent's stats and its current action
public class AgentInfoPanel : MonoBehaviour
{
    [Tooltip("The selected agent")]
    [SerializeField] private Agent agent_ = null;

    [Header("References")]
    [Tooltip("The hunger stat slider")]
    [SerializeField] private Slider hunger_ = null;

    [Tooltip("The energy stat slider")]
    [SerializeField] private Slider energy_ = null;

    [Tooltip("The bladder stat slider")]
    [SerializeField] private Slider bladder_ = null;

    [Tooltip("The thirst stat slider")]
    [SerializeField] private Slider thirst_ = null;

    [Tooltip("The current action text")]
    [SerializeField] private Text action_text_ = null;

    [Tooltip("The UI panel")]
    [SerializeField] private GameObject panel_;

    [Tooltip("The selection gameobject")]
    [SerializeField] private GameObject selection_;

    [Header("Tags")]
    [Tooltip("An agent's tag")]
    [SerializeField] private string agent_tag_ = "Agent";

    // Update
    private void Update()
    {
        // Update the selected agent
        Click();

        // Update the selected agent's info panel
        UpdateAgentPanel();

        // Press Escape to quit the application 
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }

    // Change slider's value based on the selected agent's stats
    public void ChangeEnergy() { agent_.Energy.Value = energy_.value * 0.01f; }
    public void ChangeHunger() { agent_.Hunger.Value = hunger_.value * 0.01f; }
    public void ChangeThirst() { agent_.Thirst.Value = thirst_.value * 0.01f; }
    public void ChangeBladder() { agent_.Bladder.Value = bladder_.value * 0.01f; }


    // Select an agent
    private void Select(Agent agent)
    {
        if (agent == null || selection_ == null) return;
        agent_ = agent;
        selection_.transform.parent = agent.transform;
        selection_.transform.localPosition = Vector3.zero;
    }

    // If we click on an agent, we select it
    private void Click()
    {
        // Reference: https://answers.unity.com/questions/615771/how-to-check-if-click-mouse-on-object.html
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                bool is_agent = hit.transform.CompareTag(agent_tag_);
                if(is_agent)
                    agent_ = hit.transform.gameObject.GetComponent<Agent>();
                Select(agent_);
            }
        }
    }

    // Update the agent panel based on the selected agent's stats
    private void UpdateAgentPanel()
    {
        if (!agent_)
        {
            panel_.SetActive(false);
            return;
        }
        hunger_.value = agent_.Hunger.Value * 100;
        energy_.value = agent_.Energy.Value * 100;
        bladder_.value = agent_.Bladder.Value * 100;
        thirst_.value = agent_.Thirst.Value * 100;
        Action action = agent_.CurrentAction;
        if (action != null)
        {
            string action_info = string.Format("Action: {0} \nTarget: {1}", action.Name, action.Target);
            action_text_.text = action_info;
        }
    }
}

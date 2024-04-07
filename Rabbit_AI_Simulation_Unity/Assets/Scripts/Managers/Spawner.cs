using System.Collections.Generic;
using UnityEngine;

// Spawn agents and keep track of them
public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject agent_prefab_ = null;

    [Header("Randomness Info")]
    [SerializeField] private XorshiftRNG rng_ = new XorshiftRNG();

    [Tooltip("An agent's material will be picked from the array randomly")]
    [SerializeField] private List<Material> agent_materials_ = new List<Material>();

    [Tooltip("The lower bound for an agent's size")]
    [SerializeField] private float min_size_ = 0.8f;

    [Tooltip("The higher bound for an agent's size")]
    [SerializeField] private float max_size_ = 2.6f;

    [Tooltip("The upper bound of an agent's speed")]
    [SerializeField] private float max_speed_ = 3.7f;

    [Tooltip("The lower bound of an agent's speed")]
    [SerializeField] private float min_speed_ = 2.2f;

    [Header("Agents Info")]
    [SerializeField] private List<GameObject> agent_array_ = new List<GameObject>();

    private void Awake() 
    {
        foreach (var agent in FindObjectsOfType<Agent>())
            agent_array_.Add(agent.gameObject);

        rng_.Initialize(); 
    }

    // Spawn an agent randomly
    public void SpawnAnAgent()
    {
        // Instantiation
        GameObject temp = Instantiate(agent_prefab_);
        Agent agent = temp.GetComponent<Agent>();

        // Material
        int random = rng_.GetRange(0, agent_materials_.Count - 1);
        agent.SkinnedMeshRenderer.material = agent_materials_[random];

        // Size
        float size = rng_.GetRange(min_size_, max_size_);
        temp.transform.localScale = new Vector3(size, size, size);

        // Pos
        temp.transform.position = transform.position;

        // Speed
        agent.NavMeshAgent.speed = rng_.GetRange(min_speed_, max_speed_);

        // Needs
        for (int i = 0; i < agent.StatsArray.Count; i++)
            agent.StatsArray[i].Value = rng_.GetNorm();

        agent_array_.Add(temp);
    }

    // Destroy the agent at the end of the agent array
    public void DestroyLastAgent()
    {
        if (agent_array_.Count > 0)
        {
            int index = agent_array_.Count - 1;
            GameObject agent = agent_array_[index];
            agent_array_.RemoveAt(index);
            Destroy(agent);
        }
    }
}

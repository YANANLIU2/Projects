using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SkinnedMeshRenderer skinned_Mesh_Renderer_;
    [SerializeField] private NavMeshAgent navigation_;
    [SerializeField] private Animator animator_;

    [Header("Statistics")]
    [SerializeField] private Stat hunger_;
    [SerializeField] private Stat age_;
    [SerializeField] private Stat thirst_;
    [SerializeField] private Stat bladder_;
    [SerializeField] private Stat energy_;
    [SerializeField] private float walk_radius_ = 3;
    [SerializeField] private XorshiftRNG rng_ = new XorshiftRNG();

    [Header("Action")]
    [SerializeField] private Action current_action_;

    private string running_anim_name_ = "Run";
    private Target self_target_ = null;
    private const int top_choices_num = 3;
    
    [System.Serializable]
    public class Stat
    {
        [SerializeField] private float value_ = 0.5f;
        [SerializeField] private float decay_ = 0.02f;
        public float Value { get => value_; set => value_ = value; }
        public void Update()
        {
            value_ -= decay_ * Time.deltaTime;
            value_ = Mathf.Clamp(value_, 0, 1);
        }
    }

    public class ActionPair
    {
        private ActionScriptableObject  action_scriptable_object_;
        private Target target_;
        private float score_;
        public ActionPair(ActionScriptableObject  action_scriptable_object, Target target) { action_scriptable_object_ = action_scriptable_object; target_ = target; }
        public ActionScriptableObject  ActionScriptableObject { get => action_scriptable_object_; }
        public Target Target { get => target_; }
        public float Score { get => score_; set => score_ = value; }
    }

    // Getters & Setters
    public SkinnedMeshRenderer SkinnedMeshRenderer { get => skinned_Mesh_Renderer_; set => skinned_Mesh_Renderer_ = value; }
    public NavMeshAgent NavMeshAgent { get => navigation_; }
    public Stat Hunger { get => hunger_; }
    public Stat Age { get => age_; }
    public Stat Thirst { get => thirst_; }
    public Stat Bladder { get => bladder_; }
    public Stat Energy { get => energy_; }
    public Action CurrentAction { get => current_action_; }
    public List<Stat> StatsArray { get => stats_array_; set => stats_array_ = value; }

    // Variables
    private List<Stat> stats_array_ = null;
    private Context context_ = null;

    // Initialize and safty checks 
    private void Awake() 
    {
        if(navigation_ == null || !navigation_.isOnNavMesh)
        {
#if DEBUG
            string error_msg = "Navigation is not properly set.";
            Debug.LogError(error_msg);
#endif
            return;
        }

        if(skinned_Mesh_Renderer_ == null)
        {
#if DEBUG
            string warning_msg = "Mesh renderer is not properly set.";
            Debug.LogError(warning_msg);
#endif
        }

        if(animator_ == null)
        {
#if DEBUG
            string warning_msg = "Animator is not properly set.";
            Debug.LogError(warning_msg);
#endif
        }

        self_target_ = GetComponent<Target>();
        if(self_target_ == null)
        {
#if DEBUG
            string error_msg = "Self Target is not properly set.";
            Debug.LogError(error_msg);
#endif
        }

        stats_array_ = new List<Stat>() { age_, hunger_, thirst_, bladder_, energy_ };
        context_ = new Context(this);
        rng_.Initialize();
        PlayAnim(running_anim_name_);
    }

    private void Update()
    {
        // stats
        foreach (var stat in stats_array_) stat.Update();

        // action
        if (current_action_ != null)
        {
            if (current_action_.Update()) 
                current_action_ = null;

        }
        else 
            MakeADecision();
    }

    // Make a decision based on current needs using utility theory
    private void MakeADecision()
    {
        // have a list of all possible pairs of a target and an action 
        List<ActionPair> action_pairs_array = new List<ActionPair>();

        // place targets
        foreach (var target in TargetsManager.Instance.TargetsArray)
            foreach (var action in target.ActionsArray)
                action_pairs_array.Add(new ActionPair(action, target));

        // self target
        foreach (var action in self_target_.ActionsArray)
            action_pairs_array.Add(new ActionPair(action, null));
        
        // compute a score for each pair
        foreach (var pair in action_pairs_array)
        {
            List<float> scores = new List<float>();
            foreach (var decisionFactor in pair.ActionScriptableObject.DecisionFactors)
                scores.Add(decisionFactor.Score(pair.Target, context_));
            pair.Score = MyMath.GeometricMean(scores);
        }

        // sort
        action_pairs_array.Sort((a, b) => b.Score.CompareTo(a.Score)); // descending sort

        // pick one
        ActionPair choice = PickBestChoice(action_pairs_array);

        // Assign a random pos on Navmesh 
        if(choice.Target == null)
        {
            // Reference: https://answers.unity.com/questions/475066/how-to-get-a-random-point-on-navmesh.html
            Vector3 random_direction = Random.insideUnitSphere * walk_radius_;
            random_direction += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(random_direction, out hit, walk_radius_, 1);
            Vector3 final_pos = hit.position;
            navigation_.SetDestination(final_pos);
        }

        current_action_ = new Action(choice.ActionScriptableObject, this, choice.Target);
        PlayAnim(running_anim_name_);
    }

    ActionPair PickBestChoice(List<ActionPair> action_pairs_array)
    {
        // Pick a random one from the top 3
        int max = Mathf.Min(action_pairs_array.Count, top_choices_num);
        int index = rng_.GetRange(0, max - 1);
        return action_pairs_array[index];
    }

    // Reference: https://answers.unity.com/questions/324589/how-can-i-tell-when-a-navmesh-has-reached-its-dest.html
    public bool IsArrived()
    {
        if (!navigation_.pathPending)
            if (navigation_.remainingDistance <= navigation_.stoppingDistance)
                if (!navigation_.hasPath || navigation_.velocity.sqrMagnitude == 0f) return true;
        return false;
    }

    public void PlayAnim(string anim) { animator_.Play(anim); }
}

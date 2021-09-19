using UnityEngine;
using System.Collections.Generic;
using SubGraph = System.Collections.Generic.List<uint>; 

// - splice graphs 
public class GraphGenerator : MonoBehaviour
{
    // Singleton
    public static GraphGenerator s_instance;

    [Header("Transformation Rules")]
    [Tooltip("Set the transformations rules here")]
    [SerializeField] private List<TransformationRule> m_rules = null;

    [Header("Limitation Lists")]
    [Tooltip("The rules are only supposed to run once in a generation")]
    [SerializeField] private List<int> m_canOnlyRunOnceRulesList;
    // A tracker of the rules can only be run once
    private List<int> m_canOnlyRunOnceRulesRecordList;

    [Header("Random Generation")]
    [Tooltip("Times to try to find a valid rule before giving up")]
    [SerializeField] private int m_maxRerollTimes = 8;

    [Tooltip("A random number generator")]
    [SerializeField] private XorshiftRNG m_rng = new XorshiftRNG();
  
    private Graph m_graph = null;
    private WeightedRandomGenerator m_weightedRandomGenerator;
    // Getter
    public Graph Graph { get => m_graph; }

    private void Awake()
    {
        // Singleton
        if (s_instance != null && s_instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            s_instance = this;
            DontDestroyOnLoad(this);
        }
        // Initialize the weighted random generator
        m_weightedRandomGenerator = new WeightedRandomGenerator();
        // Add the rules into the weighted random generator
        for (int i = 0; i < m_rules.Count; i++)
        {
            m_weightedRandomGenerator.AddEntry(i, m_rules[i].Weight);
        }
        // Initialize the graph
        m_graph = new Graph();
        // Initialize lists
        m_canOnlyRunOnceRulesRecordList = new List<int>();
        m_rng.Initialize();
    }

    // Reset everything to the start states
    public void Reset()
    {
        // Reset the level graph
        if (m_graph != null)
        {
            m_graph.Reset();
        }
        // Reset all rules
        if (m_canOnlyRunOnceRulesRecordList != null)
        {
            m_canOnlyRunOnceRulesRecordList.Clear();
        }
    }

    #region Helpers
    // Check to see whether a run can be run anymore or not 
    bool IsARuleAvailable(int index)
    {
        if (m_rules[index] == null)
        {
            return false;
        }
        // If we can only run the rule once and have already run it => The rule is not available => return false
        if (m_canOnlyRunOnceRulesRecordList != null && m_canOnlyRunOnceRulesRecordList.Contains(index))
        {
            return false;
        }
        return true;
    }

    // Check to see if a rule index is valid or not
    bool IsARuleIndexValid(int index)
    {
        // if the index is invalid => return false
        return !(index < 0 || index >= m_rules.Count);
    }

    // Get nodes with the type
    public List<SubGraph> GetNodesByType(Node.ENodeType type)
    {
        List<SubGraph> nodes = new List<SubGraph>();
        for (int i = 0; i < m_graph.Count; i++)
        {
            if (m_graph[i] != null && m_graph[i].Type == type)
            {
                nodes.Add(new SubGraph { m_graph[i].Id });
            }
        }
        return nodes;
    }
    #endregion

    #region Run Rules
    // Run the target transformation rule
    public bool RunARule(int ruleIndex)
    {
        // If the index is not valid or the rule is null or the rule cannot be run anymore => return false
        if (!IsARuleIndexValid(ruleIndex) || !IsARuleAvailable(ruleIndex))
        {
            return false;
        }
        // Try find subgraphs which fit the input graph of the transformation rule
        List<SubGraph> validNodes = FindValidInputSubGraphs(ruleIndex);
        // If no one is qualified => return false 
        if (validNodes == null || validNodes.Count == 0)
        {
            return false;
        }
        // Choose one from all valid subgraphs randomly
        SubGraph chosenPattern = validNodes[m_rng.GetRange(0, validNodes.Count - 1)];
        // Apply the transformation of the rule to the picked subgraph
        m_graph.Splice(chosenPattern.ToArray(), m_rules[ruleIndex]);
        // If the rule can only be run once => record it
        if (m_canOnlyRunOnceRulesList != null && m_canOnlyRunOnceRulesList.Contains(ruleIndex))
        {
            m_canOnlyRunOnceRulesRecordList.Add(ruleIndex);
        }
        return true;
    }

    // Pick and run a random rule for n times
    public void RunRandomRules(int times)
    {
        // Return if the times are invalid
        if (times <= 0)
        {
            return;
        }
        // If the start graph has zero nodes => log error and return
        if (m_graph.Count <= 0)
        {
            return;
        }
        // run a random rule for n times
        for (int i = 0; i < times; i++)
        {
            RunARandomRule();
        }
    }

    // Try run a random rule
    private void RunARandomRule(int rerollTimes = 0)
    {
        // If we cannot reroll anymore => return 
        if (rerollTimes > m_maxRerollTimes)
        {
            return;
        }
        // Get a random rule from the weighted random generator
        int ruleIndex = m_weightedRandomGenerator.GetRandom();
        // If the rule is invalid || If we fail to run a rule => reroll 
        if (!IsARuleIndexValid(ruleIndex) || !RunARule(ruleIndex))
        {
            RunARandomRule(++rerollTimes);
        }
    }
    #endregion

    #region Find Valid Input Subgraphs
    // Find valid input graphs of a rule
    private List<SubGraph> FindValidInputSubGraphs(int ruleIndex)
    {
        InputGraph.InputNode[] nodePattern = m_rules[ruleIndex].InputGraph.NodePattern;
        // Get nodes with the same type as the start node 
        List<SubGraph> startNodesList = GetNodesByType(nodePattern[0].Type);
        // If no valid start node => return 
        if (startNodesList.Count == 0)
        {
            return null;
        }
        // If only looking for a one node pattern => return the startNodesList
        if (nodePattern.Length == 1)
        {
            return startNodesList;
        }
        // For more than one node patterns: Loop through all start nodes and run a depth-limited search and fill out or cull sequences
        List<SubGraph> searchResults = new List<SubGraph>();
        foreach (var startNode in startNodesList)
        {
            FindValidNodePatternsWithDepthLimitedSearch(startNode[0], ruleIndex, searchResults);
        }
        return searchResults;
    }

    // Start a depth-limited search for one start node
    private void FindValidNodePatternsWithDepthLimitedSearch(uint rootNodeId, int ruleIndex, List<SubGraph> searchResults)
    {
        // Nodes has been processed
        var discovered = new List<uint>();
        // Progress has been made for this search
        var workingNodeSequence = new uint[m_rules[ruleIndex].InputGraph.NodePattern.Length];
        // Start searching
        FindValidNodePatternsWithDepthLimitedSearchHelper(rootNodeId, discovered, ruleIndex, 0, workingNodeSequence, 0, searchResults);
    }

    // Using recursion for searching 
    private void FindValidNodePatternsWithDepthLimitedSearchHelper(uint nodeId, List<uint> discovered, int ruleIndex, int searchNode, uint[] workingNodeSequence, int workingNodeSequenceIndex, List<SubGraph> searchResults)
    {
        // Add to discorvered 
        discovered.Add(nodeId);
        // Search Target 
        InputGraph.InputNode[] searchPattern = m_rules[ruleIndex].InputGraph.NodePattern;
        Node.ENodeType targetType = searchPattern[searchNode].Type;
        // If the node meets the requirement 
        if (m_graph.GetNodeById(nodeId).Type == targetType)
        {
            // Add to workingNodeSequence
            workingNodeSequence[workingNodeSequenceIndex] = nodeId;
            // If it's the last node in the searching pattern
            if (workingNodeSequenceIndex == searchPattern.Length - 1)
            {
                // Add to the result searchResults
                searchResults.Add(new List<uint>(workingNodeSequence));
            }
            // Heads deeper to neighbors 
            else
            {
                var outgoingNeighbors = m_graph.OutgoingAdjacencyList[m_graph.GetIndex(nodeId)];
                for (int i = 0; i < outgoingNeighbors.Count; i++)
                {
                    uint neighborId = outgoingNeighbors[i];
                    if (!discovered.Contains(neighborId))
                    {
                        FindValidNodePatternsWithDepthLimitedSearchHelper(neighborId, discovered, ruleIndex, searchNode + 1, workingNodeSequence, workingNodeSequenceIndex + 1, searchResults);
                    }
                }
            }
        }
        // Cull the branch
        else
        {
            workingNodeSequence[workingNodeSequenceIndex] = Graph.s_invalidId;
        }
    }
    #endregion
}



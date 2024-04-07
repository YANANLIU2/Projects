using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using System.Threading.Tasks;

// The script is created for handling UI in the "GraphEditor" scene
public class GraphEditorUIManager : MonoBehaviour
{
    [Header("Add")]
    [Tooltip("The add node dropdown menu")]
    [SerializeField] private Dropdown m_addNodeDD = null;

    [Header("Link")]
    [Tooltip("The input field of the From node for linking")]
    [SerializeField] private InputField m_linkFromIPF = null;
    [Tooltip("The input field of the To node for linking")]
    [SerializeField] private InputField m_linkToIPF = null;

    [Header("Unlink")]
    [Tooltip("The input field of the From node for unlinking two non-adjacent nodes")]
    [SerializeField] private InputField m_unlinkFromIPF = null;
    [Tooltip("The input field of the To node for unlinking two adjacent nodes")]
    [SerializeField] private InputField m_unlinkToIPF = null;

    [Header("Remove")]
    [Tooltip("The input field of the index value for the node we want to remove")]
    [SerializeField] private InputField m_removeNodeIPF = null;

    [Header("Run Random Rules")]
    [Tooltip("The input field of the number of random rules we want to run")]
    [SerializeField] private InputField m_randomRuleNumIPF = null;

    [Header("Graph Info")]
    [Tooltip("The text of displaying the graph information")]
    [SerializeField] private Text m_graphInfoTXT;
    [Tooltip("The scrollbar of the graph info text")]
    [SerializeField] private Scrollbar m_graphInfoScrollBar;

#if DEBUG
    [Header("General Messages")]
    [Tooltip("To inform the previous operation succeeded")]
    [SerializeField] private string m_successMsg = "Success!";

    [Tooltip("To inform the previous operation failed")]
    [SerializeField] private string m_failMsg = "Fail!";

    [Header("Error Messages")]
    [Tooltip("To inform that the accessed input field is empty")]
    [SerializeField] private string m_emptyInputFieldMsg = "Please input something. The input field is empty.";

    [Tooltip("To inform that the typed Id is invalid")]
    [SerializeField] private string m_invalidIdDebugMsg = "Please input a valid Id. This Id is invalid: ";

    [Tooltip("To inform that the typed value is < 0 but it should be >= 0")]
    [SerializeField] private string m_valueLargerThanZeroMsg = "Please input a value larger than 0. The value is invalid: ";
    
    [Tooltip("To inform that the given option in the dropdown menu is invalid")]
    [SerializeField] private string m_invalidDropDownOptionMsg = "Please revise the dropdown menu. The option value is invalid: ";
#endif
    [Header("Export")]
    [Tooltip("XML File's Name")]
    [SerializeField] private string m_xmlFileAddress = "Assets/StreamingData/GeneratedGraph.xml";

    [Tooltip("TXT File's Name")]
    [SerializeField] private string m_txtFileName = "StreamingData/GeneratedGraph.txt";
    private string m_txtFileAddress = string.Empty;
    private Graph m_graph = null;
    private GraphGenerator m_graphGenerator = null;

    // Const
    const string kIdStr = "id";
    const string kRootStr = "root";
    const string kNeighborStr = "neighbor";
    const char kColonChar = ':';
    const char kSpaceChar = ' ';

    private void Start()
    {
        // Add node
        if(!m_addNodeDD)
        {
#if DEBUG
            Debug.LogError("Initialization of adding a node failed");
#endif
            return;
        }
        // Link
        if (!m_linkFromIPF || !m_linkToIPF)
        {
#if DEBUG
            Debug.LogError("Initialization of linking nodes failed");
#endif
            return;
        }
        // Unlink
        if (!m_unlinkFromIPF || !m_unlinkToIPF)
        {
#if DEBUG
            Debug.LogError("Initialization of unlinking nodes failed");
#endif
            return;
        }
        // Remove
        if(!m_removeNodeIPF)
        {
#if DEBUG
            Debug.LogError("Initialization of removing a node failed");
#endif
            return;
        }
        // Random rules
        if(!m_randomRuleNumIPF)
        {
#if DEBUG
            Debug.LogError("Initialization of running random rules failed");
#endif
            return;
        }
        // Graph info display
        if(!m_graphInfoTXT || !m_graphInfoScrollBar)
        {
#if DEBUG
            Debug.LogError("Initialization of displaying graph info failed");
#endif
            return;
        }
        // Graph Generator
        m_graphGenerator = FindObjectOfType<GraphGenerator>();
        if(!m_graphGenerator)
        {
#if DEBUG
            Debug.LogError("The reference of the graph generator cannot be null");
#endif
            return;
        }
        else
        {
            m_graph = m_graphGenerator.Graph;
        }
        // Set txt export path
        m_txtFileAddress = Application.dataPath + "/" + m_txtFileName;
        UpdateGraphPrintMsg();
    }

    public void RunARule(int index)
    {
        // Run the function
        bool result = m_graphGenerator.RunARule(index);
#if DEBUG
        if (result)
        {
            Debug.Log(m_successMsg);
        }
        else
        {
            Debug.LogWarning(m_failMsg);
        }
#endif
        UpdateGraphPrintMsg();
    }

    public void AddANode()
    {
        // Check if the node type value is valid
        int value = m_addNodeDD.value;
        if (value < 0 || value >= (int)Node.ENodeType.kNum)
        {
#if DEBUG
            Debug.LogError(m_invalidDropDownOptionMsg + value);
#endif
            return;
        }
        // If the value is valid => add a new node into the graph
        uint id = m_graph.AddNode((Node.ENodeType)m_addNodeDD.value);
        UpdateGraphPrintMsg();
#if DEBUG
        Debug.Log(m_successMsg);
#endif
    }

    private bool GetFromToNodesFromInputFields(InputField from, InputField to, ref uint fromNode, ref uint toNode)
    {
        if (from.text == string.Empty || to.text == string.Empty)
        {
#if DEBUG
            Debug.LogWarning(m_emptyInputFieldMsg);
#endif
            return false;
        }
        fromNode = uint.Parse(from.text);
        toNode = uint.Parse(to.text);
        if (fromNode == toNode)
        {
#if DEBUG
            Debug.LogWarning(m_failMsg);
#endif
        }
        return CheckId(fromNode) && CheckId(toNode);
    }

    private bool CheckId(uint id)
    {
        if (!m_graph.IsIdValid(id))
        {
#if DEBUG
            Debug.LogWarning(m_invalidIdDebugMsg + id);
#endif
            return false;
        }
        return true;
    }
    public void Link()
    {
        uint from = 0, to = 0;
        if (!GetFromToNodesFromInputFields(m_linkFromIPF, m_linkToIPF, ref from, ref to))
        {
            return;
        }
        // Link the two nodes if they are not connected
        if (m_graph.IsLinked(from, to))
        {
#if DEBUG
            Debug.Log(m_failMsg);
#endif
            return;
        }
        m_graph.LinkNodes(from, to);
        UpdateGraphPrintMsg();
#if DEBUG
        Debug.Log(m_successMsg);
#endif
    }

    public void Unlink()
    {
        uint from = 0, to = 0;
        if (!GetFromToNodesFromInputFields(m_unlinkFromIPF, m_unlinkToIPF, ref from, ref to))
        {
            return;
        }
        // Unline the two nodes if they are connected 
        if (!m_graph.IsLinked(from, to))
        {
#if DEBUG
            Debug.LogWarning(m_failMsg);
#endif
            return;
        }
        m_graph.UnlinkNodes(from, to);
        UpdateGraphPrintMsg();
#if DEBUG
        Debug.Log(m_successMsg);
#endif
    }

    public void RemoveANode()
    {
        if (m_removeNodeIPF.text == string.Empty)
        {
#if DEBUG
            Debug.LogWarning(m_emptyInputFieldMsg);
#endif
            return;
        }
        // Get the node's Id from the Remove Node Inputfield
        uint id = uint.Parse(m_removeNodeIPF.text);
        // Check if the Id is valid 
        if (!m_graph.IsIdValid(id))
        {
#if DEBUG
            Debug.LogWarning(m_invalidIdDebugMsg);
#endif
            return;
        }
        m_graph.DestroyNode(id);
        UpdateGraphPrintMsg();
#if DEBUG
        Debug.Log(m_successMsg);
#endif
    }

    public void Reset()
    {
        m_graphGenerator.Reset();
        m_graphInfoTXT.text = string.Empty;
        m_graphInfoScrollBar.size = 1;
#if DEBUG
        Debug.Log(m_successMsg);
#endif
    }

    // Update the debug message printed on screen 
    private void UpdateGraphPrintMsg()
    {
        m_graphInfoTXT.text = m_graph.GetGraphPrintInfo();
    }

    public void RunRandomRules()
    {
        if (m_randomRuleNumIPF.text == string.Empty)
        {
#if DEBUG
            Debug.LogWarning(m_emptyInputFieldMsg);
#endif
            return;
        }
        int times = int.Parse(m_randomRuleNumIPF.text);
        if (times <= 0)
        {
#if DEBUG
            Debug.LogWarning(m_valueLargerThanZeroMsg);
#endif
            return;
        }
        m_graphGenerator.RunRandomRules(times);
        UpdateGraphPrintMsg();
    }

    public void ExportGraphAsXML()
    {
        // Create a doc
        XmlDocument doc = new XmlDocument();
        // Create a root
        XmlElement docRoot = doc.CreateElement(kRootStr);
        doc.AppendChild(docRoot);
        // Loop nodes
        for (int i = 0; i < m_graph.Count; i++)
        {
            uint id = m_graph[i] == null ? Graph.s_invalidId : m_graph[i].Id;
            Node.ENodeType type = m_graph[i] == null ? Node.ENodeType.kNum : m_graph[i].Type;
            // Create a node element
            XmlElement nodeElement = doc.CreateElement(type.ToString());
            docRoot.AppendChild(nodeElement);
            // Set Id and Type attributes
            nodeElement.SetAttribute(kIdStr, id.ToString());
            if (m_graph[i] != null)
            {
                var outgoingAdjacency = m_graph.OutgoingAdjacencyList[m_graph.GetIndex(id)];
                for (int j = 0; j < outgoingAdjacency.Count; j++)
                {
                    XmlElement neighboreElement = doc.CreateElement(kNeighborStr);
                    neighboreElement.SetAttribute(kIdStr, outgoingAdjacency[j].ToString());
                    nodeElement.AppendChild(neighboreElement);
                }
            }
        }
        //Save
        doc.Save(m_xmlFileAddress);
#if DEBUG
        Debug.Log(m_successMsg);
#endif
    }

    public void ExportGraphAsTXT()
    {
        //Pass the filepath and filename to the StreamWriter Constructor
        StreamWriter sw = new StreamWriter(m_txtFileAddress);
        //Each node will be a line: node's Id + ' ' + node's type: neighbors' Ids
        for (int i = 0; i < m_graph.Count; i++)
        {
            uint id = m_graph[i] == null ? Graph.s_invalidId : m_graph[i].Id;
            string nodeInfo = id.ToString() + kSpaceChar + (int)m_graph[i].Type + kColonChar;
            int index = m_graph.GetIndex(m_graph[i].Id);

            var outgoingAdjacency = m_graph.OutgoingAdjacencyList[index];
            for (int j = 0; j < outgoingAdjacency.Count; j++)
            {
                nodeInfo += kSpaceChar + outgoingAdjacency[j].ToString();
            }
            sw.WriteLine(nodeInfo);
        }
        //Close the file
        sw.Close();
    }

    public void ImportGraphFromTXT()
    {
        m_graph.Reset();
        string[] lines = System.IO.File.ReadAllLines(m_txtFileAddress);
        // Create nodes 
        foreach (string line in lines)
        {
            // Parse each line to a node: node's Id + space + node's typem+ colon neighbors' Ids
            int spaceIndex = line.IndexOf(kSpaceChar);
            uint id = uint.Parse(line.Substring(0, spaceIndex));
            if(id == Graph.s_invalidId)
            {
                m_graph.AddNode(null);
            }
            else
            {
                int typeStart = spaceIndex + 1;
                string typeString = line.Substring(typeStart, line.IndexOf(kColonChar) - typeStart);
                Node.ENodeType type = (Node.ENodeType)(int.Parse(typeString));
                m_graph.AddNode(new Node(id, type));
            }
        }
        // Create adjacencies
        for (int i = 0; i < m_graph.Count; i++)
        {
            if(m_graph[i] != null)
            {
                int colonIndex = lines[i].IndexOf(kColonChar);
                int neighborsStart = colonIndex + 2;
                int length = lines[i].Length - neighborsStart;
                if (length <= 0)
                {
                    continue;
                }
                string neighborsTxt = lines[i].Substring(neighborsStart, length);
                string[] neighbors = neighborsTxt.Split(kSpaceChar);
                foreach (var neighbor in neighbors)
                {
                    uint to = uint.Parse(neighbor);
                    m_graph.LinkNodes(m_graph[i].Id, to);
                }
            }
        }
        UpdateGraphPrintMsg();
    }

    public void ImportGraphFromXML()
    {
        m_graph.Reset();
        // Load the file
        XmlDocument doc = new XmlDocument();
        doc.Load(m_xmlFileAddress);
        // Get the root
        XmlElement root = doc.DocumentElement;
        // Create nodes
        for (int i = 0; i < root.ChildNodes.Count; i++)
        {
            var nodeElement = root.ChildNodes[i];
            uint id = uint.Parse(nodeElement.Attributes[kIdStr].Value);
            if(id != Graph.s_invalidId)
            {
                Node.ENodeType type = (Node.ENodeType)System.Enum.Parse(typeof(Node.ENodeType), nodeElement.Name);
                m_graph.AddNode(new Node(id, type));
            }
            else
            {
                m_graph.AddNode(null);
            }
        }
        // Create adjacencies
        for (int i = 0; i < m_graph.Count; i++)
        {
            if(m_graph[i] != null)
            {
                var neighborElements = root.ChildNodes[i];
                for (int j = 0; j < neighborElements.ChildNodes.Count; j++)
                {
                    uint neighborId = uint.Parse(neighborElements.ChildNodes[j].Attributes[kIdStr].Value);
                    m_graph.LinkNodes(m_graph[i].Id, neighborId);
                }
            }
        }
        UpdateGraphPrintMsg();
    }
}
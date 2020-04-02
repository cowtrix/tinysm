using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class MainMenuEffect : MonoBehaviour
{
    public GameObject NodePrefab;
    public int NodeCount;
    public Vector2 NodeLifetime = new Vector2(10, 15);
    public Vector2 Offset = new Vector2(10, 0);
    public int NeighbourCount = 2;
    public float MoveSpeed = 1;
    public AnimationCurve AlphaOverLifetime;

    private RectTransform m_rectTransform;

    class Node
    {
        public RectTransform Transform;
        public List<RectTransform> Neighbours = new List<RectTransform>();
        public CanvasGroup CanvasGroup;
        public UILineRenderer LineRenderer;
        public float Time;
        public float LifeTime;
    }

    List<Node> m_nodes = new List<Node>();

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        while(NodeCount - m_nodes.Count > NeighbourCount)
        {
            var temp = new Node[NeighbourCount];
            for (var n = 0; n < NeighbourCount; ++n)
            {
                var go = Instantiate(NodePrefab);
                go.SetActive(true);
                go.transform.SetParent(transform);
                go.transform.localPosition = m_rectTransform.rect.min +
                    new Vector2(Random.Range(0, m_rectTransform.rect.width), Random.Range(0, m_rectTransform.rect.height));
                go.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                var newNode = new Node
                {
                    Transform = go.GetComponent<RectTransform>(),
                    CanvasGroup = go.AddComponent<CanvasGroup>(),
                    LineRenderer = go.GetComponentInChildren<UILineRenderer>(),
                    LifeTime = Random.Range(NodeLifetime.x, NodeLifetime.y),
                };
                m_nodes.Add(newNode);
                temp[n] = newNode;
            }
            for (var n = 0; n < NeighbourCount - 1; ++n)
            {
                temp[n].Neighbours.Add(temp[n + 1].Transform);
            }
        }
        for (int i = m_nodes.Count - 1; i >= 0; i--)
        {
            Node node = m_nodes[i];
            node.Time += Time.deltaTime;
            var lifetime = node.Time / node.LifeTime;
            if(lifetime > 1)
            {
                Destroy(node.Transform.gameObject);
                m_nodes.RemoveAt(i);
                continue;
            }
            node.CanvasGroup.alpha = AlphaOverLifetime.Evaluate(lifetime);
            var lineList = new List<Vector2>();
            foreach(var neighbour in node.Neighbours)
            {
                if(!neighbour)
                {
                    continue;
                }
                var vec = node.Transform.InverseTransformPoint(neighbour.position);
                lineList.Add(vec.normalized * Offset);
                lineList.Add((Vector2)vec - (vec.normalized * Offset));
            }
            node.LineRenderer.Points = lineList.ToArray();
            node.Transform.localPosition += node.Transform.right * Time.deltaTime * MoveSpeed;
        }
    }
}

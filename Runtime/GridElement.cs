using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour
{
    [SerializeField] Vector2Int m_Index;

    public Vector2Int Index { get => m_Index; set => m_Index = value; }
}

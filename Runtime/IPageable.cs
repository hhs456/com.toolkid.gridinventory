using System.Collections;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;

public interface IPageable
{
    int CurrentPage { get; set; }
    Vector2Int PageSize { get; set; }
    int PageCount { get; set; }
    bool IsLoop { get; set; }

    void NextPage();

    void PrevPage();
}

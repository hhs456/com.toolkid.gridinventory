using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPageable
{
    int CurrentPage { get; set; }
    Vector2Int PageSize { get; set; }
    int PageCount { get; set; }
    bool IsLoop { get; set; }
}

using UnityEngine;
using System.Collections.Generic;

public abstract class GoapAction : MonoBehaviour
{
    public float cost = 1f;

    public Dictionary<string, bool> preconditions = new();
    public Dictionary<string, bool> effects = new();

    public abstract bool Perform(GoapAgent agent);
    public abstract bool IsDone();
}

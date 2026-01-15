using UnityEngine;
using System.Collections.Generic;

public class WorldState
{
    Dictionary<string, bool> state = new Dictionary<string, bool>();

    public bool Get(string key) => state.ContainsKey(key) && state[key];

    public void Set(string key, bool value) => state[key] = value;

    public Dictionary<string, bool> GetStates() => new Dictionary<string, bool>(state);
}
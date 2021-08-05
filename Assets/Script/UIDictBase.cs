
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class NameToID
{
    public string name = "";
    public Object ID ;
}
public class UIDictBase : MonoBehaviour
{
    public List<NameToID> nameToIDList = new List<NameToID>();

    Dictionary<string, Object> nameToID = new Dictionary<string, Object>();
    
    void Awake()
    {
        foreach (NameToID info in nameToIDList)
        {
            nameToID[info.name] = info.ID;
        }
        nameToIDList = null; 
    }
}


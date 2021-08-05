using System.Collections;
using System.Collections.Generic;
using Thinbug.Jason;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    public Text txtGo;
    public SerializationData uimenu;
    public static UIMain inst;
    // Start is called before the first frame update
    void Start()
    {
        inst = this;
    }
}

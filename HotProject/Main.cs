using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Thinbug.Jason;
namespace HotProject
{
    public class Main
    {
        public static void Init()
        {
            Debug.Log("Init dll Main .");

            GameRoot.inst.Print();

            //Text txt = UIMain.inst.gameObject.GetComponent<Text>();
            //UIMain.inst.txtGo.text = "edit by dll !";
            GameObject ob = GameObject.Find("Canvas");
            SerializationData sd = ob.GetComponent<SerializationData>();
            Text t = (Text)sd.dict["txtHello"];
            t.text = "edit by dll ! !!";

            SerializationData sd2 = (SerializationData)sd.dict["sdata"];
            Text t2 = (Text)sd2.dict["txt"];
            t2.text = "序列化的文本!!! by Hot dll";
        }

        public static void Update()
        {
            //Debug.Log("Hot:update");
        }
    }
}

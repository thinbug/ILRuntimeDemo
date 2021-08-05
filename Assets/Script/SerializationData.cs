using UnityEngine;

using System.Collections.Generic;

namespace Thinbug.Jason
{ 
    public class SerializationData : MonoBehaviour, ISerializationCallbackReceiver
    {
        [System.Serializable]
        public class KeyValue
        {
            public string name = "";
            public Object target;
        }

        public List<KeyValue> listData = new List<KeyValue>();

        public Dictionary<string, Object> dict
        { get {                return _dict;            } 
        }
        Dictionary<string, Object> _dict = new Dictionary<string, Object>();

        //序列化前
        public void OnBeforeSerialize()
        {
            listData.Clear();
            foreach (var kvp in _dict)
            {
                KeyValue kv = new KeyValue { name = kvp.Key, target = kvp.Value };
                listData.Add(kv);
            }
        }

        //反序列化
        public void OnAfterDeserialize()
        {
            int no = 0;
            _dict = new Dictionary<string, Object>();
            foreach (KeyValue info in listData)
            {
                if (!_dict.ContainsKey(info.name))
                {
                    _dict.Add(info.name, info.target);
                }
                else
                {
                    no++;
                    _dict.Add(info.name+"("+no+")", null);
                }
            }
        }
    }
}
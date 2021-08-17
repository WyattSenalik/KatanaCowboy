using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEventSystem.CustomEditor
{
    [Serializable]
    public class EventWithTypes: IComparable<EventWithTypes>, IEquatable<EventWithTypes>
    {
        public string Name
        {
            get => eventName;
            set => eventName = value;
        }
        private string eventName = "";
        public List<string> ParamTypeNames => paramTypeNames;
        [SerializeField]
        private List<string> paramTypeNames = new List<string>();


        public EventWithTypes(string name, params string[] paramTypes)
        {
            eventName = name;
            paramTypeNames = new List<string>(paramTypes);
        }

        public bool AddParameter(string paramString)
        {
            if (paramTypeNames.Contains(paramString))
            {
                return false;
            }
            paramTypeNames.Add(paramString);
            return true;
        }

        public int CompareTo(EventWithTypes other)
        {
            // Null check
            if (other == null)
            {
                return 1;
            }
            // Compare names
            int nameCompare = Name.CompareTo(other.Name);
            if (nameCompare != 0)
            {
                // Names are different
                return nameCompare;
            }
            // Compare list sizes
            int paramListCountCompare = ParamTypeNames.Count.CompareTo(other.ParamTypeNames.Count);
            if (paramListCountCompare != 0)
            {
                // List sizes are different
                return paramListCountCompare;
            }
            // Compare list content
            for (int i = 0; i < ParamTypeNames.Count; ++i)
            {
                string curType = ParamTypeNames[i];
                string otherType = other.ParamTypeNames[i];
                int paramNameCompare = curType.CompareTo(otherType);
                if (paramNameCompare != 0)
                {
                    // Names are different
                    return curType.CompareTo(otherType);
                }
            }

            // Name, list size, and list content are all same
            return 0;
        }

        public bool Equals(EventWithTypes other)
        {
            return CompareTo(other) == 0;
        }

        public override string ToString()
        {
            string paramStr = "";
            foreach (string singleParamStr in ParamTypeNames)
            {
                paramStr += '$' + singleParamStr;
            }

            return Name + paramStr;
        }
    }
}
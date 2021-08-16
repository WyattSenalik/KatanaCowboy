using System;
using UnityEngine;

namespace GameEventSystemAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class TypeSelectionAttribute : PropertyAttribute
    {

    }
}
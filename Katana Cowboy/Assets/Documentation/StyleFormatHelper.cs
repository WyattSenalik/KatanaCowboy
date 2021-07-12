using UnityEngine;

namespace OtherExampleNamespace
{
    /// <summary>
    /// Helper for the style format example.
    /// </summary>
    public class StyleFormatHelper : MonoBehaviour
    {
        // Amount of monobehaviours attached to this script
        public int AmountBehaviours => amountBehavs;
        private int amountBehavs = 0;


        // Domestic Initialization
        private void Awake()
        {
            MonoBehaviour[] behavs = GetComponents<MonoBehaviour>();
            amountBehavs = behavs.Length;
        }
    }
}

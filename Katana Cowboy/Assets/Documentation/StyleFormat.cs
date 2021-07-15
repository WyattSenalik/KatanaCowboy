// Base Packages
using UnityEngine;

// Imported Packages
//using ImportedPackageNamespace;

// Custom Namespaces
using OtherExampleNamespace;

// Use namespaces where appropriate
namespace ExampleNamespace
{
    /// <summary>
    /// ALWAYS include a summary of why you made this class, why its necessary, and what it does.
    /// </summary>
    [RequireComponent(typeof(StyleFormatHelper))]
    public class StyleFormat : MonoBehaviour
    {
        // ALWAYS put all member variables and properties above all functions.
        // ALWAYS give member variables default values.
        [SerializeField] private int myInt = 0;

        // References
        private StyleFormatHelper styleHelper = null;

        // Other variables
        private bool myBool = false;
        public string MyString => myString;
        private string myString = "";
        private Vector2 currentVector = Vector2.one;
        // Include 2 blank lines between the group of variables up here and where functions start


        // Use regions, keeping 2 blank lines between regions and no blank lines between functions within the region
        #region UnityMessages
        // Do not give unity messages summaries
        // Do all Domestic Initialization in Awake.
        // This includes GetComponent<> as well as setting local variable values
        private void Awake()
        {
            myBool = true;

            styleHelper = GetComponent<StyleFormatHelper>();
        }
        // Do all Foreign Initialization in Start.
        private void Start()
        {
            myInt = styleHelper.AmountBehaviours;
        }
        #endregion UnityMessages


        #region Public
        /// <summary>
        /// Summary.
        /// </summary>
        public void PublicMethod()
        {
            if (myBool)
            {
                HelperMethod();
            }
        }
        /// <summary>
        /// Summary.
        /// </summary>
        public void AnotherPublicMethod()
        {
            if (myInt == 0)
            {
                AnotherHelperMethod();
            }
        }
        #endregion Public


        #region Private (Helpers)
        /// <summary>
        /// Summary.
        /// </summary>
        private void HelperMethod()
        {
            currentVector = Vector2.left;
        }
        /// <summary>
        /// Summary.
        /// </summary>
        private void AnotherHelperMethod()
        {
            currentVector = Vector2.right;
        }
        #endregion Private (Helpers)


        // Functions not in a region should go at the bottom of the script.
        // Either have 0, 1, or 2 blank lines between these unregioned methods.
        // 0 blank lines means the functions are very closely related.
        // 1 blank line means the functions are semi closely related.
        // 2 blank lines means the functions have almost no relation.

        /// <summary>
        /// Summary.
        /// </summary>
        protected void MethodWithoutRegion()
        {
            if (currentVector == Vector2.left)
            {
                AnotherMethodWithoutRegion();
            }
        }

        /// <summary>
        /// Summary.
        /// </summary>
        protected virtual void AnotherMethodWithoutRegion()
        {

        }
    }
}

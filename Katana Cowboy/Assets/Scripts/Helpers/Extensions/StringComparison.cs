
/// <summary>
/// Collection of helpful string functions.
/// </summary>
public static class StringComparison
{
    /// <summary>
    /// Compares if two strings are equal.
    /// </summary>
    /// <param name="str0">String to compare.</param>
    /// <param name="str1">Other string to compare.</param>
    /// <returns>bool - true if both strings are equal</returns>
    public static bool Equals(string str0, string str1)
    {
        // See if the strings are the same size.
        if (str0.Length != str1.Length)
        {
            return false;
        }

        // Compare each character of the strings.
        for (int i = 0; i < str0.Length; ++i)
        {
            if (str0[i] != str1[i])
            {
                return false;
            }
        }

        return true;
    }
    /// <summary>
    /// Compares if two strings are equal, ignoring case.
    /// </summary>
    /// <param name="str0">String to compare.</param>
    /// <param name="str1">Other string to compare.</param>
    /// <returns>bool - true if both strings are equal</returns>
    public static bool EqualsIgnoreCase(this string str0, string str1)
    {
        return Equals(str0.ToLower(), str1.ToLower());
    }

    /// <summary>
    /// Compares if the left is greater than the right.
    /// Z is greater than A.
    /// A longer string will be considered greater.
    /// </summary>
    /// <param name="leftStr">Left string to compare.</param>
    /// <param name="rightStr">Right string to compare.</param>
    /// <returns>bool - true if left is greater than right</returns>
    public static bool GreaterThan(this string leftStr, string rightStr)
    {
        // Determine the shorter string
        bool leftIsShort = true;
        string shorter = leftStr;
        if (rightStr.Length < leftStr.Length)
        {
            leftIsShort = false;
            shorter = rightStr;
        }

        // Compare each character of the strings.
        for (int i = 0; i < shorter.Length; ++i)
        {
            // If the current character of left is greater than the current character of right, left is greater.
            if (leftStr[i] > rightStr[i])
            {
                return true;
            }
            // If the current character of left is less than the current character of right, right is greater.
            else if (leftStr[i] < rightStr[i])
            {
                return false;
            }
            // Otherwise, they are equal, so keep checking.
        }

        // If we reach the end of the for loop, that means the strings are equal or one of the strings is a substring of the other.
        // First check if the strings are equal, if they are, left is not greater than right.
        if (leftStr.Length == rightStr.Length)
        {
            return false;
        }
        // A longer string is considered greater, so if left is longer, return true. if shorter false.
        if (leftIsShort)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Compares if the left is greater than the right, but ignores casing.
    /// Z is greater than A.
    /// A longer string will be considered greater.
    /// </summary>
    /// <param name="leftStr">Left string to compare.</param>
    /// <param name="rightStr">Right string to compare.</param>
    /// <returns>bool - true if left is greater than right</returns>
    public static bool GreaterThanIgnoreCase(this string leftStr, string rightStr)
    {
        return  leftStr.ToLower().GreaterThan(rightStr.ToLower());
    }

    /// <summary>
    /// Compares if the left is less than the right.
    /// A is less than Z.
    /// A shorter string will be considered lesser.
    /// </summary>
    /// <param name="leftStr">Left string to compare.</param>
    /// <param name="rightStr">Right string to compare.</param>
    /// <returns>bool - true if left is less than right</returns>
    public static bool LessThan(this string leftStr, string rightStr)
    {
        return rightStr.GreaterThan(leftStr);
    }

    /// <summary>
    /// Compares if the left is less than the right, but ignores case.
    /// A is less than Z.
    /// A shorter string will be considered lesser.
    /// </summary>
    /// <param name="leftStr">Left string to compare.</param>
    /// <param name="rightStr">Right string to compare.</param>
    /// <returns>bool - true if left is less than right</returns>
    public static bool LessThanIgnoreCase(this string leftStr, string rightStr)
    {
        return leftStr.ToLower().LessThan(rightStr.ToLower());
    }

    /// <summary>
    /// Compares if the left is greater than or equal to the right.
    /// Z is greater than A.
    /// A longer string will be considered greater.
    /// </summary>
    /// <param name="leftStr">Left string to compare.</param>
    /// <param name="rightStr">Right string to compare.</param>
    /// <returns>bool - true if left is greater than or equal to the right</returns>
    public static bool GreaterThanEqualTo(this string leftStr, string rightStr)
    {
        return !leftStr.LessThan(rightStr);
    }

    /// <summary>
    /// Compares if the left is greater than or equal to the right, but ignores casing.
    /// Z is greater than A.
    /// A longer string will be considered greater.
    /// </summary>
    /// <param name="leftStr">Left string to compare.</param>
    /// <param name="rightStr">Right string to compare.</param>
    /// <returns>bool - true if left is greater than or equal to the right</returns>
    public static bool GreaterThanEqualToIgnoreCase(string leftStr, string rightStr)
    {
        return leftStr.ToLower().GreaterThanEqualTo(rightStr.ToLower());
    }

    /// <summary>
    /// Compares if the left is less than or equal to the right.
    /// A is less than Z.
    /// A shorter string will be considered lesser.
    /// </summary>
    /// <param name="leftStr">Left string to compare.</param>
    /// <param name="rightStr">Right string to compare.</param>
    /// <returns>bool - true if left is less than or equal to the right</returns>
    public static bool LessThanEqualTo(this string leftStr, string rightStr)
    {
        return !leftStr.GreaterThan(rightStr);
    }

    /// <summary>
    /// Compares if the left is less than or equal to the right, but ignores casing.
    /// A is less than Z.
    /// A shorter string will be considered lesser.
    /// </summary>
    /// <param name="leftStr">Left string to compare.</param>
    /// <param name="rightStr">Right string to compare.</param>
    /// <returns>bool - true if left is less than or equal to the right</returns>
    public static bool LessThanEqualToIgnoreCase(this string leftStr, string rightStr)
    {
        return leftStr.ToLower().LessThanEqualTo(rightStr.ToLower());
    }
}

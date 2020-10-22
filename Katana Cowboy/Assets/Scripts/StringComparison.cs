using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringComparison
{
    /// <summary>
    /// Compares if two strings are equal.
    /// </summary>
    /// <param name="_str0_">String to compare.</param>
    /// <param name="_str1_">Other string to compare.</param>
    /// <returns>bool - true if both strings are equal</returns>
    public static bool Equals(string _str0_, string _str1_)
    {
        // See if the strings are the same size.
        if (_str0_.Length != _str1_.Length)
            return false;

        // Compare each character of the strings.
        for (int i = 0; i < _str0_.Length; ++i)
        {
            if (_str0_[i] != _str1_[i])
                return false;
        }

        return true;
    }
    /// <summary>
    /// Compares if two strings are equal, ignoring case.
    /// </summary>
    /// <param name="_str0_">String to compare.</param>
    /// <param name="_str1_">Other string to compare.</param>
    /// <returns>bool - true if both strings are equal</returns>
    public static bool EqualsIgnoreCase(string _str0_, string _str1_)
    {
        return StringComparison.Equals(_str0_.ToLower(), _str1_.ToLower());
    }

    /// <summary>
    /// Compares if the left is greater than the right.
    /// Z is greater than A.
    /// A longer string will be considered greater.
    /// </summary>
    /// <param name="_left_">Left string to compare.</param>
    /// <param name="_right_">Right string to compare.</param>
    /// <returns>bool - true if left is greater than right</returns>
    public static bool GreaterThan(string _left_, string _right_)
    {
        // Determine the shorter string
        bool leftIsShort = true;
        string shorter = _left_;
        if (_right_.Length < _left_.Length)
        {
            leftIsShort = false;
            shorter = _right_;
        }

        // Compare each character of the strings.
        for (int i = 0; i < shorter.Length; ++i)
        {
            // If the current character of left is greater than the current character of right, left is greater.
            if (_left_[i] > _right_[i])
                return true;
            // If the current character of left is less than the current character of right, right is greater.
            else if (_left_[i] < _right_[i])
                return false;
            // Otherwise, they are equal, so keep checking.
        }

        // If we reach the end of the for loop, that means the strings are equal or one of the strings is a substring of the other.
        // First check if the strings are equal, if they are, left is not greater than right.
        if (_left_.Length == _right_.Length)
            return false;
        // A longer string is considered greater, so if left is longer, return true. if shorter false.
        if (leftIsShort)
            return false;
        else
            return true;
    }

    /// <summary>
    /// Compares if the left is greater than the right, but ignores casing.
    /// Z is greater than A.
    /// A longer string will be considered greater.
    /// </summary>
    /// <param name="_left_">Left string to compare.</param>
    /// <param name="_right_">Right string to compare.</param>
    /// <returns>bool - true if left is greater than right</returns>
    public static bool GreaterThanIgnoreCase(string _left_, string _right_)
    {
        return GreaterThan(_left_.ToLower(), _right_.ToLower());
    }

    /// <summary>
    /// Compares if the left is less than the right.
    /// A is less than Z.
    /// A shorter string will be considered lesser.
    /// </summary>
    /// <param name="_left_">Left string to compare.</param>
    /// <param name="_right_">Right string to compare.</param>
    /// <returns>bool - true if left is less than right</returns>
    public static bool LessThan(string _left_, string _right_)
    {
        return GreaterThan(_right_, _left_);
    }

    /// <summary>
    /// Compares if the left is less than the right, but ignores case.
    /// A is less than Z.
    /// A shorter string will be considered lesser.
    /// </summary>
    /// <param name="_left_">Left string to compare.</param>
    /// <param name="_right_">Right string to compare.</param>
    /// <returns>bool - true if left is less than right</returns>
    public static bool LessThanIgnoreCase(string _left_, string _right_)
    {
        return LessThan(_left_.ToLower(), _right_.ToLower());
    }

    /// <summary>
    /// Compares if the left is greater than or equal to the right.
    /// Z is greater than A.
    /// A longer string will be considered greater.
    /// </summary>
    /// <param name="_left_">Left string to compare.</param>
    /// <param name="_right_">Right string to compare.</param>
    /// <returns>bool - true if left is greater than or equal to the right</returns>
    public static bool GreaterThanEqualTo(string _left_, string _right_)
    {
        return !LessThan(_left_, _right_);
    }

    /// <summary>
    /// Compares if the left is greater than or equal to the right, but ignores casing.
    /// Z is greater than A.
    /// A longer string will be considered greater.
    /// </summary>
    /// <param name="_left_">Left string to compare.</param>
    /// <param name="_right_">Right string to compare.</param>
    /// <returns>bool - true if left is greater than or equal to the right</returns>
    public static bool GreaterThanEqualToIgnoreCase(string _left_, string _right_)
    {
        return GreaterThanEqualTo(_left_.ToLower(), _right_.ToLower());
    }

    /// <summary>
    /// Compares if the left is less than or equal to the right.
    /// A is less than Z.
    /// A shorter string will be considered lesser.
    /// </summary>
    /// <param name="_left_">Left string to compare.</param>
    /// <param name="_right_">Right string to compare.</param>
    /// <returns>bool - true if left is less than or equal to the right</returns>
    public static bool LessThanEqualTo(string _left_, string _right_)
    {
        return !GreaterThan(_left_, _right_);
    }

    /// <summary>
    /// Compares if the left is less than or equal to the right, but ignores casing.
    /// A is less than Z.
    /// A shorter string will be considered lesser.
    /// </summary>
    /// <param name="_left_">Left string to compare.</param>
    /// <param name="_right_">Right string to compare.</param>
    /// <returns>bool - true if left is less than or equal to the right</returns>
    public static bool LessThanEqualToIgnoreCase(string _left_, string _right_)
    {
        return LessThanEqualTo(_left_.ToLower(), _right_.ToLower());
    }
}

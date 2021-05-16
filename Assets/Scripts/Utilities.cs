using System;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public static Utilities instance;

    void Start() {
        instance = this;
    }

    /// <summary>
    /// Creates an ordinal number from an integer.
    /// Used for naming units.
    /// </summary>
    public string CountToOrdinal(int count)
    {
        string stringCount = count.ToString();
        char lastLetter = stringCount[stringCount.Length - 1];

        // Handles any x11, x12, x13
        if (stringCount.Length > 1)
        {
            char secondToLastLetter = stringCount[stringCount.Length - 2];
            if (secondToLastLetter == '1')
            {
                stringCount += "th";
                return stringCount;
            }
        }

        // Handles anything else
        switch (lastLetter)
        {
            case '1':
                stringCount += "st";
                break;
            case '2':
                stringCount += "nd";
                break;
            case '3':
                stringCount += "rd";
                break;
            default:
                stringCount += "th";
                break;
        }

        return stringCount;
    }

    /// <summary>
    /// Returns the provided string with only the first letter uppercased.
    /// </summary>
    public string UppercaseFirstLetter (string str) 
    {
        // Remove the first letter from the rest of the word
        // and perform uppercase/lowercase as necessary
        String strHead = str.Substring(0, 1).ToUpper();
        String strBody = str.Remove(0, 1).ToLower();

        return String.Concat(strHead, strBody);
    }
}

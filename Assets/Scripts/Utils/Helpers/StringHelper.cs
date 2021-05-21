using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class StringHelper {

    public static string PascalToLowerDash(this string text) {
        if (string.IsNullOrEmpty(text)) { return null; }

        List<string> words = new List<string>();
        string currentWord = "";
        foreach (char c in text) {
            if (currentWord.Length > 0 && char.IsUpper(c)) {
                words.Add(currentWord);
                currentWord = "";
            }
            currentWord += c;
        }
        if (!string.IsNullOrEmpty(currentWord)) {
            words.Add(currentWord);
        }

        string connected = "";
        for (int i = 0; i < words.Count; i++) {
            if (string.IsNullOrEmpty(words[i])) { continue; }
            connected += words[i];
            if (i < words.Count - 1) {
                connected += '-';
            }
        }

        return connected.ToLower();
    }

    public static string LowerDashToPascal(this string text) {
        if (string.IsNullOrEmpty(text)) { return null; }
        string[] words = text.Split('-');
        string connected = "";
        foreach (string word in words) {
            if (string.IsNullOrEmpty(word)) { continue; }
            connected += word.Substring(0, 1).ToUpper() + word.Substring(1, word.Length - 1).ToLower();
        }
        return connected;
    }

    public static string CapitalizeFirstChar(this string text) {
        if (string.IsNullOrEmpty(text)) { return null; }
        string firstChar = text.Substring(0, 1).ToUpper();
        return firstChar + text.Substring(1, text.Length - 1);
    }

    public static string AddSpaceBetweenUpperChars(this string text) {
        string newText = "";
        foreach (char c in text) {
            if (!string.IsNullOrEmpty(newText) &&
                char.IsUpper(c)) {
                newText += ' ';
            }
            newText += c;
        }
        return newText;
    }

    public static string AbbreviateInt(int value, int preferredNumberCount = 4) {
        if (value > 999999) {
            return Mathf.FloorToInt(value / 1000000).ToString() + "M";
        }
        if (value >= Mathf.Pow(10, preferredNumberCount)) {
            return Mathf.FloorToInt(value / 1000).ToString() + "K";
        }
        return value.ToString();
    }

    public static string GetTimerText(float floatSeconds, int decimals = 2) {
        int hours = Mathf.FloorToInt(floatSeconds / 60.0f / 60.0f);
        int minutes = Mathf.FloorToInt(floatSeconds / 60.0f) - hours * 60;
        int intSeconds = Mathf.FloorToInt(floatSeconds - minutes * 60 - hours * 60 * 60);

        string result = minutes.ToString("D2") + ":" + intSeconds.ToString("D2");

        if (hours > 0) {
            result = hours.ToString("D2") + ":" + result;
        }
		
        if (decimals > 0) {
			decimal seconds = (decimal)floatSeconds;// Use decimal instead of float to prevent floating precision problems
			decimal restValue = seconds - Math.Floor(seconds);

			int milliseconds = (int)Math.Floor(restValue * (decimal)Mathf.Pow(10, decimals));
            result += "." + milliseconds.ToString("D" + decimals.ToString());
		}		
	
        return result;
    }

    public static string GetCountdownText(float floatSeconds, int maxUnits = 2, bool showEmptyUnits = true) {
        floatSeconds = Mathf.Ceil(floatSeconds);

        int days = Mathf.FloorToInt(floatSeconds / 60.0f / 60.0f / 24.0f);
        int hours = Mathf.FloorToInt(floatSeconds / 60.0f / 60.0f) - days * 24;
        int minutes = Mathf.FloorToInt(floatSeconds / 60.0f) - (hours + days * 24) * 60;
        int intSeconds = Mathf.FloorToInt(floatSeconds - minutes * 60 - hours * 60 * 60 - days * 24 * 60 * 60);

        string result = "";
        int unitCount = 0;

        Func<int, bool> shouldShow = (unitValue) => {
            if (maxUnits > 0 && unitCount >= maxUnits) { return false; }
            if (unitValue == 0) { return unitCount > 0 && showEmptyUnits; }
            return true;
        };

        if (shouldShow(days)) { result += string.Format("{0}d", days) + " "; unitCount++; }
        if (shouldShow(hours)) { result += string.Format("{0}h", hours) + " "; unitCount++; }
        if (shouldShow(minutes)) { result += string.Format("{0}m", minutes) + " "; unitCount++; }
        if (shouldShow(intSeconds)) { result += string.Format("{0}s", intSeconds); unitCount++; }

        return result.Trim();
    }

    public static string RemoveNumbers(this string input) {
        return Regex.Replace(input, @"[\d-]", string.Empty);
    }

    public static string TextToNumber(int number) {
        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return alphabet[number].ToString();
    }

}
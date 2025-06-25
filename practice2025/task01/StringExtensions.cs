using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            input = input.ToLower();
            string newStr = "";
            for(int i = 0; i<input.Length; i++)
            {
                if (char.IsPunctuation(input[i]) || char.IsWhiteSpace(input[i])) continue;

                newStr += input[i];
            }
            if (newStr.Length == 0) return false;

            return newStr.ToCharArray().SequenceEqual(newStr.Reverse());
        }
    }
}

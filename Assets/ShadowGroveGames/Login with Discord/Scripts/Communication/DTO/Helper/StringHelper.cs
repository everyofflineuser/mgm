using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Helper
{
    public static class StringHelper
    {
        /// <summary>
        /// Uppercase first character of the given string
        /// </summary>
        public static string UppercaseFirst(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            if (str.Length == 1)
                return str.ToUpper();

            return char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}

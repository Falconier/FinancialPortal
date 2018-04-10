using FinacialPlanner2.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace FinacialPlanner2.Models.Helpers
{
    public class InviteToken
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public InviteToken()
        {
        }
        #endregion

        #region Generators
        public string GenerateHHToken()
        {
            string finalHHToken = "";
            int[] lengs = { 8, 4, 4, 4, 12 };
            for (int i = 0; i < lengs.Length; i++)
            {
                Random rdm = new Random((int)DateTime.Now.Ticks);
                if(i == lengs.Length - 1)
                {
                    finalHHToken += GeneratePartialHHToken(lengs[i], rdm);
                }
                else
                {
                    finalHHToken += (GeneratePartialHHToken(lengs[i], rdm) + "-");
                }
                Thread.Sleep(millisecondsTimeout: 20);
            }
            return finalHHToken;
        }
        private string GeneratePartialHHToken(int length, Random rnd)
        {
            string chars = "0123456789ABCDEF0123456789ABCDEF";

            StringBuilder hhtoken = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                hhtoken.Append(chars[rnd.Next(chars.Length)]);
            }
            return hhtoken.ToString();
        }
        #endregion
    }
}

/// <summary>
/// Tester
/// </summary>
public static class InviteTokenTester
{
    public static void Main(string[] args)
    {
        InviteToken tkn = new InviteToken();
        string token = tkn.GenerateHHToken();
        Console.WriteLine(token);
        Console.WriteLine(token.Length);
    }
}

﻿using GiaiKhatNgocMai.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Infrastructure.Security
{
    public enum RoleType
    {
        Admin,
        Editor,
        Member,
        Customer
    }

    public class SecurityHelper
    {
        public static Random rand = new Random();
        public static bool CheckRoleType(string type)
        {
            var types = new string[] { "Admin", "Editor", "Member", "Customer" };
            return types.Contains(type);
        }

        public static RoleType GetRoleType(string type)
        {
            switch (type)
            {
                case "Admin": return RoleType.Admin;
                case "Editor": return RoleType.Editor;
                case "Member": return RoleType.Member;
                case "Customer": return RoleType.Customer;
                default: throw new InvalidRoleTypeException();
            }
        }


        public static string GeneratePassword(int length)
        {
            string alphanumericCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                            "abcdefghijklmnopqrstuvwxyz" +
                                            "0123456789";
            var pass = "";
            while (length > 0)
            {
                pass += alphanumericCharacters[rand.Next(alphanumericCharacters.Length)];
                length--;
            }
            return pass;
        }

        public static string Encrypt(string input)
        {
            return String.Empty;
        }

        public static string Decrypt(string input)
        {
            return String.Empty;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace GiaiKhatNgocMai.Infrastructure.Utils
{
    public class StringHelper
    {
        //68 characters
        const string FindText = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵ ";
        const string ReplaceText = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyy-";
        private static StringBuilder result = new StringBuilder();

        public static string VNSignedToLowerUnsigned(string input)
        {
            result.Clear();
            if (string.IsNullOrWhiteSpace(input)) return "";

            result.Clear().Append(EliminateOccurrenceWhitespace(input.ToLower()));

            int maxReplacedCount = FindText.Length;

            int matchedIndex = (-1);

            for (int i = 0; i < input.Length; i++)
            {
                matchedIndex = FindText.IndexOf(input[i]);
                if (matchedIndex >= 0)
                {
                    result.Replace(FindText[matchedIndex], ReplaceText[matchedIndex]);
                    maxReplacedCount -= 1;
                    matchedIndex = (-1);
                }
                if (maxReplacedCount <= 0) break;
            }
            return result.ToString();
        }

        public static string EliminateOccurrenceWhitespace(string input)
        {
            Regex regx = new Regex(@"[ ]{2,}", RegexOptions.None);
            input = regx.Replace(input, @" ");
            return input;
        }

        public static string EliminateAllWhitespace(string input)
        {
            Regex regx = new Regex(@"[ ]{1,}", RegexOptions.None);
            input = regx.Replace(input, @"");
            return input;
        }
        public static DateTime StringToDateTime(string datetime, char separator)
        {
            DateTime result;
            try
            {
                result = Convert.ToDateTime(datetime);
                return result;
            }
            catch{
                var ds = datetime.Split(separator);
                if (ds.Length > 3 || ds.Length < 3) throw new InvalidCastException();
                return new DateTime(int.Parse(ds[0]), int.Parse(ds[1]), int.Parse(ds[2]));
            }            
        }

        public static string ConvertToUnSign(string text)
        {

            for (int i = 33; i < 48; i++)
            {

                text = text.Replace(((char)i).ToString(), "");

            }



            for (int i = 58; i < 65; i++)
            {

                text = text.Replace(((char)i).ToString(), "");

            }



            for (int i = 91; i < 97; i++)
            {

                text = text.Replace(((char)i).ToString(), "");

            }

            for (int i = 123; i < 127; i++)
            {

                text = text.Replace(((char)i).ToString(), "");

            }

            text = text.Replace(" ", "-");

            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");

            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);

            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

        }
    }
}
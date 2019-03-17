using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mp3Md
{
    public static class Extensions
    {
        public static string GetFileNameWithoutExtension(this string path)
        {
            string filename = Path.GetFileName(path);
            filename = filename.Substring(0, filename.LastIndexOf('.'));
            return filename;
        }
        public static string RemoveSpecialCharacters(this string value)
        {
            value = value.Replace("  ", " ");
            StringBuilder sb = new StringBuilder();
            foreach (char c in value)
            {
                if (
                    (c >= '0' && c <= '9')
                    || (c.ToUpper() >= 'A' && c.ToUpper() <= 'Z')
                    || (c == '.')
                    || (c == '_')
                    || (c == '-')
                    || (c == '_')
                    || (c == ' ')
                    || (c == '(')
                    || (c == ')')
                    || (c.ToUpper() == 'Ñ')
                    )
                {
                    sb.Append(c);
                }

                else
                {
                    switch (c.ToUpper())
                    {
                        case 'Á':
                            sb.Append('a');
                            break;
                        case 'É':
                            sb.Append('e');
                            break;
                        case 'Í':
                            sb.Append('i');
                            break;
                        case 'Ó':
                            sb.Append('o');
                            break;
                        case 'Ú':
                            sb.Append('u');
                            break;
                    }
                    //Caracter eliminado
                }
            }
            return sb.ToString();
        }
        public static char ToUpper(this char letter)
        {
            return char.Parse(letter.ToString().ToUpper());
        }

        public static string ToUpperFirst(this string text)
        {
            text = text.ToLower();
            char first = text.Where(x => char.IsLetter(x)).FirstOrDefault();
            int index = text.IndexOf(first);

            string texto = text.Remove(index,1).Insert(index,first.ToString().ToUpper());
            return texto;
            
        }

        public static string ToUpperCammel(this string text)
        {
            if (text != null && text.Length > 0)
            {
                text = text.ToLower();
                var textos = text.Split(' ');

                string result = "";
                foreach (var te in textos)
                {
                    if (te.Trim() != "")
                    {
                        result += te.ToUpperFirst();
                        if (te != textos.Last())
                        {
                            result += " ";
                        }
                    }
                }
                return result;
            }
            return text;
        }
    }
}

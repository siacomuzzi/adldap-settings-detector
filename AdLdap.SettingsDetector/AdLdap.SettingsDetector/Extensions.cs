namespace AdLdap.SettingsDetector
{
    using System;
    using System.Text;

    internal static class Extensions
    {
        public static string CleanForJSON(this string originalString)
        {
            if (string.IsNullOrEmpty(originalString))
            {
                return string.Empty;
            }

            char c = '\0';
            var sb = new StringBuilder(originalString.Length + 4);
            string t;

            for (var i = 0; i < originalString.Length; i += 1)
            {
                c = originalString[i];

                switch (c)
                {
                    case '\\':
                    case '"':
                        sb.Append('\\');
                        sb.Append(c);
                        break;
                    case '/':
                        sb.Append('\\');
                        sb.Append(c);
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    default:
                        if (c < ' ')
                        {
                            t = "000" + string.Format("X", c);
                            sb.Append("\\u" + t.Substring(t.Length - 4));
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }

            return sb.ToString();
        }
    }
}

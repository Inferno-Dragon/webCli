using System;
using System.IO;

namespace webCLI
{
    public class Core
    {
        public static bool IsValidUri(string value)
        {
            // Check if uri is valid
            if (!Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute))
            {
                throw new System.InvalidOperationException("Uri is not valid");

            }

            return true;
        }

        public static bool IsValidOutput(String value)
        {

            // Check if file path is correct
            if (!Path.IsPathFullyQualified(value))
            {
                throw new System.InvalidOperationException("File name is not valid");
            }

            return true;

        }


    }

}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileHelper.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Catel.LogAnalyzer.Helpers
{
    using System.Collections.Generic;
    using System.IO;

    public static class FileHelper
    {
        #region Methods
        public static IEnumerable<string> ReadAllLines(string fileName)
        {
            Argument.IsNotNullOrWhitespace(() => fileName);

            if (!Exists(fileName))
            {
                throw new FileNotFoundException(string.Format("Unable to found the file '{0}'", fileName));
            }

            var fileLines = File.ReadAllLines(fileName);

            return fileLines;
        }

        public static bool Exists(string fileName)
        {
            Argument.IsNotNullOrWhitespace(() => fileName);

            return File.Exists(fileName);
        }
        #endregion
    }
}
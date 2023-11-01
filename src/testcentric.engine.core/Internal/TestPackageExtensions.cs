// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.IO;

namespace TestCentric.Engine.Internal
{
    public delegate bool TestPackageSelectorDelegate(TestPackage p);

    /// <summary>
    /// Extension methods for use with TestPackages
    /// </summary>
    public static class TestPackageExtensions
    {
        public static string ToXml(this TestPackage package)
        {
            var stringWriter = new StringWriter();
            //var settings = new XmlWriterSettings { OmitXmlDeclaration = true };
            //var xmlWriter = XmlWriter.Create(stringWriter, settings);

            new TestPackageSerializer().Serialize(stringWriter, package);

            //xmlWriter.Flush();
            //xmlWriter.Close();

            return stringWriter.ToString();
        }
    }
}

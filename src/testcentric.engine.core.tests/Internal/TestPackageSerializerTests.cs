// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.IO;
using System.Xml;
using NUnit.Framework;

namespace TestCentric.Engine.Internal
{
    public class TestPackageSerializerTests
    {
        TestPackage _package;
        TestPackageSerializer _serializer;

        [OneTimeSetUp]
        public void CreateTestPackage()
        {
            _package = new TestPackage("test1.dll");
            _package.AddSetting("StringSetting", "SomeString");
            _package.AddSetting("IntSetting", 999);
            _package.AddSetting("BoolSetting", true);
        }

        [SetUp]
        public void CreateSerializer()
        {
            _serializer = new TestPackageSerializer();
        }

        [Test]
        public void SerializeToXmlWriter()
        {
            var stringWriter = new StringWriter();
            var xmlWriter = XmlWriter.Create(stringWriter);
            _serializer.Serialize(xmlWriter, _package);

            xmlWriter.Flush();
            xmlWriter.Close();

            VerifyXml(stringWriter.ToString());
        }

        [Test]
        public void SerializeToStringWriter()
        {
            var stringWriter = new StringWriter();
            _serializer.Serialize(stringWriter, _package);

            VerifyXml(stringWriter.ToString());
        }

        [Test]
        public void SerializeUsingPackageExtension()
        {
            VerifyXml(_package.ToXml());
        }

        [Test]
        public void RoundTripTest()
        {
            var newPackage = _serializer.Deserialize(_package.ToXml());

            Assert.Multiple(() =>
            {
                //TODO: it would be better if ids matched
                //Assert.That(newPackage.ID, Is.EqualTo(_package.ID));
                Assert.That(newPackage.Name, Is.EqualTo(_package.Name));
                Assert.That(newPackage.FullName, Is.EqualTo(_package.FullName));
                Assert.That(newPackage.Settings.Count, Is.EqualTo(_package.Settings.Count));
                foreach(string key in _package.Settings.Keys)
                {
                    Assert.That(newPackage.Settings.ContainsKey(key), $"Key '{key}' not found");
                    // TODO: These should be equal, not just have the same string representation
                    Assert.That(newPackage.Settings[key].ToString(), Is.EqualTo(_package.Settings[key].ToString()));
                }
            });
        }

        private void VerifyXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            var node = doc.DocumentElement;
            Assert.That(node.Name, Is.EqualTo("TestPackage"));
            var id1 = node.GetAttribute("id");
            Assert.That(!string.IsNullOrEmpty(id1));
            Assert.That(node.HasAttribute("id"));
            Assert.False(node.HasAttribute("fullname"));

            CheckSettings(node.SelectSingleNode("Settings"));

            var subPackage = node.SelectSingleNode("TestPackage");
            var id2 = subPackage.GetAttribute("id");
            Assert.That(!string.IsNullOrEmpty(id2));
            Assert.That(id2, Is.Not.EqualTo(id1));
            Assert.That(subPackage.GetAttribute("fullname"), Is.EqualTo(Path.GetFullPath("test1.dll")));

            CheckSettings(subPackage.SelectSingleNode("Settings"));

            void CheckSettings(XmlNode settings)
            {
                Assert.That(settings.Name, Is.EqualTo("Settings"));
                Assert.That(settings.Attributes.Count, Is.EqualTo(3));
                Assert.That(settings.GetAttribute("StringSetting"), Is.EqualTo("SomeString"));
                Assert.That(int.Parse(settings.GetAttribute("IntSetting")), Is.EqualTo(999));
                Assert.That(bool.Parse(settings.GetAttribute("BoolSetting")), Is.True);
            }
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using TestTask1;

namespace TestTask1UnitTest
{
    [TestClass]
    public class MessageContainerUnitTest
    {
        [TestInitialize]
        public void Initialize()
        {
            var dict = new Dictionary<string, string>
            {
                ["0"] = "�Twas brillig; and the slithy toves",
                ["1"] = "Did gyre and gimble in the wabe:",
                ["2"] = "All mimsy were the borogoves;",
                ["3"] = "And the mome raths outgrabe.",
                ["4"] = "�Beware the Jabberwock; my son!",
                ["5"] = "The jaws that bite; the claws that catch!",
                ["6"] = "Beware the Jubjub bird; and shun",
                ["7"] = "The frumious Bandersnatch!�"
            };
            var json = JsonSerializer.Serialize(dict);
            File.WriteAllText("data_en-US.json", json);
            dict = new Dictionary<string, string>
            {
                ["0"] = "���������. ������� ������",
                ["1"] = "�������� �� ����,",
                ["2"] = "� ��������� ������,",
                ["3"] = "��� ������� � ����.",
                ["4"] = "� ����� ����������, ���!",
                ["5"] = "�� ��� ������� � ���,",
                ["6"] = "� � ��y�� ����� ������� --",
                ["7"] = "���������� ����������."
            };
            json = JsonSerializer.Serialize(dict);
            File.WriteAllText("data_ru-RU.json", json);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            File.Delete("data_en-US.json");
            File.Delete("data_ru-RU.json");
        }

        [TestMethod]
        public void MessageContainerAShouldWorkAsIntended()
        {
            TestMessageContainer(new MessageContainerA());
        }

        [TestMethod]
        public void MessageContainerBShouldWorkAsIntended()
        {
            TestMessageContainer(new MessageContainerB());
        }

        private void TestMessageContainer(MessageContainer container)
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            Assert.AreEqual("�Twas brillig; and the slithy toves", container[MsgCodes.Msg1]);
            Assert.AreEqual("Did gyre and gimble in the wabe:", container[MsgCodes.Msg2]);
            CultureInfo.CurrentUICulture = new CultureInfo("ru-RU");
            Assert.AreEqual("���������. ������� ������", container[MsgCodes.Msg1]);
            Assert.AreEqual("�������� �� ����,", container[MsgCodes.Msg2]);
            CultureInfo.CurrentUICulture = new CultureInfo("en-GB");
            Assert.ThrowsException<Exception>(() => container[MsgCodes.Msg1], "Data file for en-GB is not found.");
        }
    }
}

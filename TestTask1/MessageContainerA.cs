using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace TestTask1
{
    public class MessageContainerA : MessageContainer
    {
        private static readonly int MessagesCount;

        static MessageContainerA()
        {
            MessagesCount = Enum.GetValues(typeof(MsgCodes)).Cast<int>().Max() + 1;
        }

        private readonly Dictionary<CultureInfo, string[]> _dataByCi = new Dictionary<CultureInfo, string[]>();

        protected override string GetMessage(MsgCodes code, CultureInfo ci)
        {
            if (!_dataByCi.ContainsKey(ci))
            {
                var json = File.ReadAllText(GetDataFileName(ci));
                var dataDict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                var data = new string[MessagesCount];
                for (var i = 0; i < MessagesCount; i++)
                {
                    data[i] = dataDict[i.ToString()];
                }
                _dataByCi[ci] = data;
                return data[(int)code];
            }
            return _dataByCi[ci][(int)code];
        }
    }
}

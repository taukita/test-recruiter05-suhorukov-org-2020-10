using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;

namespace TestTask1
{
    public class MessageContainerB : MessageContainer
    {
        protected override string GetMessage(MsgCodes code, CultureInfo ci)
        {
            using (var reader = new StreamReader(GetDataFileName(ci)))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    while (jsonReader.Read())
                    {
                        if (jsonReader.TokenType == JsonToken.PropertyName &&
                            jsonReader.Value is string s &&
                            int.TryParse(s, out int val) &&
                            val == (int)code)
                        {
                            jsonReader.Read();
                            return jsonReader.Value as string;
                        }
                    }
                }
            }
            throw new Exception($"data_{ci.Name}.json is not valid: missed message with code {(int)code}");
        }
    }
}

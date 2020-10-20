using System;
using System.Globalization;
using System.IO;

namespace TestTask1
{
    public abstract class MessageContainer
    {
        public string this[MsgCodes code]
        {
            get
            {
                var ci = CultureInfo.CurrentUICulture;
                try
                {
                    return GetMessage(code, ci);
                }
                catch (FileNotFoundException)
                {
                    throw new Exception($"Data file for {ci.Name} is not found.");
                }
            }
        }
        protected string GetDataFileName(CultureInfo ci) => $"data_{ci.Name}.json";
        protected abstract string GetMessage(MsgCodes code, CultureInfo ci);
    }
}

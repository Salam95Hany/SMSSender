using SMSSender.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Services.Common
{
    public class AppSettings: IAppSettings
    {
        public string[] URLList { get; set; }
        public string SecretKey { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public JWT Jwt { get; set; }
        public MessageParsingSettings MessageParsing { get; set; } = new();
    }
}

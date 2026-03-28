using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Models
{
    public enum ProviderType
    {
        [EnumMember(Value = "VF-Cash")]
        VodafoneCash = 1,

        [EnumMember(Value = "InstaPay")]
        InstaPay = 2,

        [EnumMember(Value = "VF-Cash-En")]
        VodafoneCashEnglish = 3
    }
    public enum MsgStatus
    {
        Success = 200,
        Failure = 400,
    }
}

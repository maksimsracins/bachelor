using System.Runtime.Serialization;

namespace BlazorWebAppAuthentication.Domain.Enum;

public enum TransactionType
    {
        [EnumMember(Value = "SWIFT")]
        SWIFT,
        [EnumMember(Value = "ISO")]
        ISO
    }

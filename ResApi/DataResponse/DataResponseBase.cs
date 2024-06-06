using System.Diagnostics.CodeAnalysis;

namespace ResApi.DataResponse
{
    /// <summary>
    /// DataResponse base class
    /// </summary>
    /// <typeparam name="TStatusEnum"></typeparam>
    [ExcludeFromCodeCoverage]
    public class DataResponseBase<TStatusEnum> where TStatusEnum : struct
    {
        public string ErrorMessage { get; set; }

        public TStatusEnum ResponseCode { get; set; }
    }
}

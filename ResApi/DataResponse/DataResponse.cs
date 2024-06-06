using ResApi.DataResponse;
using System.Diagnostics.CodeAnalysis;

namespace ResApi.DataResponse
{
    /// <summary>
    /// DataResponse class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ExcludeFromCodeCoverage]
    public class DataResponse<T> : DataResponseBase<EDataResponseCode>
    {
        public bool Succeeded { get; set; }
        public T Data { get; set; }
        public int Count { get; set; }
        public decimal Limit { get; set; }
        public string LimitDescription { get; set; }
        /// <summary>
        /// TO BE INTRODUCED
        /// </summary>
        public string Action { get; set; }
    }

    public enum EDataResponseCode
    {
        Success = 0,
        NoDataFound = 1,
        DatabaseConnectionError = 2,
        InvalidInputParameter = 3,
        RecordSuccessfullyAdded = 4,
        RecordSuccesfullyDeleted = 5,
        RecordSuccesfullyEdited = 6,
        StaleObjectState = 7,
        ReferenceDtoHasNoId = 8,
        Locked = 9,
        InvalidToken = 10,
        ProfileNotActive = 11,
        GenericError = 10000
    }
}

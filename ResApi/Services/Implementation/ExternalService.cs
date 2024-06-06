using ResApi.Models;
using ResApi.Services.Interface;
using ResApi.DataResponse;
using ResApi.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace ResApi.Services.Implementation
{
    public class ExternalService : IExternalService
    {
        private readonly ILogger<ExternalService> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public ExternalService(ILogger<ExternalService> logger, IConfiguration configuration, HttpClient httpClient)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        #region Customer methods
        public async Task<DataResponse<Guid>> CreateCustomerProfile(RegisterUserDto customer)
        {
            var dataResponse = new DataResponse<Guid>
            {
                Succeeded = false,
                ResponseCode = EDataResponseCode.GenericError,
                ErrorMessage = "Per shkak te problemeve teknike nuk jemi ne gjendje te krijojme profilin."
            };
            try
            {
                var authKey = GetApiAuthKey();
                var serverUrl = GetApiEndpoint();
                var username = GetApiUser();
                var password = GetApiPassword();
                if (!string.IsNullOrEmpty(authKey) && !string.IsNullOrEmpty(serverUrl) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    //Set Basic Authorization
                    var authenticationString = $"{username}:{password}";
                    var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(authenticationString));
                    _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);

                    var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json"); ;

                    var url = $"{serverUrl}Account/createcustomer";

                    _logger.LogInformation($"CreateCustomerProfile:  {url} content { content.ReadAsStringAsync().Result}");

                    var response = await _httpClient.PostAsync(url, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("CreateCustomerProfile: Remote server return http status code {0}. {1}", response.StatusCode.ToString(), response.Content.ToString());
                        return dataResponse;
                    }
                    else
                    {
                        var responseContent = string.Empty;
                        try
                        {
                            responseContent = await response.Content.ReadAsStringAsync();
                            var deserialized = JsonConvert.DeserializeObject<DataResponse<Guid>>(responseContent);
                            if (deserialized == null)
                            {
                                _logger.LogError($"CreateCustomerProfile: For registering customer recieved empty response: {dataResponse.ErrorMessage}");
                                return dataResponse;
                            }
                            //success
                            return deserialized;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"CreateCustomerProfile: On Deserialize  content {responseContent} error {ex.Message}");
                            return dataResponse;
                        }
                    }
                }
                else
                {
                    _logger.LogError("CreateCustomerProfile: Invalid server configuratio");
                    return dataResponse;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "CreateCustomerProfile: Server could not be reached");
                return dataResponse;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CreateCustomerProfile: Error occurred while sending request to remote server");
                return dataResponse;
            }
        }

        public async Task<DataResponse<ExtCustomerProfile>> GetCustomerProfile(Guid externalCustomerId)
        {
            var dataResponse = new DataResponse<ExtCustomerProfile>
            {
                Data = new ExtCustomerProfile(),
                Succeeded = false,
                ResponseCode = EDataResponseCode.GenericError,
                ErrorMessage = "Per shkak te problemeve teknike nuk mund te hyni ne sistem. Ju lutem provoni me vone!"//"Due to technical issues we are not able to sign you in. Please try later."
            };
            try
            {
                var authKey = GetApiAuthKey();
                var serverUrl = GetApiEndpoint();
                var username = GetApiUser();
                var password = GetApiPassword();
                if (!string.IsNullOrEmpty(authKey) && !string.IsNullOrEmpty(serverUrl) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    //Set Basic Authorization
                    var authenticationString = $"{username}:{password}";
                    var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(authenticationString));
                    _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);

                    var url = $"{serverUrl}Account/GetCustomer?id={externalCustomerId}";

                    _logger.LogInformation($"GetCustomer:  {url} ");

                    var response = await _httpClient.GetAsync(url);
                    var responseContent = string.Empty;

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("GetCustomer - Remote server return http status code {0}. {1}", response.StatusCode.ToString(), response.Content.ToString());
                        return dataResponse;
                    }
                    else
                    {
                        try
                        {
                            responseContent = await response.Content.ReadAsStringAsync();
                            var deserialized = JsonConvert.DeserializeObject<DataResponse<ExtCustomerProfile>>(responseContent);
                            dataResponse.Data = deserialized.Data;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"On Deserialize  content {responseContent} error {ex.Message}");
                            return dataResponse;
                        }
                    }
                }
                else
                {
                    _logger.LogError("GetCustomerProfile: Invalid server url configuration");
                    return dataResponse;
                }
                dataResponse.Succeeded = true;
                return dataResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Server could not be reached");
                return dataResponse;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while sending request to remote server");
                return dataResponse;
            }
        }

        public async Task<DataResponse<bool>> UpdateCustomer(CustomerDto customer)
        {
            var dataResponse = new DataResponse<bool>
            {
                Succeeded = false,
                ResponseCode = EDataResponseCode.GenericError,
                ErrorMessage = "Per shkak te problemeve teknike nuk jemi ne gjendje te krijojme profilin."
            };
            try
            {
                var authKey = GetApiAuthKey();
                var serverUrl = GetApiEndpoint();
                var username = GetApiUser();
                var password = GetApiPassword();
                if (!string.IsNullOrEmpty(authKey) && !string.IsNullOrEmpty(serverUrl) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    //Set Basic Authorization
                    var authenticationString = $"{username}:{password}";
                    var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(authenticationString));
                    _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);

                    var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json"); ;

                    var url = $"{serverUrl}Account/updatecustomer";

                    _logger.LogInformation($"UpdateCustomer:  {url} content { content.ReadAsStringAsync().Result}");

                    var response = await _httpClient.PostAsync(url, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("UpdateCustomer: Remote server return http status code {0}. {1}", response.StatusCode.ToString(), response.Content.ToString());
                        return dataResponse;
                    }
                    else
                    {
                        var responseContent = string.Empty;
                        try
                        {
                            responseContent = await response.Content.ReadAsStringAsync();
                            var deserialized = JsonConvert.DeserializeObject<DataResponse<bool>>(responseContent);
                            if (deserialized == null)
                            {
                                _logger.LogError($"UpdateCustomer: For registering customer recieved empty response: {dataResponse.ErrorMessage}");
                                return dataResponse;
                            }
                            //success
                            return deserialized;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"UpdateCustomer: On Deserialize  content {responseContent} error {ex.Message}");
                            return dataResponse;
                        }
                    }
                }
                else
                {
                    _logger.LogError("UpdateCustomer: Invalid server configuratio");
                    return dataResponse;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "UpdateCustomer: Server could not be reached");
                return dataResponse;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UpdateCustomer: Error occurred while sending request to remote server");
                return dataResponse;
            }
        }

        public async Task<DataResponse<bool>> SendMail(ExtMailData policy)
        {
            var dataResponse = new DataResponse<bool>
            {
                Succeeded = false,
                Data = false,
                ResponseCode = EDataResponseCode.GenericError,
                ErrorMessage = "Per shkak te problemeve teknike nuk jemi ne gjendje ta ekzekutojme kete veprim ne kete moment. Ju lutemi rifreskoni faqen!"
            };
            try
            {
                var authKey = GetApiAuthKey();
                var serverUrl = GetApiEndpoint();
                var username = GetApiUser();
                var password = GetApiPassword();
                if (!string.IsNullOrEmpty(authKey) && !string.IsNullOrEmpty(serverUrl) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    //Set Basic Authorization
                    var authenticationString = $"{username}:{password}";
                    var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(authenticationString));
                    _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);

                    var content = new StringContent(JsonConvert.SerializeObject(policy), Encoding.UTF8, "application/json"); ;

                    var url = $"{serverUrl}mail/send";

                    _logger.LogInformation($"SendMail:  {url} content { content.ReadAsStringAsync().Result}");

                    var response = await _httpClient.PostAsync(url, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("SendMail - Remote server return http status code {0}. {1}", response.StatusCode.ToString(), response.Content.ToString());
                        return dataResponse;
                    }
                    else
                    {
                        var responseContent = string.Empty;
                        try
                        {
                            responseContent = await response.Content.ReadAsStringAsync();
                            var deserialized = JsonConvert.DeserializeObject<DataResponse<bool>>(responseContent);
                            if (deserialized == null)
                            {
                                _logger.LogError($"SendMail: For send mail received empty response: {dataResponse.ErrorMessage}");
                                return dataResponse;
                            }
                            //success
                            return deserialized;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"SendMail: On Deserialize  content {responseContent} error {ex.Message}");
                            return dataResponse;
                        }
                    }
                }
                else
                {
                    _logger.LogError("SendMail: Invalid server configuration");
                    return dataResponse;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "SendMail: Server could not be reached");
                return dataResponse;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "SendMail: Error occurred while sending request to remote server");
                return dataResponse;
            }
        }

        public async Task<DataResponse<bool>> ValidateCustomerProfile(RegisterUserDto customer)
        {
            var dataResponse = new DataResponse<bool>
            {
                Succeeded = false,
                ResponseCode = EDataResponseCode.GenericError,
                ErrorMessage = "Per shkak te problemeve teknike nuk jemi ne gjendje te krijojme profilin."
            };
            try
            {
                var authKey = GetApiAuthKey();
                var serverUrl = GetApiEndpoint();
                var username = GetApiUser();
                var password = GetApiPassword();
                if (!string.IsNullOrEmpty(authKey) && !string.IsNullOrEmpty(serverUrl) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    //Set Basic Authorization
                    var authenticationString = $"{username}:{password}";
                    var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(authenticationString));
                    _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);

                    var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json"); ;

                    var url = $"{serverUrl}Account/ValidateCustomerProfile";

                    _logger.LogInformation($"ValidateCustomerProfile:  {url} content { content.ReadAsStringAsync().Result}");

                    var response = await _httpClient.PostAsync(url, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("ValidateCustomerProfile: Remote server return http status code {0}. {1}", response.StatusCode.ToString(), response.Content.ToString());
                        return dataResponse;
                    }
                    else
                    {
                        var responseContent = string.Empty;
                        try
                        {
                            responseContent = await response.Content.ReadAsStringAsync();
                            var deserialized = JsonConvert.DeserializeObject<DataResponse<bool>>(responseContent);
                            if (deserialized == null)
                            {
                                _logger.LogError($"ValidateCustomerProfile: For validate customer recieved empty response: {dataResponse.ErrorMessage}");
                                return dataResponse;
                            }
                            //success
                            return deserialized;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"ValidateCustomerProfile: On Deserialize  content {responseContent} error {ex.Message}");
                            return dataResponse;
                        }
                    }
                }
                else
                {
                    _logger.LogError("ValidateCustomerProfile: Invalid server configuratio");
                    return dataResponse;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "ValidateCustomerProfile: Server could not be reached");
                return dataResponse;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ValidateCustomerProfile: Error occurred while sending request to remote server");
                return dataResponse;
            }
        }


        #endregion




        //public async Task<DataResponse<bool>> CheckFValid(Guid customerId, string typePolicy, string subtype, string fvalid)
        //{
        //    var dataResponse = new DataResponse<bool>
        //    {
        //        Succeeded = false,
        //        ResponseCode = EDataResponseCode.GenericError,
        //        ErrorMessage = "Per shkak te problemeve teknike nuk jemi ne gjendje ta ekzekutojme kete veprim ne kete moment. Ju lutemi rifreskoni faqen!"
        //    };

        //    try
        //    {
        //        var authKey = GetApiAuthKey();
        //        var serverUrl = GetApiEndpoint();
        //        var username = GetApiUser();
        //        var password = GetApiPassword();
        //        if (!string.IsNullOrEmpty(authKey) && !string.IsNullOrEmpty(serverUrl) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        //        {
        //            //Set Basic Authorization
        //            var authenticationString = $"{username}:{password}";
        //            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(authenticationString));
        //            _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);

        //            var url = $"{serverUrl}policy/CheckFValid?customerId={customerId}&typePolicy={typePolicy}&subtype={subtype}&fvalid={fvalid}";

        //            _logger.LogInformation($"CheckFValid:  {url} ");

        //            var response = await _httpClient.GetAsync(url);

        //            if (!response.IsSuccessStatusCode)
        //            {
        //                _logger.LogError("CheckFValid - Remote server return http status code {0}. {1}", response.StatusCode.ToString(), response.Content.ToString());
        //                return dataResponse;
        //            }
        //            else
        //            {
        //                var responseContent = string.Empty;
        //                try
        //                {
        //                    responseContent = await response.Content.ReadAsStringAsync();
        //                    var deserialized = JsonConvert.DeserializeObject<DataResponse<bool>>(responseContent);
        //                    if (deserialized == null)
        //                    {
        //                        _logger.LogError($"CheckFValid: For fetching tpl quotation received empty response: {dataResponse.ErrorMessage}");
        //                        return dataResponse;
        //                    }
        //                    //success
        //                    return deserialized;
        //                }
        //                catch (Exception ex)
        //                {
        //                    _logger.LogError(ex, $"CheckFValid: On Deserialize  content {responseContent} error {ex.Message}");
        //                    return dataResponse;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            _logger.LogError("CheckFValid: Invalid server configuration");
        //            return dataResponse;
        //        }
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        _logger.LogError(ex, "CheckFValid: Server could not be reached");
        //        return dataResponse;
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e, "CheckFValid: Error occurred while sending request to remote server");
        //        return dataResponse;
        //    }
        //}


        #region Nomen 
        //public async Task<DataResponse<List<NomenDto>>> GetTHZone()
        //{
        //    var dataResponse = new DataResponse<List<NomenDto>> { Data = new List<NomenDto>(), Succeeded = false };
        //    try
        //    {
        //        var authKey = GetApiAuthKey();
        //        var serverUrl = GetApiEndpoint();
        //        var username = GetApiUser();
        //        var password = GetApiPassword();
        //        if (!string.IsNullOrEmpty(authKey) && !string.IsNullOrEmpty(serverUrl) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        //        {
        //            //Set Basic Authorization
        //            var authenticationString = $"{username}:{password}";
        //            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(authenticationString));
        //            _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);

        //            var url = $"{serverUrl}nomen/GetTHZone";

        //            _logger.LogInformation($"GetTHZone:  {url} ");

        //            var response = await _httpClient.GetAsync(url);

        //            if (!response.IsSuccessStatusCode)
        //            {
        //                _logger.LogError("GetTHZone - Remote server return http status code {0}. {1}", response.StatusCode.ToString(), response.Content.ToString());
        //                return dataResponse;
        //            }
        //            else
        //            {
        //                var responseContent = string.Empty;
        //                try
        //                {
        //                    responseContent = await response.Content.ReadAsStringAsync();
        //                    var deserialized = JsonConvert.DeserializeObject<DataResponse<List<NomenDto>>>(responseContent);
        //                    if (deserialized == null)
        //                    {
        //                        _logger.LogError($"GetTHZone: For get th zone received empty response: {dataResponse.ErrorMessage}");
        //                        return dataResponse;
        //                    }
        //                    //success
        //                    return deserialized;
        //                }
        //                catch (Exception ex)
        //                {
        //                    _logger.LogError(ex, $"GetTHZone: On Deserialize  content {responseContent} error {ex.Message}");
        //                    return dataResponse;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            _logger.LogError("GetTHZone: Invalid server configuration");
        //            return dataResponse;
        //        }
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        _logger.LogError(ex, "GetTHZone: Server could not be reached");
        //        return dataResponse;
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e, "GetTHZone: Error occurred while sending request to remote server");
        //        return dataResponse;
        //    }
        //}
        #endregion

        #region HealthClaim
        public async Task<DataResponse<bool>> CheckPolicy(string policyCode)
        {
            var dataResponse = new DataResponse<bool>
            {
                Succeeded = false,
                ResponseCode = EDataResponseCode.GenericError,
                ErrorMessage = "Per shkak te problemeve teknike nuk jemi ne gjendje te krijojme profilin."
            };
            try
            {
                var authKey = GetApiAuthKey();
                var serverUrl = GetApiEndpoint();
                var username = GetApiUser();
                var password = GetApiPassword();
                if (!string.IsNullOrEmpty(authKey) && !string.IsNullOrEmpty(serverUrl) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    //Set Basic Authorization
                    var authenticationString = $"{username}:{password}";
                    var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(authenticationString));
                    _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);

                    var url = $"{serverUrl}healthclaim/CheckPolicy?policy={policyCode}";

                    _logger.LogInformation($"DownloadPolicy:  {url} ");

                    var response = await _httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("CheckPolicy - Remote server return http status code {0}. {1}", response.StatusCode.ToString(), response.Content.ToString());
                        return dataResponse;
                    }
                    else
                    {
                        var responseContent = string.Empty;
                        try
                        {
                            responseContent = await response.Content.ReadAsStringAsync();
                            var deserialized = JsonConvert.DeserializeObject<DataResponse<bool>>(responseContent);
                            if (deserialized == null)
                            {
                                _logger.LogError($"CheckPolicy: For check policy received empty response: {dataResponse.ErrorMessage}");
                                return dataResponse;
                            }
                            //success
                            return deserialized;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"CheckPolicy: On Deserialize  content {responseContent} error {ex.Message}");
                            return dataResponse;
                        }
                    }
                }
                else
                {
                    _logger.LogError("CheckPolicy: Invalid server configuration");
                    return dataResponse;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "CheckPolicy: Server could not be reached");
                return dataResponse;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CheckPolicy: Error occurred while sending request to remote server");
                return dataResponse;
            }
        }
        #endregion

        #region private methods

        private string ConvertAsciiToUTF8(string inAsciiString)
        {
            // Create encoding ASCII.

            Encoding inAsciiEncoding = Encoding.ASCII;
            // Create encoding UTF8.

            Encoding outUTF8Encoding = Encoding.UTF8;

            // Convert the input string into a byte[].
            byte[] inAsciiBytes = inAsciiEncoding.GetBytes(inAsciiString);


            byte[] outUTF8Bytes = Encoding.Convert(inAsciiEncoding, outUTF8Encoding, inAsciiBytes);

            // Convert the byte array into a char[] and then into a string.

            char[] inUTF8Chars = new

            char[outUTF8Encoding.GetCharCount(outUTF8Bytes, 0, outUTF8Bytes.Length)];
            outUTF8Encoding.GetChars(outUTF8Bytes, 0, outUTF8Bytes.Length, inUTF8Chars, 0);

            string outUTF8String = new

            string(inUTF8Chars);
            return outUTF8String;
        }

        public string GetApiAuthKey()
        {
            try
            {
                return _configuration?.GetSection("Authority:Auth_key")?.Get<string>();
            }
            catch
            {
                return string.Empty;
            }
        }
        public string GetUtilityID()
        {
            try
            {
                return _configuration?.GetSection("Authority:UtilityID")?.Get<string>();
            }
            catch
            {
                return string.Empty;
            }
        }
        public string GetApiEndpoint()
        {
            try
            {
                return _configuration?.GetSection("Authority:ExternalAPI")?.Get<string>();
            }
            catch
            {
                return string.Empty;
            }
        }
        public string GetApiUser()
        {
            try
            {
                return _configuration?.GetSection("Authority:APIUser")?.Get<string>();
            }
            catch
            {
                return string.Empty;
            }
        }
        public string GetApiPassword()
        {
            try
            {
                return _configuration?.GetSection("Authority:APIPassword")?.Get<string>();
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

    }
}

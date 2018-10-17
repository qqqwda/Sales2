using Newtonsoft.Json;
using Plugin.Connectivity;
using Sales.Common;
using Sales.Common.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Services
{
    //clase consuno api
    public class ApiService
    {
        
        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller) //metodo generico que va a servir para consumir listas de cualquier servicio API
        {

            try
            {
                var client = new HttpClient(); //comunicacion con el client
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}";//es equivalente a hacer string.format
                var response = await client.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();//recibe el JSON como string

                if (!response.IsSuccessStatusCode) //Si la respuesta no es success
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,


                    };

                }
                //
                var list = JsonConvert.DeserializeObject<List<T>>(answer);//Deserializa con "Newtonsoft" la respuesta "answer" y la pasa a un var llamado "list"
                return new Response
                {
                    IsSuccess = true,
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,

                };
            }

        }
        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller, string tokenType, string accessToken) //metodo generico que va a servir para consumir listas de cualquier servicio API
        {

            try
            {
                var client = new HttpClient(); //comunicacion con el client
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}";//es equivalente a hacer string.format
                var response = await client.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();//recibe el JSON como string

                if (!response.IsSuccessStatusCode) //Si la respuesta no es success
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,


                    };

                }
                //
                var list = JsonConvert.DeserializeObject<List<T>>(answer);//Deserializa con "Newtonsoft" la respuesta "answer" y la pasa a un var llamado "list"
                return new Response
                {
                    IsSuccess = true,
                    Result = list,
                };
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,

                };
            }

        }

        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller, int id, string tokenType, string accessToken)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}{id}";
                var response = await client.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }



        public async Task<Response> Post<T>(string urlBase, string prefix, string controller, T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model); // serializa el objeto model en un string y lo pasa a request
                var content = new StringContent(request, Encoding.UTF8,"application/json");
                var client = new HttpClient(); //comunicacion con el client
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}";//es equivalente a hacer string.format
                var response = await client.PostAsync(url,content);//pide url y el contenido del objeto serializado
                var answer = await response.Content.ReadAsStringAsync();//recibe el JSON como string

                if (!response.IsSuccessStatusCode) //Si la respuesta no es success
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };

                }
                //
                var obj = JsonConvert.DeserializeObject<T>(answer);//Deserializa con "Newtonsoft" la respuesta "answer" y la pasa a un var llamado "obj"
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,

                };
            }

        }
        public async Task<Response> Post<T>(string urlBase, string prefix, string controller, T model, string tokenType, string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model); // serializa el objeto model en un string y lo pasa a request
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient(); //comunicacion con el client
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}";//es equivalente a hacer string.format
                var response = await client.PostAsync(url, content);//pide url y el contenido del objeto serializado
                var answer = await response.Content.ReadAsStringAsync();//recibe el JSON como string

                if (!response.IsSuccessStatusCode) //Si la respuesta no es success
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };

                }
                //
                var obj = JsonConvert.DeserializeObject<T>(answer);//Deserializa con "Newtonsoft" la respuesta "answer" y la pasa a un var llamado "obj"
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,

                };
            }

        }
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected) //Validar conexion
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Please turn on your internet settings.",
                };
            }
            //ping google.com
            var isReachable = await CrossConnectivity.Current.IsRemoteReachable(
                "google.com");
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Check you internet connection.",
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = "Ok",
            };
        }
        public async Task<Response> Delete(string urlBase, string prefix, string controller, int id)
        {
            try
            {
                var client = new HttpClient(); //comunicacion con el client
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}/{id}";//es equivalente a hacer string.format
                var response = await client.DeleteAsync(url);//pide url y el contenido del objeto serializado
                var answer = await response.Content.ReadAsStringAsync();//recibe el JSON como string

                if (!response.IsSuccessStatusCode) //Si la respuesta no es success
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };

                }
                //
                return new Response
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,

                };
            }

        }
        public async Task<Response> Delete(string urlBase, string prefix, string controller, int id, string tokenType, string accessToken)
        {
            try
            {
                var client = new HttpClient(); //comunicacion con el client
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}/{id}";//es equivalente a hacer string.format
                var response = await client.DeleteAsync(url);//pide url y el contenido del objeto serializado
                var answer = await response.Content.ReadAsStringAsync();//recibe el JSON como string

                if (!response.IsSuccessStatusCode) //Si la respuesta no es success
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };

                }
                //
                return new Response
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,

                };
            }

        }
        public async Task<Response> Put<T>(string urlBase, string prefix, string controller, T model, int id)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model); // serializa el objeto model en un string y lo pasa a request
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient(); //comunicacion con el client
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}/{id}";//es equivalente a hacer string.format
                var response = await client.PutAsync(url, content);//pide url y el contenido del objeto serializado
                var answer = await response.Content.ReadAsStringAsync();//recibe el JSON como string

                if (!response.IsSuccessStatusCode) //Si la respuesta no es success
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };

                }
                //
                var obj = JsonConvert.DeserializeObject<T>(answer);//Deserializa con "Newtonsoft" la respuesta "answer" y la pasa a un var llamado "obj"
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,

                };
            }

        }
        public async Task<Response> Put<T>(string urlBase, string prefix, string controller, T model, int id, string tokenType, string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model); // serializa el objeto model en un string y lo pasa a request
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient(); //comunicacion con el client
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}/{id}";//es equivalente a hacer string.format
                var response = await client.PutAsync(url, content);//pide url y el contenido del objeto serializado
                var answer = await response.Content.ReadAsStringAsync();//recibe el JSON como string

                if (!response.IsSuccessStatusCode) //Si la respuesta no es success
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };

                }
                //
                var obj = JsonConvert.DeserializeObject<T>(answer);//Deserializa con "Newtonsoft" la respuesta "answer" y la pasa a un var llamado "obj"
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,

                };
            }

        }
        public async Task<TokenResponse> GetToken(
                string urlBase,
                string username,
                string password)
                    {
                        try
                        {
                            var client = new HttpClient();
                            client.BaseAddress = new Uri(urlBase);
                            var response = await client.PostAsync("Token",
                                new StringContent(string.Format(
                                "grant_type=password&username={0}&password={1}",
                                username, password),
                                Encoding.UTF8, "application/x-www-form-urlencoded"));
                            var resultJSON = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<TokenResponse>(
                                resultJSON);
                            return result;
                        }
                        catch
                        {
                            return null;
                        }
                    }
        public async Task<Response> GetUser(string urlBase, string prefix, string controller, string email, string tokenType, string accessToken)
        {
            try
            {
                var getUserRequest = new GetUserRequest
                {
                    Email = email,
                };

                var request = JsonConvert.SerializeObject(getUserRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var user = JsonConvert.DeserializeObject<MyUserASP>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = user,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }



    }

}

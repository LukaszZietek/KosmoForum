using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace KosmoForumClient
{


    public static class ModelStateDeserializer
    {
        public static string DeserializeModelState(string responseError) // Próbuje zdeserializować otrzymaną wiadomosć, możliwe 2 typy (Model State lub zamodelowana klasa z polem message z web api)
        {
            try // Model state
            {
                var errorObj = JsonConvert.DeserializeObject<Dictionary<string, Object>>(responseError);
                string message = "";
                foreach (var item in errorObj)
                {
                    var value = errorObj[item.Key].ToString();
                    var index = value.IndexOf("errorMessage") + "errorMessage".Length + 4;
                    var index2 = value.IndexOf('\"', index);

                    message = value.Substring(index,
                        index2 - index);
                    return message;
                }

                return message;
            }
            catch(Exception e)
            {
                try // Model klasy z web api z polem message
                {
                    var errorMessage = JsonConvert.DeserializeAnonymousType(responseError, new {message = ""});
                    return errorMessage.message;
                }
                catch (Exception exception)
                {
                    return "";
                }

            }
        }
    }
}

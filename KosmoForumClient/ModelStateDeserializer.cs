using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KosmoForumClient
{
    public static class ModelStateDeserializer
    {
        public static string DeserializeModelState(ModelStateDictionary modelState)
        {
            string errorMessage = "";
            foreach (var item in modelState.Keys)
            {
                var value = modelState[item];
                foreach (var error in value.Errors)
                {
                    errorMessage += " " + error.ErrorMessage;
                }
            }

            return errorMessage;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KosmoForum
{
    public static class ModelStateToString
    {
        public static string ConvertModelStateToString(ModelStateDictionary modelState)
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

            if (errorMessage == "")
            {
                errorMessage = "Error";
            }

            return errorMessage;
        }
    }
}

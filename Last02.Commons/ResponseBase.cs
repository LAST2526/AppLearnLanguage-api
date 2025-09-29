using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Commons
{
    public class ResponseBase<T>
    {
        public bool IsSuccess { get; init; }
        public string Message { get; init; }
        public T? Data { get; init; }
        public int StatusCode { get; init; }

        private ResponseBase(bool isSuccess, string message, T? data = default, int statusCode = 200)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }

        public static ResponseBase<T> Success(T? data = default, string message = "Success")
        {
            return new ResponseBase<T>(true, message, data, 200);
        }

        public static ResponseBase<T> Error(string message, int statusCode = 400)
        {
            return new ResponseBase<T>(false, message, default, statusCode);
        }

        // For POST Create
        public static ResponseBase<T> Created(T data, string message = "Created")
        {
            return new ResponseBase<T>(true, message, data, 201);
        }

        // For PUT/PATCH
        public static ResponseBase<T> NoContent(string message = "No Content")
        {
            return new ResponseBase<T>(true, message, default, 204);
        }

        public static ResponseBase<T> NotFound(string message = "Not Found")
        {
            return new ResponseBase<T>(false, message, default, 404);
        }


        /// <summary>
        /// Creates an unauthorized response (401)
        /// </summary>
        public static ResponseBase<T> Unauthorized(string message = "Unauthorized")
        {
            return new ResponseBase<T>(false, message, default, 401);
        }
    }

    public static class ResponseExtensions
    {
        public static IActionResult ToActionResult<T>(this ResponseBase<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}

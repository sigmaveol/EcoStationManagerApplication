using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.Results
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static Result<T> Ok(T data, string message = null)
        {
            return new Result<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        public static Result<T> Fail(string errorMessage)
        {
            return new Result<T>
            {
                Success = false,
                Message = errorMessage,
                Errors = new List<string> { errorMessage }
            };
        }

        public static Result<T> Fail(List<string> errors, string message = null)
        {
            return new Result<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }

    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static Result Ok(string message = null)
        {
            return new Result
            {
                Success = true,
                Message = message
            };
        }

        public static Result Fail(string errorMessage)
        {
            return new Result
            {
                Success = false,
                Message = errorMessage,
                Errors = new List<string> { errorMessage }
            };
        }

        public static Result Fail(List<string> errors, string message = null)
        {
            return new Result
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }



}

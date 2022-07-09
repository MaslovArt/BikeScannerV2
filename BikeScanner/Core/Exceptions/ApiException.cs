using System;
using Microsoft.AspNetCore.Http;

namespace BikeScanner.Core.Exceptions
{
	public class ApiException : Exception
	{
		public int ErrorCode { get; set; }

        public ApiException(string message)
			: base(message)
        {
			ErrorCode = StatusCodes.Status400BadRequest;
        }

		public ApiException(string message, int code)
			: base(message)
		{
			ErrorCode = code;
		}

        public static ApiException Error(string message) =>
			new ApiException(message, StatusCodes.Status400BadRequest);

        public static ApiException ServerError(string message) =>
			new ApiException(message, StatusCodes.Status500InternalServerError);

		public static ApiException NotFound(string message = "Запись не найдена.") =>
			new ApiException(message, StatusCodes.Status404NotFound);

		public static ApiException Confict(string message) =>
			new ApiException(message, StatusCodes.Status409Conflict);
	}
}


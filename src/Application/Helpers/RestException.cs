﻿using System.Net;

namespace Application.Helpers
{
    public class RestException : Exception
    {
        public string ErrorMessage { get; set; }
        public HttpStatusCode Code { get; }
        public object Errors { get; }
        /// <summary>
        /// Helps set the rest exception error message as the message value for the exception base class.
        /// </summary>
		public override string Message { get { return !string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage : base.Message; } }


        public RestException(HttpStatusCode code, string message, string field)
        {
            ErrorMessage = message;
            Code = code;
            Errors = new HashSet<object>
            {
                new {Field = field, Message = message}
            }.ToList();
        }
    }
}

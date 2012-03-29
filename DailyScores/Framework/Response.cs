using System;
using System.Collections.Generic;

namespace DailyScores.Framework
{
    public class Response<T>
    {
        #region Constructors

        public Response() : this(false, default(T)) { }

        public Response(bool success) : this(success, default(T)) { }

        public Response(bool success, T value)
        {
            this.IsSuccess = success;
            this.Value = value;

            this.ErrorMessages = new List<string>();
            this.Exceptions = new List<Exception>();
        }

        #endregion Constructors
        
        public T Value { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; private set; }
        public List<Exception> Exceptions { get; private set; }
 
    }
}
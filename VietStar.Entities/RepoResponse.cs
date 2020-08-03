using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities
{
    public class RepoResponse<T>
    {
        public static RepoResponse<T> Create(T dat, string err  =null)
        {
            return new RepoResponse<T>
            {
                data = dat,
                error = err,
                success = string.IsNullOrWhiteSpace(err)
            };
        }
        public string error { get; set; }
        public T data { get; set; }
        public bool success { get; set; }
    }
}

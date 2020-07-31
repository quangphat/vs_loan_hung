using Antlr4.StringTemplate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Entity;

namespace VS_LOAN.Core.Utility
{
    public  class ExportUtil
    {
        public static async Task<ActionResult> Export<TRequest, TResult>(HttpResponseBase Response,Func<TRequest, Task<BaseFindResponse<TResult>>> func, TRequest request, string fileName, string[] columns)
            where TRequest : BaseFindRequest
            where TResult : class
        {
            var currentDate = DateTime.Now;
            var sw = new Stopwatch();
            sw.Start();
            Response.ContentType = "text/comma-separated-values";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);

            var exportHelper = new CsvExportHelper<TResult>();
            for (var pageNumber = 1; pageNumber <=2; pageNumber++)
            {
                if (Response.IsClientConnected)
                {
                    request.PageNumber = 1;
                    var result = await func(request);
                    if (result != null && result.Results.Count > 0)
                    {
                        //request.MinId = result.MinId;
                        request.MaxId = result.MaxId;
                        using (var stream = new MemoryStream())
                        {
                            await exportHelper.WriteDataAsync(stream, result.Results, columns, pageNumber == 1);
                            Response.OutputStream.Write(stream.GetBuffer(), 0, (int)stream.Length);
                            Response.Flush();
                            await stream.FlushAsync();
                        }
                    }
                   
                }
                
            }
            sw.Stop();
           
            return new EmptyResult();
        }
    }
    
  
    public class CsvExportHelper<TResultModel>
       where TResultModel : class
    {
        private const string Array = "array";

        public async Task WriteDataAsync(Stream stream, List<TResultModel> data, string[] columns, bool isWriteHeader = false)
        {
            var streamWriter = new StreamWriter(stream, Encoding.UTF8);
            if (isWriteHeader)
            {
                await WriteHeaderAsync(streamWriter, columns);
            }
            await WriteDataAsync(streamWriter, data, columns);
            await streamWriter.FlushAsync();
        }

        private async Task WriteHeaderAsync(StreamWriter writer, string[] columns)
        {
            var properties = typeof(TResultModel).GetProperties()
                .Where(a => columns.Contains(a.Name))
                .Select(GetPropertyName)
                .ToList();
            //single line template: $array:{it|"$it$"};separator=","$
            var template = new Template($"${Array}:{{it|\"$it$\"}};separator=\",\"$", '$', '$');
            template.Add(Array, properties);
            await writer.WriteAsync(template.Render());
            //await writer.WriteLineAsync();
        }

        private async Task WriteDataAsync(StreamWriter writer, List<TResultModel> data, string[] columns)
        {
            var col = columns.ToList();
            col.Add("_tmp");
            //multi lines template: $array:{it|"$it.OrderNumber$","$it.OrderDate$","$it.OrderStatus$"};separator="\r\n"$
            //var template = new Template($"${Array}:{{it|{string.Join(",", columns.Select(a => $"\"$it.{a}$\""))}}};separator=\"\r\n\"$", '$', '$');
            var template = new Template($"${Array}:{{it|{string.Join(",", col.Select(a => (a.ToLower() == "shipmenttrackingnumber" || a.ToLower() == "trackingnumber") ? "=" + $"\"$it.{a}$\"" : $"\"$it.{a}$\""))}}};separator=\"\r\n\"$", '$', '$');

            template.Add(Array, data);
            await writer.WriteLineAsync();
            await writer.WriteAsync(template.Render());

        }

        private string GetPropertyName(PropertyInfo propertyInfo)
        {
            var attribute = propertyInfo.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? propertyInfo.Name;
        }

    }
}

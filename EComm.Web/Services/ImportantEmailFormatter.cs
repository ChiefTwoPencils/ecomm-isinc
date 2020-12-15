using EComm.Web.Interfaces;
using System;

namespace EComm.Web.Services
{
    public class ImportantEmailFormatter : IEmailFormatter
    {
        public ImportantEmailFormatter()
            => Console.WriteLine($"Instantiating the {nameof(ImportantEmailFormatter)}.");

        public string Format(string content)
            => $"{content.ToUpper()}!!!";
    }
}

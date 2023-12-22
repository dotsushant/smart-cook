using System.Linq;
using CrazyAboutPi.CoreLib;

namespace CrazyAboutPi.Server
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                return View["index"];
            };
        }
    }
}
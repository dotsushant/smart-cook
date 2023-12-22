using System.Linq;
using System.Collections.Generic;

using CrazyAboutPi.CoreLib;

namespace CrazyAboutPi.Server
{
    using Nancy;

    public class RecipeModule : NancyModule
    {
        public RecipeModule()
        {
            Get["/Recipes"] = parameters =>
            {
                return 200;
               // return View["Recipe", Recipe.Suggestions().ToArray()];
            };

            Get["/Recipes/{value:int}"] = parameters =>
            {
                return View["RecipeDetails", Recipe.Get(parameters.value)];
            };
        }
    }
}
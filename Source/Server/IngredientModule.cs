using System.Linq;
using CrazyAboutPi.CoreLib;

namespace CrazyAboutPi.Server
{
    using Nancy;

    public class IngredientModule : NancyModule
    {
        public IngredientModule()
        {
            Put["/ingredients"] = parameters =>
            {
                var requestBody = this.Request.Body;
                int requestBodyLength = (int)requestBody.Length; //this is a dynamic variable.
                byte[] ingredientIDs = new byte[requestBodyLength];
                requestBody.Read(ingredientIDs, 0, requestBodyLength);

                foreach (var ingredientID in ingredientIDs)
                {
                    //Ingredient.Update(ingredientID, true);
                }

                return 200;
            };
        }
    }
}
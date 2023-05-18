using AngleSharp;
using AngleSharp.Dom;
using CookingBot.Domain.Entities;
using CookingBot.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingBot.Application.Services
{
    public class ParserService
    {
        public CookingBotContext _context;
        public ParserService(CookingBotContext _context)
        {
            this._context = _context;
        }

        public async Task Parse()
        {
            List<Recipe> recipes =  _context.Recipes.ToList();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var config = Configuration.Default.WithDefaultLoader();
            int recipeId = 15000;
            using var context = BrowsingContext.New(config);

            List<Recipe> result = new List<Recipe>();
            while (recipeId > 14995)
            {
                Console.WriteLine(recipeId);
                recipeId--;
                string urlAddress = $"https://www.russianfood.com/recipes/recipe.php?rid={recipeId}";
                using var doc = await context.OpenAsync(urlAddress);
                var titleElement = doc.QuerySelector(".recipe_new h1.title");
                if (titleElement == null)
                {
                    continue;
                }
                var title = titleElement.Text();
                var picture = doc.QuerySelector(".recipe_new .main_image img")?.Attributes["src"];
                if(picture == null)
                {
                    continue;
                }
                var products = doc.QuerySelectorAll(".recipe_new #from span").Select(p => p.Text()).ToArray();
                var steps = doc.QuerySelectorAll(".recipe_new .step_images_n .step_n p").Select(p => p.Text()).ToArray();

                List<Ingredient> ingredients = products.Select(_=> new Ingredient { Name=_}).ToList();

                List<Step> _steps = steps.Select(_=> new Step { Number= Array.IndexOf(steps,_)+1, Description=_}).ToList();
                    
                Recipe recipe = new Recipe {Name= title, Picture=picture.Value, Steps=_steps, Ingredients=ingredients};

                result.Add(recipe);               

            }

            await _context.Recipes.AddRangeAsync(result);
            await _context.SaveChangesAsync();
        }
    }
}

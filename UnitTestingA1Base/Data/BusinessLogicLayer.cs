using System.Text.Json.Serialization;
using UnitTestingA1Base.Models;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json;

namespace UnitTestingA1Base.Data
{
    public class BusinessLogicLayer
    {
        public AppStorage _appStorage;

        public BusinessLogicLayer(AppStorage appStorage) {
            _appStorage = appStorage;
        }
        public HashSet<Recipe> GetRecipesByIngredient(int? id, string? name)
        {
            Ingredient? ingredient;
            HashSet<Recipe> recipes = new HashSet<Recipe>();

            if (id != null || !string.IsNullOrEmpty(name))
            {
                ingredient = _appStorage.Ingredients.FirstOrDefault(i => i.Id == id);

                if (!string.IsNullOrEmpty(name))
                {
                    ingredient ??= _appStorage.Ingredients.FirstOrDefault(i => i.Name.ToLower().Contains(name.ToLower()));
                }

                if (ingredient == null) throw new ArgumentException();

                HashSet<RecipeIngredient> recipeIngredients = _appStorage.RecipeIngredients
                    .Where(rI => rI.IngredientId == ingredient?.Id).ToHashSet();

                recipes = _appStorage.Recipes
                    .Where(r => recipeIngredients
                    .Any(ri => ri.RecipeId == r.Id))
                    .ToHashSet();
            }

            return recipes;
        }
        public HashSet<Recipe> GetRecipesByDiet(int? id, string? name)
        {
            DietaryRestriction? dietaryRestriction;
            HashSet<Recipe> recipes = new HashSet<Recipe>();

            if (id != null || !string.IsNullOrEmpty(name))
            {
                dietaryRestriction = _appStorage.DietaryRestrictions.FirstOrDefault(dr => dr.Id == id);

                if (!string.IsNullOrEmpty(name))
                {
                    dietaryRestriction ??= _appStorage.DietaryRestrictions.FirstOrDefault(dr => dr.Name.ToLower().Contains(name.ToLower()));
                }

                if (dietaryRestriction == null) throw new ArgumentException();

                HashSet<IngredientRestriction> ingredientRestrictions = _appStorage.IngredientRestrictions
                    .Where(ir => ir.DietaryRestrictionId == dietaryRestriction.Id).ToHashSet();

                recipes = _appStorage.Recipes
                    .Where(r => _appStorage.RecipeIngredients
                    .Where(ri => ri.RecipeId == r.Id)
                    .All(ri => ingredientRestrictions.Any(ir => ir.IngredientId == ri.IngredientId)))
                    .ToHashSet();
            }

            return recipes;
        }
        public HashSet<Recipe> GetRecipesByNameOrPK(int? id, string? name)
        {
            HashSet<Recipe> recipes = new HashSet<Recipe>();

            if (id != null)
            {
                recipes = _appStorage.Recipes.Where(r => r.Id == id).ToHashSet();
            }

            if (!string.IsNullOrEmpty(name))
            {
                recipes = recipes.Count <= 0 ? _appStorage.Recipes.Where(r => r.Name.ToLower().Contains(name.ToLower())).ToHashSet() : recipes;
            }

            return recipes;
        }
        public async Task<HashSet<Recipe>> CreateNewRecipe(HttpContext context)
        {
            StreamReader reader = new StreamReader(context.Request.Body);
            string requestBody = await reader.ReadToEndAsync();
            RecipeAndIngredientsRequest? request = JsonSerializer.Deserialize<RecipeAndIngredientsRequest>(requestBody);

            if (request == null) throw new ArgumentException();

            if (_appStorage.Recipes.Any(r => r.Name == request.Recipe.Name))
            {
                throw new InvalidOperationException("Recipe with the same name already exists.");
            }

            Recipe newRecipe = new Recipe
            {
                Id = _appStorage.GeneratePrimaryKey(),
                Name = request.Recipe.Name,
                Description = request.Recipe.Description,
                Servings = request.Recipe.Servings
            };
            _appStorage.Recipes.Add(newRecipe);


            foreach (Ingredient ingredient in request.Ingredients)
            {
                if (!_appStorage.Ingredients.Any(i => i.Name == ingredient.Name))
                {
                    Ingredient newIngredient = new Ingredient
                    {
                        Id = _appStorage.GeneratePrimaryKey(),
                        Name = ingredient.Name
                    };
                    _appStorage.Ingredients.Add(newIngredient);
                }

                RecipeIngredient recipeIngredient = new RecipeIngredient
                {
                    IngredientId = _appStorage.Ingredients.First(i => i.Name == ingredient.Name).Id,
                    RecipeId = newRecipe.Id,
                    Amount = 100,
                    MeasurementUnit = MeasurementUnit.Grams
                };
                _appStorage.RecipeIngredients.Add(recipeIngredient);
            }

            return new HashSet<Recipe> { newRecipe };
        }
        public HashSet<Ingredient> DeleteIngredient(int? id, string? name)
        {
            Ingredient? ingredient;
            HashSet<Ingredient> ingredients = new HashSet<Ingredient>();

            if (id != null || !string.IsNullOrEmpty(name))
            {
                ingredient = _appStorage.Ingredients.FirstOrDefault(i => i.Id == id);

                if (!string.IsNullOrEmpty(name))
                {
                    ingredient ??= _appStorage.Ingredients.FirstOrDefault(i => i.Name.ToLower().Contains(name.ToLower()));
                }

                if (ingredient == null) throw new ArgumentException();

                ingredients.Add(ingredient);

                HashSet<RecipeIngredient> recipesUsingIngredient = _appStorage.RecipeIngredients
                    .Where(ri => ri.IngredientId == ingredient.Id)
                    .ToHashSet();

                if (recipesUsingIngredient.Count == 0)
                {
                    _appStorage.Ingredients.Remove(ingredient);
                }
                else if (recipesUsingIngredient.Count == 1)
                {
                    int recipeId = recipesUsingIngredient.First().RecipeId;

                    _appStorage.RecipeIngredients.RemoveWhere(ri => ri.RecipeId == recipeId);

                    _appStorage.Recipes.RemoveWhere(r => r.Id == recipeId);

                    _appStorage.Ingredients.Remove(ingredient);
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                throw new ArgumentException();
            }

            return ingredients;
        }
        public HashSet<Recipe> DeleteRecipe(int? id, string? name)
        {
            Recipe? recipe;
            HashSet<Recipe> recipes = new HashSet<Recipe>();

            if (id != null || !string.IsNullOrEmpty(name))
            {
                recipe = _appStorage.Recipes.FirstOrDefault(r => r.Id == id);

                if (!string.IsNullOrEmpty(name))
                {
                    recipe ??= _appStorage.Recipes.FirstOrDefault(r => r.Name.ToLower().Contains(name.ToLower()));
                }

                if (recipe == null) throw new ArgumentException();

                recipes.Add(recipe);

                HashSet<RecipeIngredient> recipeIngredients = _appStorage.RecipeIngredients
                    .Where(ri => ri.RecipeId == recipe.Id)
                    .ToHashSet();

                foreach (RecipeIngredient recipeIngredient in recipeIngredients)
                {
                    _appStorage.RecipeIngredients.Remove(recipeIngredient);
                }

                _appStorage.Recipes.Remove(recipe);
            }
            else
            {
                throw new ArgumentException();
            }

            return recipes;
        }
    }

    public class RecipeAndIngredientsRequest
    {
        public Recipe Recipe { get; set; }
        public HashSet<Ingredient> Ingredients { get; set; }
    }
}

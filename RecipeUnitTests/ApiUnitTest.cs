using Microsoft.AspNetCore.Http;
using System.Text.Json;
using UnitTestingA1Base.Data;
using UnitTestingA1Base.Models;

namespace RecipeUnitTests
{
    [TestClass]
    public class ApiUnitTest
    {
        private BusinessLogicLayer _initializeBusinessLogic()
        {
            return new BusinessLogicLayer(new AppStorage());
        }

        #region GetRecipesByIngredient
        [TestMethod]
        public void GetRecipesByIngredient_ValidId_ReturnsRecipesWithIngredient()
        {
            // arrange
            BusinessLogicLayer bll = _initializeBusinessLogic();
            int ingredientId = 6;
            int recipeCount = 2;

            // act
            HashSet<Recipe> recipes = bll.GetRecipesByIngredient(ingredientId, null);

            Assert.AreEqual(recipeCount, recipes.Count);
        }

        [TestMethod]
        public void GetRecipesByIngredient_ValidName_ReturnsRecipesWithIngredient()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string ingredientName = "Salmon";
            int recipeCount = 2;

            HashSet<Recipe> recipes = bll.GetRecipesByIngredient(null, ingredientName);

            Assert.AreEqual(recipeCount, recipes.Count);
        }

        [TestMethod]
        public void GetRecipesByIngredient_IncompleteValidName_ReturnsRecipesWithIngredient()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string ingredientName = "Sal";
            int recipeCount = 2;

            HashSet<Recipe> recipes = bll.GetRecipesByIngredient(null, ingredientName);

            Assert.AreEqual(recipeCount, recipes.Count);
        }

        [TestMethod]
        public void GetRecipesByIngredient_InvalidId_ThrowsException()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            int ingredientId = 11;

            Assert.ThrowsException<ArgumentException>(() =>
            {
                HashSet<Recipe> recipes = bll.GetRecipesByIngredient(ingredientId, null);
            });
        }

        [TestMethod]
        public void GetRecipesByIngredient_InvalidName_ThrowsException()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string ingredientName = "qwe123";

            Assert.ThrowsException<ArgumentException>(() =>
            {
                HashSet<Recipe> recipes = bll.GetRecipesByIngredient(null, ingredientName);
            });
        }

        [TestMethod]
        public void GetRecipesByIngredient_NullArguments_ReturnsEmptyCollection()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();

            HashSet<Recipe> recipes = bll.GetRecipesByIngredient(null, null);

            Assert.AreEqual(0, recipes.Count);
        }
        #endregion


        #region GetRecipesByDiet
        [TestMethod]
        public void GetRecipesByDiet_ValidId_ReturnsRecipesInDiet()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            int dietId = 1;
            int recipeCount = 3;

            HashSet<Recipe> recipes = bll.GetRecipesByDiet(dietId, null);

            Assert.AreEqual(recipeCount, recipes.Count);
        }

        [TestMethod]
        public void GetRecipesByDiet_ValidName_ReturnsRecipesInDiet()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string dietName = "Vegetarian";
            int recipeCount = 3;

            HashSet<Recipe> recipes = bll.GetRecipesByDiet(null, dietName);

            Assert.AreEqual(recipeCount, recipes.Count);
        }

        [TestMethod]
        public void GetRecipesByDiet_IncompleteValidName_ReturnsRecipesInDiet()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string dietName = "Veget";
            int recipeCount = 3;

            HashSet<Recipe> recipes = bll.GetRecipesByDiet(null, dietName);

            Assert.AreEqual(recipeCount, recipes.Count);
        }

        [TestMethod]
        public void GetRecipesByDiet_InvalidId_ThrowsException()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            int dietId = 6;

            Assert.ThrowsException<ArgumentException>(() =>
            {
                HashSet<Recipe> recipes = bll.GetRecipesByDiet(dietId, null);
            });
        }

        [TestMethod]
        public void GetRecipesByDiet_InvalidName_ThrowsException()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string dietName = "qwe123";

            Assert.ThrowsException<ArgumentException>(() =>
            {
                HashSet<Recipe> recipes = bll.GetRecipesByDiet(null, dietName);
            });
        }

        [TestMethod]
        public void GetRecipesByDiet_NullArguments_ReturnsEmptyCollection()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();

            HashSet<Recipe> recipes = bll.GetRecipesByDiet(null, null);

            Assert.AreEqual(0, recipes.Count);
        }
        #endregion


        #region GetRecipesByNameOrPK
        [TestMethod]
        public void GetRecipesByNameOrPK_ValidId_ReturnsCorrectRecipe()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            int recipeId = 4;

            HashSet<Recipe> recipe = bll.GetRecipesByNameOrPK(recipeId, null);

            Assert.AreEqual(recipeId, recipe.First().Id);
        }

        [TestMethod]
        public void GetRecipesByNameOrPK_ValidName_ReturnsCorrectRecipe()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string recipeName = "Margherita Pizza";

            HashSet<Recipe> recipe = bll.GetRecipesByNameOrPK(null, recipeName);

            Assert.AreEqual(recipeName, recipe.First().Name);
        }

        [TestMethod]
        public void GetRecipesByNameOrPK_IncompleteValidName_ReturnsCorrectRecipe()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string recipeFullName = "Margherita Pizza";
            string recipeNameSection = "rita";

            HashSet<Recipe> recipe = bll.GetRecipesByNameOrPK(null, recipeNameSection);

            Assert.AreEqual(recipeFullName, recipe.First().Name);
        }

        [TestMethod]
        public void GetRecipesByNameOrPK_SingleLetter_ReturnsCorrectRecipes()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string recipeLetter = "c";
            int recipeCount = 6;

            HashSet<Recipe> recipe = bll.GetRecipesByNameOrPK(null, recipeLetter);

            Assert.AreEqual(recipeCount, recipe.Count);
        }

        [TestMethod]
        public void GetRecipesByNameOrPK_InvalidId_ReturnsEmptyCollection()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            int recipeId = 13;

            HashSet<Recipe> recipe = bll.GetRecipesByNameOrPK(recipeId, null);

            Assert.AreEqual(0, recipe.Count);
        }

        [TestMethod]
        public void GetRecipesByNameOrPK_InvalidName_ReturnsEmptyCollection()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string recipeName = "qwe123";

            HashSet<Recipe> recipe = bll.GetRecipesByNameOrPK(null, recipeName);

            Assert.AreEqual(0, recipe.Count);
        }

        [TestMethod]
        public void GetRecipesByNameOrPK_NullArguments_ReturnsEmptyCollection()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();

            HashSet<Recipe> recipe = bll.GetRecipesByNameOrPK(null, null);

            Assert.AreEqual(0, recipe.Count);
        }
        #endregion


        #region CreateNewRecipe
        [TestMethod]
        public async Task CreateNewRecipe_WithValidRequest_CreatesRecipeIngredientsAndRelations()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();

            RecipeAndIngredientsRequest request = new RecipeAndIngredientsRequest
            {
                Recipe = new Recipe
                {
                    Name = "NewRecipe",
                    Description = "Description",
                    Servings = 4
                },
                Ingredients = new HashSet<Ingredient>
                {
                    new Ingredient { Name = "Ingredient1" },
                    new Ingredient { Name = "Ingredient2" }
                }
            };

            HttpContext context = CreateHttpContextWithRequestBody(request);

            HashSet<Recipe> recipes = await bll.CreateNewRecipe(context);

            Assert.IsTrue(bll._appStorage.Recipes.Any(r => r.Name == request.Recipe.Name));
            Assert.IsTrue(bll._appStorage.Ingredients.Any(i => i.Name == request.Ingredients.ElementAt(0).Name));
            Assert.IsTrue(bll._appStorage.Ingredients.Any(i => i.Name == request.Ingredients.ElementAt(1).Name));
            Assert.AreEqual(2, bll._appStorage.RecipeIngredients.Where(ri => ri.RecipeId == recipes.ElementAt(0).Id).Count());
        }

        [TestMethod]
        public void CreateNewRecipe_ExistingRecipeName_ThrowsException()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();

            RecipeAndIngredientsRequest request = new RecipeAndIngredientsRequest
            {
                Recipe = new Recipe
                {
                    Name = "Spaghetti Carbonara",
                    Description = "Description",
                    Servings = 4
                },
                Ingredients = new HashSet<Ingredient>
                {
                    new Ingredient { Name = "Ingredient1" },
                    new Ingredient { Name = "Ingredient2" }
                }
            };

            HttpContext context = CreateHttpContextWithRequestBody(request);

            Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                HashSet<Recipe> recipes = await bll.CreateNewRecipe(context);
            });
        }

        [TestMethod]
        public async Task CreateNewRecipe_ExistingIngredientName_DoesNotAddNewIngredient()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();

            RecipeAndIngredientsRequest request = new RecipeAndIngredientsRequest
            {
                Recipe = new Recipe
                {
                    Name = "NewRecipe",
                    Description = "Description",
                    Servings = 4
                },
                Ingredients = new HashSet<Ingredient>
                {
                    new Ingredient { Name = "Eggs" }
                }
            };

            HttpContext context = CreateHttpContextWithRequestBody(request);

            HashSet<Recipe> recipes = await bll.CreateNewRecipe(context);

            Assert.IsTrue(bll._appStorage.Recipes.Any(r => r.Name == request.Recipe.Name));
            Assert.IsTrue(bll._appStorage.Ingredients.Where(i => i.Name == request.Ingredients.ElementAt(0).Name).Count() < 2);
            Assert.AreEqual(1, bll._appStorage.RecipeIngredients.Where(ri => ri.RecipeId == recipes.ElementAt(0).Id).Count());
        }


        private HttpContext CreateHttpContextWithRequestBody(RecipeAndIngredientsRequest request)
        {
            var context = new DefaultHttpContext();

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(JsonSerializer.Serialize(request));
            writer.Flush();
            stream.Position = 0;

            context.Request.Body = stream;

            return context;
        }
        #endregion


        #region DeleteIngredient
        [TestMethod]
        public void DeleteIngredient_ValidIdOneRecipe_RemovesIngredientAndRecipeFromAppStorage()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            int ingredientId = 2;
            int recipeId = 1;

            Assert.IsTrue(bll._appStorage.Ingredients.Any(i => i.Id == ingredientId));
            Assert.IsTrue(bll._appStorage.Recipes.Any(r => r.Id == recipeId));
            Assert.IsTrue(bll._appStorage.RecipeIngredients.Any(ri => ri.RecipeId == recipeId));

            HashSet<Ingredient> ingredient = bll.DeleteIngredient(ingredientId, null);

            Assert.AreEqual(ingredientId, ingredient.First().Id);
            Assert.IsFalse(bll._appStorage.Ingredients.Any(i => i.Id == ingredientId));
            Assert.IsFalse(bll._appStorage.Recipes.Any(r => r.Id == recipeId));
            Assert.IsFalse(bll._appStorage.RecipeIngredients.Any(ri => ri.RecipeId == recipeId));
        }

        [TestMethod]
        public void DeleteIngredient_ValidIdManyRecipes_ThrowsException()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            int ingredientId = 8;

            Assert.ThrowsException<Exception>(() =>
            {
                bll.DeleteIngredient(ingredientId, null);
            });
        }

        [TestMethod]
        public void DeleteIngredient_ValidNameOneRecipe_RemovesIngredientAndRecipeFromAppStorage()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string ingredientName = "Eggs";
            int recipeId = 1;

            Assert.IsTrue(bll._appStorage.Ingredients.Any(i => i.Name == ingredientName));
            Assert.IsTrue(bll._appStorage.Recipes.Any(r => r.Id == recipeId));
            Assert.IsTrue(bll._appStorage.RecipeIngredients.Any(ri => ri.RecipeId == recipeId));

            HashSet<Ingredient> ingredient = bll.DeleteIngredient(null, ingredientName);

            Assert.AreEqual(ingredientName, ingredient.First().Name);
            Assert.IsFalse(bll._appStorage.Ingredients.Any(i => i.Name == ingredientName));
            Assert.IsFalse(bll._appStorage.Recipes.Any(r => r.Id == recipeId));
            Assert.IsFalse(bll._appStorage.RecipeIngredients.Any(ri => ri.RecipeId == recipeId));
        }

        [TestMethod]
        public void DeleteIngredient_ValidNameManyRecipes_ThrowsException()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string ingredientName = "Parmesan Cheese";

            Assert.ThrowsException<Exception>(() =>
            {
                bll.DeleteIngredient(null, ingredientName);
            });
        }

        [TestMethod]
        public void DeleteIngredient_IncompleteValidNameOneRecipe_RemovesIngredientAndRecipeFromAppStorage()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string ingredientFullName = "Eggs";
            string ingredientNameSection = "gg";
            int recipeId = 1;

            Assert.IsTrue(bll._appStorage.Ingredients.Any(i => i.Name == ingredientFullName));
            Assert.IsTrue(bll._appStorage.Recipes.Any(r => r.Id == recipeId));
            Assert.IsTrue(bll._appStorage.RecipeIngredients.Any(ri => ri.RecipeId == recipeId));

            HashSet<Ingredient> ingredient = bll.DeleteIngredient(null, ingredientNameSection);

            Assert.AreEqual(ingredientFullName, ingredient.First().Name);
            Assert.IsFalse(bll._appStorage.Ingredients.Any(i => i.Name == ingredientFullName));
            Assert.IsFalse(bll._appStorage.Recipes.Any(r => r.Id == recipeId));
            Assert.IsFalse(bll._appStorage.RecipeIngredients.Any(ri => ri.RecipeId == recipeId));
        }

        [TestMethod]
        public void DeleteIngredient_IncompleteValidNameManyRecipes_ThrowsException()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string ingredientFullName = "Parmesan Cheese";
            string ingredientNameSection = "mesan";

            Assert.ThrowsException<Exception>(() =>
            {
                bll.DeleteIngredient(null, ingredientNameSection);
            });
        }

        [TestMethod]
        public void DeleteIngredient_InvalidId_ThrowsException()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            int ingredientId = 11;

            Assert.ThrowsException<ArgumentException>(() =>
            {
                HashSet<Ingredient> ingredient = bll.DeleteIngredient(ingredientId, null);
            });
        }

        [TestMethod]
        public void DeleteIngredient_InvalidName_ThrowsException()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string ingredientName = "qwe123";

            Assert.ThrowsException<ArgumentException>(() =>
            {
                bll.DeleteIngredient(null, ingredientName);
            });
        }

        [TestMethod]
        public void DeleteIngredient_NullArguments_ThrowsException()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                bll.DeleteIngredient(null, null);
            });
        }
        #endregion


        #region DeleteRecipe
        [TestMethod]
        public void DeleteRecipe_ValidId_RemovesRecipeFromAppStorage()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            int recipeId = 1;

            Assert.IsTrue(bll._appStorage.Recipes.Any(r => r.Id == recipeId));
            Assert.IsTrue(bll._appStorage.RecipeIngredients.Any(ri => ri.RecipeId == recipeId));

            HashSet<Recipe> recipes = bll.DeleteRecipe(recipeId, null);

            Assert.AreEqual(recipeId, recipes.First().Id);
            Assert.IsFalse(bll._appStorage.Recipes.Any(r => r.Id == recipeId));
            Assert.IsFalse(bll._appStorage.RecipeIngredients.Any(ri => ri.RecipeId == recipeId));
        }

        [TestMethod]
        public void DeleteRecipe_ValidName_RemovesRecipeFromAppStorage()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string recipeName = "Spaghetti Carbonara";
            int recipeId = 1;

            Assert.IsTrue(bll._appStorage.Recipes.Any(r => r.Name == recipeName));
            Assert.IsTrue(bll._appStorage.RecipeIngredients.Any(ri => ri.RecipeId == recipeId));

            HashSet<Recipe> recipes = bll.DeleteRecipe(null, recipeName);

            Assert.AreEqual(recipeName, recipes.First().Name);
            Assert.IsFalse(bll._appStorage.Recipes.Any(r => r.Name == recipeName));
            Assert.IsFalse(bll._appStorage.RecipeIngredients.Any(ri => ri.RecipeId == recipeId));
        }

        [TestMethod]
        public void DeleteRecipe_IncompleteValidName_RemovesRecipeFromAppStorage()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string recipeFullName = "Spaghetti Carbonara";
            string recipeNameSection = "hetti";
            int recipeId = 1;

            Assert.IsTrue(bll._appStorage.Recipes.Any(r => r.Name == recipeFullName));
            Assert.IsTrue(bll._appStorage.RecipeIngredients.Any(ri => ri.RecipeId == recipeId));

            HashSet<Recipe> recipes = bll.DeleteRecipe(null, recipeNameSection);

            Assert.AreEqual(recipeFullName, recipes.First().Name);
            Assert.IsFalse(bll._appStorage.Recipes.Any(r => r.Name == recipeFullName));
            Assert.IsFalse(bll._appStorage.RecipeIngredients.Any(ri => ri.RecipeId == recipeId));
        }

        [TestMethod]
        public void DeleteRecipe_InvalidId_ThrowsException()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            int recipeId = 13;

            Assert.ThrowsException<ArgumentException>(() =>
            {
                bll.DeleteRecipe(recipeId, null);
            });
        }

        [TestMethod]
        public void DeleteRecipe_InvalidName_ThrowsException()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string recipeName = "qwe123";

            Assert.ThrowsException<ArgumentException>(() =>
            {
                bll.DeleteRecipe(null, recipeName);
            });
        }

        [TestMethod]
        public void DeleteRecipe_NullArguments_ThrowsException()
        {
            BusinessLogicLayer bll = _initializeBusinessLogic();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                bll.DeleteRecipe(null, null);
            });
        }
        #endregion
    }
}
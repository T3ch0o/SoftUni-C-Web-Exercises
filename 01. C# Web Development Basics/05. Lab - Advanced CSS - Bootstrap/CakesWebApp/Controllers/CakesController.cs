namespace CakesWebApp.Controllers
{
    using CakesWebApp.Models;

    public class CakesController : BaseController
    {
        public IHttpResponse AddCakes(IHttpRequest request)
        {
            return View("AddCakes");
        }

        public IHttpResponse DoAddCakes(IHttpRequest request)
        {
            var name = request.FormData["name"].ToString().Trim().UrlDecode();
            var price = decimal.Parse(request.FormData["price"].ToString().UrlDecode());
            var picture = request.FormData["picture"].ToString().Trim().UrlDecode();

            // TODO: Validation

            Product product = new Product { Name = name, Price = price, ImageUrl = picture };
            Db.Products.Add(product);

            try
            {
                Db.SaveChanges();
            }
            catch (Exception e)
            {
                // TODO: Log error
                return ServerError(e.Message);
            }

            // Redirect
            return new RedirectResult("/");
        }

        public IHttpResponse ById(IHttpRequest request)
        {
            var id = int.Parse(request.QueryData["id"].ToString());
            var product = Db.Products.FirstOrDefault(x => x.Id == id);
            if (product == null) return BadRequestError("Cake not found.");

            var viewBag = new Dictionary<string, string> { { "Name", product.Name }, { "Price", product.Price.ToString(CultureInfo.InvariantCulture) }, { "ImageUrl", product.ImageUrl } };
            return View("CakeById", viewBag);
        }
    }
}
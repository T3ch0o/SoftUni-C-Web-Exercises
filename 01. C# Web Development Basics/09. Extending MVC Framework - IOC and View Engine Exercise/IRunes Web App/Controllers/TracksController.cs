namespace IRunes.Controllers
{
    using System;
    using System.Net;

    using IRunes.Models;

    using SIS.HTTP.Requests.Interfaces;
    using SIS.HTTP.Responses.Interfaces;

    internal class TracksController : BaseController
    {
        public IHttpResponse CreateGet(IHttpRequest request)
        {
            if (!IsAuthenticated(request))
            {
                return Redirect("/users/login");
            }

            if (!request.QueryData.ContainsKey("albumId"))
            {
                return Redirect("/albums/all");
            }

            string albumId = request.QueryData["albumId"].ToString();

            ViewBag["albumId"] = albumId;

            return View();
        }

        public IHttpResponse CreatePost(IHttpRequest request)
        {
            string name = request.FormData["name"].ToString().Trim();
            string link = request.FormData["link"].ToString();
            decimal price = decimal.Parse(request.FormData["price"].ToString());

            if (name == string.Empty || link == string.Empty)
            {
                BadRequestError("Please fill the form.");
            }

            if (price <= 0)
            {
                BadRequestError("Price is to low.");
            }

            string albumId = request.QueryData["albumId"].ToString();
            link = link.Replace("https://www.youtube.com/watch?v=", string.Empty);

            Track track = new Track
            {
                Name = name,
                Link = link,
                Price = price,
                AlbumId = albumId
            };

            Db.Tracks.Add(track);

            try
            {
                Db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequestError(e.Message);
            }

            return Redirect($"/albums/details?id={albumId}");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!IsAuthenticated(request))
            {
                return Redirect("/users/login");
            }

            if (!request.QueryData.ContainsKey("albumId") || !request.QueryData.ContainsKey("trackId"))
            {
                return Redirect("/albums/all");
            }

            ViewBag["albumId"] = request.QueryData["albumId"].ToString();
            string trackId = request.QueryData["trackId"].ToString();

            Track track = Db.Tracks.Find(trackId);

            ViewBag["name"] = WebUtility.UrlDecode(track.Name);
            ViewBag["link"] = WebUtility.UrlDecode(track.Link);
            ViewBag["price"] = track.Price.ToString("F2");

            return View();
        }
    }
}
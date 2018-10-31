namespace IRunes.Controllers
{
    using System;
    using System.Collections.Generic;

    using IRunes.Models;

    using SIS.HTTP.Requests.Interfaces;
    using SIS.HTTP.Responses.Interfaces;
    using System.Linq;
    using System.Net;
    using System.Text;

    using IRunes.Services;
    using IRunes.Services.Interfaces;

    internal class AlbumsController : BaseController
    {
        private readonly IAlbumService albumsService = new AlbumsService();

        public IHttpResponse All(IHttpRequest request)
        {
            if (!IsAuthenticated(request))
            {
                return Redirect("/users/login");
            }

            List<Album> albums = Db.Albums.ToList();
            StringBuilder listOfAlbums = new StringBuilder();

            if (albums.Any())
            {
                foreach (Album album in albums)
                {
                    listOfAlbums.AppendLine($"<a class=\"font-weight-bold\" href=\"/albums/details?id={album.Id}\">{album.Name}</a>");
                }

                ViewBag["albums"] = listOfAlbums.ToString();
            }
            else
            {
                ViewBag["albums"] = "There are currently no albums.";
            }

            return View();
        }

        public IHttpResponse CreateGet(IHttpRequest request)
        {
            if (!IsAuthenticated(request))
            {
                return Redirect("/users/login");
            }

            return View();
        }

        public IHttpResponse CreatePost(IHttpRequest request)
        {
            string name = request.FormData["name"].ToString();
            string cover = request.FormData["cover"].ToString();

            if (name == string.Empty)
            {
                return BadRequestError("Album name is required");
            }

            if (cover == string.Empty)
            {
                return BadRequestError("Cover name is required");
            }

            Album album = new Album
            {
                Name = name,
                Cover = cover
            };

            Db.Albums.Add(album);

            try
            {
                Db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequestError(e.Message);
            }

            return Redirect("/albums/all");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!IsAuthenticated(request))
            {
                return Redirect("/users/login");
            }

            if (!request.QueryData.ContainsKey("id"))
            {
                return Redirect("/albums/all");
            }

            string albumId = request.QueryData["id"].ToString();
            Album album = Db.Albums.Find(albumId);

            if (album == null)
            {
                return BadRequestError("Album does not exist.");
            }

            ViewBag["name"] = album.Name;
            ViewBag["cover"] = WebUtility.UrlDecode(album.Cover);
            ViewBag["albumId"] = albumId;

            List<Track> albumTracks = album.Tracks.ToList();

            StringBuilder resultHtml = new StringBuilder();

            if (albumTracks.Any())
            {
                resultHtml.AppendLine("<ol>");
                decimal price = 0;
                foreach (Track track in albumTracks)
                {
                    string trackText = $"<li><div><a href=/tracks/details?albumId={albumId}&trackId={track.Id}>{WebUtility.UrlDecode(track.Name)}</a></div></li><br/>";
                    price += track.Price;
                    resultHtml.AppendLine(trackText);
                }

                resultHtml.AppendLine("</ol>");

                ViewBag["price"] = albumsService.GetPrice(price);
                ViewBag["tracks"] = resultHtml.ToString();
            }
            else
            {
                ViewBag["tracks"] = "There is no tracks";
            }

            return View();
        }
    }
}
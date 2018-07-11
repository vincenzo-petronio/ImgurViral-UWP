using ImgurViralUWP.Exceptions;
using ImgurViralUWP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ImgurViralUWP.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class DataService : IDataService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task<List<GalleryImageData>> GetGalleryImage(Action<List<GalleryImageData>, Exception> callback)
        {
            Exception exception = null;
            List<GalleryImageData> results = new List<GalleryImageData>();
            String uri = Constants.ENDPOINT_API_GALLERY_VIRAL;
            var response = String.Empty;

            try
            {
                response = await this.DownloadData(uri);

                GalleryImage responseDeserialized = JsonConvert.DeserializeObject<GalleryImage>(response);
                // Filtro gli item che non sono visualizzabili, esempio video o album o GIF.
                var responseDeserializedRestricted = from item in responseDeserialized.Data
                                                     where !item.IsAlbum
                                                     && !item.Type.Contains("gif")
                                                     select item;
                foreach (var d in responseDeserializedRestricted)
                {
                    results.Add(d);
                }
            }
            catch (NetworkException ne)
            {
                exception = ne;
            }
            catch (ArgumentNullException ane)
            {
                exception = ane;
            }
            catch (ApiException ae)
            {
                exception = ae;
            }

            System.Diagnostics.Debug.WriteLine("[URI]\t{0}\n[RESPONSE]{1}\n\n", uri, response);

            callback(results, exception);

            return null;
        }


        public async Task<List<AlbumImageCommentsData>> GetAlbumImageComments(Action<List<AlbumImageCommentsData>, Exception> callback, String id)
        {
            Exception exception = null;
            List<AlbumImageCommentsData> results = new List<AlbumImageCommentsData>();
            String uri = String.Format(Constants.ENDPOINT_API_ALBUMIMAGECOMMENTS, id, "best"); //"https://api.imgur.com/3/gallery/image/8W62Swc/comments/best";
            var response = String.Empty;

            try
            {
                response = await this.DownloadData(uri);

                AlbumImageComments responseDeserialized = JsonConvert.DeserializeObject<AlbumImageComments>(response);
                var responseDeserializedSelected = from item in responseDeserialized.Data select item;
                foreach (var d in responseDeserializedSelected)
                {
                    results.Add(d);
                }
            }
            catch (NetworkException ne)
            {
                exception = ne;
            }
            catch (ArgumentNullException ane)
            {
                exception = ane;
            }
            catch (ApiException ae)
            {
                exception = ae;
            }

            System.Diagnostics.Debug.WriteLine("[URI]\t{0}\n[RESPONSE]{1}\n\n", uri, response);

            callback(results, exception);

            return null;
        }

        /// <summary>
        /// Get data from url
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private async Task<String> DownloadData(String uri)
        {
            var response = String.Empty;
            HttpResponseMessage httpResponseMessage;
            HttpClient httpClient = new HttpClient();
            AuthUser authUser;

            if (!NetworkHelper.CheckConnectivity())
            {
                throw new NetworkException("NO_NET");
            }

            authUser = await AuthHelper.ReadAuthData();
            if (null == authUser || String.IsNullOrEmpty(authUser.AccessToken) || String.IsNullOrEmpty(authUser.RefreshToken))
            {
                throw new ArgumentNullException("NO_LOCAL_TOKEN");
            }

            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Bearer " + authUser.AccessToken);
                httpResponseMessage = await httpClient.GetAsync(new Uri(uri));
                response = await httpResponseMessage.Content.ReadAsStringAsync();
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("BAD_GET_URI");
            }

            switch (httpResponseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    {
                        // 200
                        Debug.WriteLine("[DataService.DownloadData]\t" + "HttpStatusCode 200 - OK");
                        break;
                    }
                case System.Net.HttpStatusCode.BadRequest:
                    {
                        // 400
                        Debug.WriteLine("[DataService.DownloadData]\t" + "HttpStatusCode 400 - BadRequest");
                        throw new ApiException(response);
                    }
                case System.Net.HttpStatusCode.Unauthorized:
                    {
                        // 401
                        Debug.WriteLine("[DataService.DownloadData]\t" + "HttpStatusCode 401 - Unauthorized");
                        throw new ApiException(response);
                    }
                case System.Net.HttpStatusCode.Forbidden:
                    {
                        // 403
                        Debug.WriteLine("[DataService.DownloadData]\t" + "HttpStatusCode 403 - Forbidden");
                        bool isNewToken = await RefreshAccessToken(authUser.RefreshToken);

                        authUser = await AuthHelper.ReadAuthData();
                        if (!isNewToken || null == authUser || String.IsNullOrEmpty(authUser.AccessToken) || String.IsNullOrEmpty(authUser.RefreshToken))
                        {
                            throw new ArgumentNullException("NO_LOCAL_TOKEN");
                        }

                        try
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Bearer " + authUser.AccessToken);
                            httpResponseMessage = await httpClient.GetAsync(new Uri(uri));
                            response = await httpResponseMessage.Content.ReadAsStringAsync();
                        }
                        catch (ArgumentNullException)
                        {
                            throw new ArgumentNullException("BAD_GET_URI");
                        }

                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            // OK
                        }
                        else
                        {
                            throw new ApiException(response);
                        }

                        break;
                    }
                case System.Net.HttpStatusCode.NotFound:
                    {
                        // 404
                        Debug.WriteLine("[DataService.DownloadData]\t" + "HttpStatusCode 404 - NotFound");
                        throw new ApiException(response);
                    }
                //case "429":
                //{
                //    // 429 Rate Limiting
                //    Debug.WriteLine("[DataService.DownloadData]\t" + "HttpStatusCode 429 - Rate Limiting");
                //    break;
                //}
                case System.Net.HttpStatusCode.InternalServerError:
                    {
                        // 500
                        Debug.WriteLine("[DataService.DownloadData]\t" + "HttpStatusCode 500 - InternalServerError");
                        throw new ApiException(response);
                    }
            }

            return response;
        }

        /// <summary>
        /// Ottiene un nuovo Access Token attraverso il Refresh Token, e memorizza i nuovi dati in locale.
        /// </summary>
        /// <param name="refreshToken">String</param>
        /// <returns>bool</returns>
        private async static Task<bool> RefreshAccessToken(String refreshToken)
        {
            var response = String.Empty;
            HttpResponseMessage httpResponseMessage;
            HttpClient httpClient = new HttpClient();

            // QUERYSTRING
            var values = new Dictionary<string, string>();
            values.Add(Constants.AUTH_REFRESH, refreshToken);
            values.Add(Constants.AUTH_CLIENT_ID, Constants.API_CLIENTID);
            values.Add(Constants.AUTH_CLIENT_SECRET, Constants.API_SECRET);
            values.Add(Constants.AUTH_GRANT_TYPE, "refresh_token");
            var content = new FormUrlEncodedContent(values);

            try
            {
                httpResponseMessage = await httpClient.PostAsync(new Uri(Constants.ENDPOINT_API_REFRESH_BASE), content);
                response = await httpResponseMessage.Content.ReadAsStringAsync();
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("BAD_POST_URI");
            }

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Debug.WriteLine("[DataService.RefreshAccessToken]\t" + httpResponseMessage.StatusCode + " New Access Token: " + response);
                AuthUser authUser = await AuthHelper.CreateAuthUser(response, false);
                bool isSaved = await AuthHelper.SaveAuthData(authUser);
                if (isSaved)
                {
                    Debug.WriteLine("[DataService.RefreshAccessToken]\t" + "New Access Token stored!");
                    return true;
                }
            }
            else
            {
                Debug.WriteLine("[DataService.RefreshAccessToken]\t" + httpResponseMessage.StatusCode + " Error New Access Token: " + response);
                throw new ApiException(response);
            }

            return false;
        }
    }
}

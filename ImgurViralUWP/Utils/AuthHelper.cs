using ImgurViralUWP.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace ImgurViralUWP.Utils
{
    class AuthHelper
    {
        /// <summary>
        /// Crea un oggetto AuthUser partendo da una QueryString o una struttura di dati in Json.
        /// </summary>
        /// <param name="raw"></param>
        /// <param name="isQueryString">True se raw è una querystring, False se è una stringa json</param>
        /// <returns></returns>
        public async static Task<AuthUser> CreateAuthUser(String raw, Boolean isQueryString) {
            Debug.WriteLine("[AuthHelper.CreateAuthUser]\t" + "RAW: " + raw);
            AuthUser authUser = new AuthUser();
            if (isQueryString)
            {
                int indexOfSharp = raw.IndexOf("#");
                String query = raw.Substring(indexOfSharp + 1, raw.Length - indexOfSharp - 1);

                authUser.AccessToken = query.Split('&')
                    .Where(s => s.Split('=')[0] == Constants.AUTH_ACCESS_TOKEN)
                    .Select(s => s.Split('=')[1])
                    .FirstOrDefault();

                authUser.RefreshToken = query.Split('&')
                    .Where(s => s.Split('=')[0] == Constants.AUTH_REFRESH)
                    .Select(s => s.Split('=')[1])
                    .FirstOrDefault();

                authUser.Username = query.Split('&')
                    .Where(s => s.Split('=')[0] == Constants.AUTH_ACCOUNT_USERNAME)
                    .Select(s => s.Split('=')[1])
                    .FirstOrDefault();

                authUser.AccountId = query.Split('&')
                    .Where(s => s.Split('=')[0] == Constants.AUTH_ACCOUNT_ID)
                    .Select(s => s.Split('=')[1])
                    .FirstOrDefault();

                authUser.ExpiresToken = query.Split('&')
                    .Where(s => s.Split('=')[0] == Constants.AUTH_EXPIRES)
                    .Select(s => s.Split('=')[1])
                    .FirstOrDefault();

                authUser.TypeToken = query.Split('&')
                    .Where(s => s.Split('=')[0] == Constants.AUTH_TYPE)
                    .Select(s => s.Split('=')[1])
                    .FirstOrDefault();
            }
            else
            {
                authUser = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<AuthUser>(raw));
                //Debug.WriteLine("[AuthHelper.CreateAuthUser]\t" +
                //    "JSON=[" + 
                //    "Username=" + authUser.Username + ", " +
                //    "AccessToken=" + authUser.AccessToken + ", " +
                //    "ExpiresToken=" + authUser.ExpiresToken + ", " +
                //    "RefreshToken=" + authUser.RefreshToken + ", " +
                //    "AccountID=" +  authUser.AccountId + ", " +
                //    "TypeToken=" + authUser.TypeToken + 
                //    "]");
            }

            return authUser;
        }

        /// <summary>
        /// Salva un oggetto AuthUser in un file locale.
        /// </summary>
        /// <param name="user">AuthUser</param>
        /// <returns>Boolean True/False se il salvataggio è andato a buon fine o meno.</returns>
        public async static Task<bool> SaveAuthData(AuthUser user)
        {
            String authUserToJson = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(user, Formatting.Indented));
            Debug.WriteLine("[AuthHelper.SaveAuthData]\t" + authUserToJson);

            StorageFolder sFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sFile = await sFolder.CreateFileAsync(Constants.AUTH_LOCALSETTINGS_FILENAME, CreationCollisionOption.ReplaceExisting);

            // WRITE
            using (var sWriter = new StreamWriter(await sFile.OpenStreamForWriteAsync()))
            {
                await sWriter.WriteAsync(authUserToJson);
                await sWriter.FlushAsync();
            }

            // READ
            String sFileContent;
            using (var sReader = new StreamReader(await sFile.OpenStreamForReadAsync()))
            {
                sFileContent = await sReader.ReadToEndAsync();
            }

            if (String.IsNullOrEmpty(sFileContent))
            {
                return false;
            }

            //if (sFileContent != null)
            //{
            //    AuthUser jsonToAuthUser = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<AuthUser>(sFileContent));
            //    Debug.WriteLine("[AUTHHELPER]\t" +
            //        "JSON=[" + "Username=" + jsonToAuthUser.Username + ", " +
            //        "AccessToken=" + jsonToAuthUser.AccessToken + ", " +
            //        "ExpiresToken=" + jsonToAuthUser.ExpiresToken + ", " +
            //        "RefreshToken=" + jsonToAuthUser.RefreshToken + ", " +
            //        "TypeToken=" + jsonToAuthUser.TypeToken + "]");
            //}
            //else
            //{
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// Restituisce un oggetto AuthUser partendo da un file locale.
        /// </summary>
        /// <returns>AuthUser</returns>
        public async static Task<AuthUser> ReadAuthData()
        {
            StorageFolder sFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sFile = await sFolder.GetFileAsync(Constants.AUTH_LOCALSETTINGS_FILENAME);
            String sFileContent = null;
            AuthUser jsonToAuthUser = null;

            using (var sReader = new StreamReader(await sFile.OpenStreamForReadAsync()))
            {
                sFileContent = await sReader.ReadToEndAsync();
            }

            if (sFileContent != null)
            {
                jsonToAuthUser = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<AuthUser>(sFileContent));
                Debug.WriteLine("[AuthHelper.ReadAuthData]\t" + jsonToAuthUser.Username);
            }

            return jsonToAuthUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async static Task DeleteAuthData()
        {
            StorageFolder sFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sFile = await sFolder.GetFileAsync(Constants.AUTH_LOCALSETTINGS_FILENAME);
            await sFile.DeleteAsync();
            await Windows.Storage.ApplicationData.Current.ClearAsync(ApplicationDataLocality.Temporary);
            await Windows.Storage.ApplicationData.Current.ClearAsync(ApplicationDataLocality.Local);
            await Windows.Storage.ApplicationData.Current.ClearAsync(ApplicationDataLocality.LocalCache);
            HttpBaseProtocolFilter httpBaseProtoFilter = new HttpBaseProtocolFilter();
            try
            {
                foreach (HttpCookie cookie in httpBaseProtoFilter.CookieManager.GetCookies(new Uri(Constants.ENDPOINT_IMGUR_BASE)))
                {
                    Debug.WriteLine("[AuthHelper.DeleteAuthData]\t" + "Cookie=[" + cookie.Domain.ToString() + ", " + cookie.Name + "]");
                    httpBaseProtoFilter.CookieManager.DeleteCookie(cookie);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("[AuthHelper.DeleteAuthData]\t" + "Exception!");
            }
            Debug.WriteLine("[AuthHelper.DeleteAuthData]\t" + "Deleted!");
        }
    }
}

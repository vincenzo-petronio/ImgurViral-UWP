using ImgurViralUWP.Models;
using ImgurViralUWP.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

// Il modello di elemento per la pagina vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=234238

namespace ImgurViralUWP.Views
{
    /// <summary>
    /// </summary>
    public sealed partial class AuthView : Page
    {
        public AuthView()
        {
            this.InitializeComponent();
        }

        // WEBVIEW LifeCycle:
        // - NavigationStarting
        // - ContentLoading
        // - NavigationCompleted
        // - NavigationFailed

        private async void WebView_Loaded(object sender, RoutedEventArgs e)
        {
            AuthUser authUser = null;
            try
            {
                Debug.WriteLine("[AuthView.WebView_Loaded]\t" + "Try to load local token");
                authUser = await AuthHelper.ReadAuthData();
                Frame.Navigate(typeof(MainPageView));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[AuthView.WebView_Loaded]\t" + "Catch exception, local token not found. Move to Imgur login page!");
                Debug.WriteLine("[AuthView.WebView_Loaded]\n" + ex.Message);
                // Request: https://api.imgur.com/oauth2/authorize?client_id=CLIENT_ID&response_type=token
                this.webView.Navigate(new Uri(String.Format(Constants.ENDPOINT_API_AUTHORIZE, Constants.API_CLIENTID)));
            }
        }

        private async void webView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            // Response: http://example.com#access_token=ACCESS_TOKEN&token_type=Bearer&expires_in=3600
            string uriToString = args.Uri.ToString();
            Debug.WriteLine("[AuthView.webView_NavigationStarting]\t" + uriToString);
            if (uriToString.Contains(Constants.AUTH_ACCESS_TOKEN))
            {
                AuthUser authUser = await AuthHelper.CreateAuthUser(uriToString, true);
                
                if (await AuthHelper.SaveAuthData(authUser))
                {
                    Frame.Navigate(typeof(MainPageView));
                }
            }
            else
            {
                Debug.WriteLine("[AuthView.webView_NavigationStarting]\t" + "NO ACCESS TOKEN FROM URI!");
            }
        }
    }
}

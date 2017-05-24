using ImgurViralUWP.Models;
using ImgurViralUWP.Utils;
using ImgurViralUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento per la pagina vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=234238

namespace ImgurViralUWP.Views
{
    /// <summary>
    /// Pagina vuota che può essere utilizzata autonomamente oppure esplorata all'interno di un frame.
    /// </summary>
    public sealed partial class MainPageView : Page
    {
        public MainPageView()
        {
            this.InitializeComponent();

            this.Loaded += page_Loaded;
        }

        private void page_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Window_SizeChanged;
        }

        private void Window_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            var visualState = "DefaultLayout";
            var applicationView = ApplicationView.GetForCurrentView();
            if (applicationView.IsFullScreen)
            {
                //if (applicationView.Orientation == ApplicationViewOrientation.Landscape)
                //    visualState = "FullScreenLandscape";
                //else
                //    visualState = "FullScreenPortrait";
            }
            else
            {
                var size = Window.Current.Bounds;
                if (size.Width == 320) {
                    //visualState = "SnappedLayout";
                }
                else if (size.Width <= 500) {
                    visualState = "MinimalLayout";
                }
                //else
                //    visualState = "FilledLayout";
            }
            
            VisualStateManager.GoToState(this, visualState, true);
        }
    }
}

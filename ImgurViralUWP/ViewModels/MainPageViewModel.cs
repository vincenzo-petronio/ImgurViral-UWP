using Caliburn.Micro;
using ImgurViralUWP.Exceptions;
using ImgurViralUWP.Models;
using ImgurViralUWP.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ImgurViralUWP.ViewModels
{
    public class MainPageViewModel : Screen
    {
        private IDataService dataService;
        private INavigationService navigationService;
        private Boolean progressRingIsActive;
        private List<GalleryImageData> items;
        //private List<AlbumImageCommentsData> imageComments;
        private GalleryImageData selectedItem;
        private String itemsCounter;
        private ResourceLoader resourceLoader;
        private Boolean isLogoutVisible;
        //private Boolean isFlipViewEnabled;
        private DataTransferManager dtm;

        public MainPageViewModel(IDataService dataService, INavigationService navigationService)
        {
            this.dataService = dataService;
            this.navigationService = navigationService;
            this.progressRingIsActive = true;
            this.items = new List<GalleryImageData>();
            //this.imageComments = new List<AlbumImageCommentsData>();
            this.resourceLoader = new ResourceLoader();
            this.isLogoutVisible = true;
            //this.isFlipViewEnabled = true;

#if WINDOWS_PHONE_APP
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
#endif

        }

        public MainPageViewModel()
        {
            if (Execute.InDesignMode)
            {
                //this.ImageComments = new List<AlbumImageCommentsData>
                //{
                //    new AlbumImageCommentsData 
                //    {
                //        Author = "Blake",
                //        Comment = "Lorem ipsum dolor sit amet, consectetur adipiscing elit",
                //    },
                //    new AlbumImageCommentsData 
                //    {
                //        Author = "Colin1985imgurOfficial",
                //        Comment = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                //    },
                //    new AlbumImageCommentsData 
                //    {
                //        Author = "Christopher",
                //        Comment = "Lorem ipsum dolor sit amet",
                //    },
                //    new AlbumImageCommentsData 
                //    {
                //        Author = "Justin",
                //        Comment = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor",
                //    },
                //    new AlbumImageCommentsData 
                //    {
                //        Author = "Justin & Christopher",
                //        Comment = "Lorem ipsum",
                //    }
                //};
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            this.dataService.GetGalleryImage(async (gallery, err) => {
                ProgressRingIsActive = false;
                if (err == null)
                {
                    this.Items = gallery;
                    this.dtm = DataTransferManager.GetForCurrentView();
                    this.dtm.DataRequested += ShareHandler;
                }
                else
                {
                    if (err.GetType() == typeof(ApiException))
                    {
                        Debug.WriteLine("[MainPageViewModel.OnActivate]\t" + "ApiException");
                        ApiError apiError = JsonConvert.DeserializeObject<ApiError>(err.Message);

                        var dialog = new MessageDialog(apiError.Status + " - " + apiError.Data.Error + "\n" + resourceLoader.GetString("msg_login_again"));
                        dialog.Commands.Add(new UICommand("OK",
                            new UICommandInvokedHandler(async (s) =>
                            {
                                await AuthHelper.DeleteAuthData();
                                navigationService.GoBack();
                            })));
                        await dialog.ShowAsync();
                    }
                    else if (err.GetType() == typeof(NetworkException))
                    {
                        Debug.WriteLine("[MainPageViewModel.OnActivate]\t" + "NetworkException");
                        var dialog = new MessageDialog(resourceLoader.GetString("msg_connection_error"));
                        dialog.Commands.Add(new UICommand("OK",
                            new UICommandInvokedHandler((s) => { CaliburnApplication.Current.Exit(); })));
                        await dialog.ShowAsync();
                    }
                    else if (err.GetType() == typeof(ArgumentNullException))
                    {
                        Debug.WriteLine("[MainPageViewModel.OnActivate]\t" + "ArgumentNullException");
                    }
                }
            });
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            if (this.dtm != null)
            {
                this.dtm.DataRequested -= ShareHandler;
            }
        }

#if WINDOWS_PHONE_APP
        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            CaliburnApplication.Current.Exit();
        }
#endif

        /// <summary>
        /// In binding con la proprietà Visible della Progress.
        /// </summary>
        public Boolean ProgressRingIsActive
        {
            get
            {
                return progressRingIsActive;
            }

            set
            {
                if (progressRingIsActive != value)
                {
                    progressRingIsActive = value;
                    NotifyOfPropertyChange(() => ProgressRingIsActive);
                }
            }
        }

        /// <summary>
        /// In binding con la proprietà IsEnabled del FlipView
        /// </summary>
        //public Boolean IsFlipViewEnabled
        //{
        //    get
        //    {
        //        return isFlipViewEnabled;
        //    }

        //    set
        //    {
        //        if (value != isFlipViewEnabled)
        //        {
        //            isFlipViewEnabled = value;
        //            NotifyOfPropertyChange(() => IsFlipViewEnabled);
        //        }
        //    }
        //}

        public void ImageIsOpened()
        {
            Debug.WriteLine("IS_IMAGE_OPENED");
            // ProgressRingIsActive = false;
        }

        public void ImageIsFailed()
        {
            Debug.WriteLine("IS_IMAGE_FAILED");
        }

        /// <summary>
        /// In binding con la proprietà Visible del Button Logout.
        /// </summary>
        public Boolean IsLogoutVisible
        {
            get
            {
                return isLogoutVisible;
            }

            set
            {
                if (isLogoutVisible != value)
                {
                    IsLogoutVisible = value;
                    NotifyOfPropertyChange(() => IsLogoutVisible);
                }
            }
        }

        /// <summary>
        /// Collection of items for FlipView
        /// </summary>
        public List<GalleryImageData> Items
        {
            get
            {
                return items;
            }

            set
            {
                if(items != value)
                {
                    items = value;
                    NotifyOfPropertyChange(() => Items);
                }
            }
        }

        /// <summary>
        /// Collection of items for ListView
        /// </summary>
        //public List<AlbumImageCommentsData> ImageComments
        //{
        //    get
        //    {
        //        return imageComments;
        //    }

        //    set
        //    {
        //        if (value != null && imageComments != value)
        //        {
        //            imageComments = value;
        //            NotifyOfPropertyChange(() => ImageComments);
        //        }
        //    }
        //}

        /// <summary>
        /// Selected item from FlipView
        /// </summary>
        public GalleryImageData SelectedItem
        {
            get
            {
                return selectedItem;
            }

            set
            {
                if (selectedItem != value && value != null)
                {
                    selectedItem = value;
                    NotifyOfPropertyChange(() => SelectedItem);
                }
            }
        }

        /// <summary>
        /// Counter for the images of FlipView. 
        /// </summary>
        public String ItemsCounter
        {
            get
            {
                return itemsCounter;
            }

            set
            {
                if (itemsCounter != value)
                {
                    itemsCounter = value;
                    NotifyOfPropertyChange(() => ItemsCounter);
                }
            }
        }

        public async void Logout()
        {
            await AuthHelper.DeleteAuthData();
            this.navigationService.GoBack();
        }

        public void Share()
        {
            Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
        }

        private void ShareHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            e.Request.Data.Properties.Title = resourceLoader.GetString("AppTitle/Text");
            e.Request.Data.SetText(SelectedItem.Title + "\n\n" + SelectedItem.Link);
        }

        //public async void FlipViewSelectionChanged(GalleryImageData item)
        //{
        //    if (item == null) return;

        //    IsFlipViewEnabled = false;
        //    ProgressRingIsActive = true;

        //    await this.dataService.GetAlbumImageComments((comments, err) =>
        //    {
        //        if (err == null)
        //        {
        //            this.ImageComments = comments;
        //        }
        //        else
        //        {
        //            // TODO
        //        }
        //    }, item.Id);

        //    await EnableFlipViewTaskDelay();
        //    ProgressRingIsActive = false;
        //    IsFlipViewEnabled = true;
        //}

        //private async Task EnableFlipViewTaskDelay()
        //{
        //    try
        //    {
        //        await Task.Delay(1000);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("EnableFlipViewTaskDelay", e.Message);
        //    }
        //}

        public void ImageDoubleTapped(ScrollViewer sv)
        {
            if (sv == null) return;

            if (sv.ZoomFactor != 1)
            {
                sv.HorizontalScrollMode = ScrollMode.Disabled;
                sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                sv.MaxZoomFactor = 1;
                #if WINDOWS_APP
                bool isViewChanged = sv.ChangeView(null, null, 1);
                #elif WINDOWS_PHONE_APP
                sv.ZoomToFactor(1);
                #endif
            }
            else if (sv.ZoomFactor == 1)
            {
                var image = (sv.Content as Windows.UI.Xaml.Controls.Image);
                var bitmapImage = (image.Source as BitmapImage).PixelWidth;
                if (bitmapImage > sv.ActualWidth)
                {
                    sv.MaxZoomFactor = 2.0f;
                    sv.HorizontalScrollMode = ScrollMode.Enabled;
                    sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                    #if WINDOWS_APP
                    bool isViewChanged = sv.ChangeView(((bitmapImage * 2.0f) - sv.ActualWidth) / 2, null, sv.MaxZoomFactor);
                    #elif WINDOWS_PHONE_APP
                    sv.ZoomToFactor(2);
                    sv.ScrollToHorizontalOffset(((bitmapImage * 2.0f) - sv.ActualWidth) / 2);
                    #endif
                }
            }
        }
    }
}

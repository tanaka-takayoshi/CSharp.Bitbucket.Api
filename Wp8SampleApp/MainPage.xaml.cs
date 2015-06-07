using AsyncOAuth;
using BitBucket.Api;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Wp8SampleApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        readonly StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        string consumerKey = "consumerKey";
        string consumerKeySecret = "consumerKeySecret";

        // 認証に使う一連のAPIのURL
        string requestTokenUrl = "https://bitbucket.org/!api/1.0/oauth/request_token/";
        string authorizeUrl = "https://bitbucket.org/!api/1.0/oauth/authenticate/";
        string accessTokenUrl = "https://bitbucket.org/!api/1.0/oauth/access_token/";

        // 認証のめんどくさいことをしてくれる人を作る
        readonly OAuthAuthorizer authrizer;
        RequestToken requestToken;
        private AccessToken accessToken;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            authrizer = new OAuthAuthorizer(consumerKey, consumerKeySecret);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.
            try
            {
                var sampleFile = await localFolder.GetFileAsync("AccessToken.json");
                var formatter = new DataContractJsonSerializer(typeof(AccessToken));
                using (var stream = await sampleFile.OpenStreamForReadAsync())
                {
                    var obj = formatter.ReadObject(stream);
                    accessToken = obj as AccessToken;
                }

            }
            catch (Exception)
            {
                //notfound
            }
            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            requestToken = await Util.AuthenticateAndContinue(consumerKey,
                consumerKeySecret,
                requestTokenUrl,
                authorizeUrl);
        }

        internal async void ContinueWebAuthentication(WebAuthenticationBrokerContinuationEventArgs e)
        {
            // 結果からHttpのパラメータを抜き取る
            accessToken = await Util.ContinueAuthentication(e.WebAuthenticationResult.ResponseData,
                authrizer, accessTokenUrl, requestToken);
            var formatter = new DataContractJsonSerializer(typeof (AccessToken));
            var sampleFile = await localFolder.CreateFileAsync("AccessToken.json",
                    CreationCollisionOption.ReplaceExisting);
            using (var stream = await sampleFile.OpenStreamForWriteAsync())
            {
                formatter.WriteObject(stream, accessToken);
            }
            await Exexute();
        }

        private async Task Exexute()
        {
            var bitBucket = new BitBucketClient(consumerKey, consumerKeySecret, accessToken);
            var repos = await bitBucket.GetRepositories();
            Debug.WriteLine(repos);
        }
    }
}

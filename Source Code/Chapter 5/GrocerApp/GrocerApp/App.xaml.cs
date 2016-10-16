using GrocerApp.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GrocerApp {

    sealed partial class App : Application {
        private ViewModel viewModel;
        private Task locationTask;
        private CancellationTokenSource locationTokenSource;

        public App() {
            this.InitializeComponent();

            viewModel = new ViewModel();

            viewModel.StoreList.Add("Whole Foods");
            viewModel.StoreList.Add("Kroger");
            viewModel.StoreList.Add("Costco");
            viewModel.StoreList.Add("Walmart");

            viewModel.GroceryList.Add(new GroceryItem {
                Name = "Apples",
                Quantity = 4, Store = "Whole Foods"
            });
            viewModel.GroceryList.Add(new GroceryItem {
                Name = "Hotdogs",
                Quantity = 12, Store = "Costco"
            });
            viewModel.GroceryList.Add(new GroceryItem {
                Name = "Soda",
                Quantity = 2, Store = "Costco"
            });
            viewModel.GroceryList.Add(new GroceryItem {
                Name = "Eggs",
                Quantity = 12, Store = "Kroger"
            });

            this.Suspending += OnSuspending;
            this.Resuming += OnResuming;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args) {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null) {
                rootFrame = new Frame();
                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated) {
                    //TODO: Load state from previously suspended application
                }
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null) {

                if (!rootFrame.Navigate(typeof(Pages.MainPage), viewModel)) {
                    throw new Exception("Failed to create initial page");
                }
            }
            Window.Current.Activate();
            StartLocationTracking(rootFrame);
        }

        protected override void OnSearchActivated(SearchActivatedEventArgs args) {
            viewModel.SearchAndSelect(args.QueryText);
        }

        private void OnResuming(object sender, object e) {
            viewModel.Location = "Unknown";
            StartLocationTracking(Window.Current.Content as Frame);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e) {
            StopLocationTracking();
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
            locationTask.Wait();
            deferral.Complete();
        }

        private void StartLocationTracking(Frame frame) {
            locationTokenSource = new CancellationTokenSource();
            CancellationToken token = locationTokenSource.Token;

            locationTask = new Task(async () => {
                while (!token.IsCancellationRequested) {
                    await frame.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        async () => {
                            viewModel.Location = await Location.TrackLocation();
                        });
                    token.WaitHandle.WaitOne(5000);
                }
            });
            locationTask.Start();
        }

        private void StopLocationTracking() {
            locationTokenSource.Cancel();
        }
    }
}

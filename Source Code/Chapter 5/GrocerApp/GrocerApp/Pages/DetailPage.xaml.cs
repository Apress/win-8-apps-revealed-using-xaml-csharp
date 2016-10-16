using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GrocerApp.Pages {
    public sealed partial class DetailPage : Page {

        public DetailPage() {
            this.InitializeComponent();

            SizeChanged += DetailPage_SizeChanged;
        }

        void DetailPage_SizeChanged(object sender, SizeChangedEventArgs e) {
            //if (ApplicationView.Value == ApplicationViewState.Snapped) {
            //    GridLayout.ColumnDefinitions[0].Width
            //        = new GridLength(0);
            //} else {
            //    GridLayout.ColumnDefinitions[0].Width
            //        = new GridLength(1, GridUnitType.Star);
            //}            

            string stateName = ApplicationView.Value ==
                ApplicationViewState.Snapped ? "Snapped" : "Others";
            VisualStateManager.GoToState(this, stateName, false);

        }

        private void HandleButtonClick(object sender, RoutedEventArgs e) {
            Windows.UI.ViewManagement.ApplicationView.TryUnsnap();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
        }
    }
}

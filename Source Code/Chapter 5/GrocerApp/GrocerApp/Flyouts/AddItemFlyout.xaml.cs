using System;
using GrocerApp.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GrocerApp.Flyouts {
    public sealed partial class AddItemFlyout : UserControl {

        public AddItemFlyout() {
            this.InitializeComponent();
        }

        public void Show(Page page, AppBar appbar, Button button) {
            AddItemPopup.IsOpen = true;
            FlyoutHelper.ShowRelativeToAppBar(AddItemPopup, page, appbar, button);
        }

        private void AddButtonClick(object sender, RoutedEventArgs e) {

            ((ViewModel)DataContext).GroceryList.Add(new GroceryItem {
                Name = ItemName.Text,
                Quantity = Int32.Parse(ItemQuantity.Text),
                Store = ItemStore.SelectedItem.ToString()
            });

            AddItemPopup.IsOpen = false;
        }
    }
}

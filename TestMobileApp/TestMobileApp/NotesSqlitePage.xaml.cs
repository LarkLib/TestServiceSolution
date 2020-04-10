using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotesSqlitePage : ContentPage
    {
        public NotesSqlitePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            listView.ItemsSource = await App.Database.GetNotesAsync();
        }

        async void OnNoteAddedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NoteEntrySqlitePage
            {
                BindingContext = new NoteSqlite()
            });
        }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new NoteEntrySqlitePage
                {
                    BindingContext = e.SelectedItem as NoteSqlite
                });
            }
        }

    }
}
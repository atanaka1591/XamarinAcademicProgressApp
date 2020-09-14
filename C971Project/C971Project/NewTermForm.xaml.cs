using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971Project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTermForm : ContentPage
    {
        public NewTermForm()
        {
            InitializeComponent();
        }

        //submit button to add new term
        async private void Button_Clicked(object sender, EventArgs e)
        {
            //checks that all start dates are before the end date
            if (startdateEntry.Date > enddateEntry.Date)
            {
                await DisplayAlert("Warning", "Start date must be before the end date.", "OK");
            }
            else if (string.IsNullOrWhiteSpace(titleEntry.Text))
            {
                await DisplayAlert("Warning", "All fields must be filled out.", "OK");
            }
            else
            {
                Term.Status status = Term.Status.Upcoming;

                if (statusEntry.SelectedIndex == 0)
                {
                    status = Term.Status.Upcoming;
                }
                else if (statusEntry.SelectedIndex == 1)
                {
                    status = Term.Status.InProgress;
                }
                else if (statusEntry.SelectedIndex == 2)
                {
                    status = Term.Status.Completed;
                }

                var term = new Term()
                {
                    Title = titleEntry.Text,
                    TermStatus = status,
                    TermStart = startdateEntry.Date,
                    TermEnd = enddateEntry.Date
                };

                var connection = new SQLiteAsyncConnection(TermsMainPage.path);
                await connection.InsertAsync(term);

                await DisplayAlert("Alert", "New term has been added.", "OK");

                await Navigation.PopAsync();
            }              
        }
    }
}
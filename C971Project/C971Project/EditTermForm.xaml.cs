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

    public partial class EditTermForm : ContentPage
    {
        Term selectedTerm;

        public EditTermForm(Term term)
        {
            InitializeComponent();

            selectedTerm = term;

            if (term.TermStatus == Term.Status.InProgress)
            {
                statusEntry.SelectedIndex = 1;
            }
            else if (term.TermStatus == Term.Status.Completed)
            {
                statusEntry.SelectedIndex = 2;
            }
            else if (term.TermStatus == Term.Status.Upcoming)
            {
                statusEntry.SelectedIndex = 0;
            }

            titleEntry.Text = term.Title;
            startdateEntry.Date = term.TermStart;
            enddateEntry.Date = term.TermEnd;
        }

        //Update Term button
        async private void Button_Clicked(object sender, EventArgs e)
        {
            //checks that all start dates are before the end date and checks null fields
            if (startdateEntry.Date > enddateEntry.Date)
            {
                await DisplayAlert("Warning", "Start date must be before the end date.", "OK");
            }
            else if
                (string.IsNullOrWhiteSpace(titleEntry.Text))
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

                //sets the selected term as an object and makes any changes from the form
                var term = selectedTerm;
                term.Title = titleEntry.Text;
                term.TermStatus = status;
                term.TermStart = startdateEntry.Date;
                term.TermEnd = enddateEntry.Date;

                var connection = new SQLiteAsyncConnection(TermsMainPage.path);

                await connection.UpdateAsync(term);

                await DisplayAlert("Alert", "Term has been updated.", "OK");

                await Navigation.PopAsync();
            }                
        }
    }
}
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
    public partial class NewCourseForm : ContentPage
    {
        Term selectedTerm;

        public NewCourseForm(Term term)
        {
            selectedTerm = term;
            InitializeComponent();
        }

        //method to return the correct status
        Course.Status GetStatus(Picker fieldName)
        {
            if (fieldName.SelectedIndex == 0)
            {
                return Course.Status.Upcoming;
            }
            else if (fieldName.SelectedIndex == 1)
            {
                return Course.Status.InProgress;
            }
            else if (fieldName.SelectedIndex == 2)
            {
                return Course.Status.Completed;
            }
            else
            {
                return Course.Status.Upcoming;
            }
        }

        //submit button to create new course
        async private void Button_Clicked(object sender, EventArgs e)
        {

            string instructorPhone = "";
            try
            {
                instructorPhone = instructorphoneEntry.Text.ToString();
            }
            catch (Exception)
            {
                instructorPhone = "";
            }

            //checks that all start dates are before the end date
            if (startdateEntry.Date > enddateEntry.Date || OAstartEntry.Date > OAendEntry.Date || PAstartEntry.Date > PAendEntry.Date)
            {
                await DisplayAlert("Warning", "Start date must be before the end date.", "OK");
            }
            else if
                (
                string.IsNullOrWhiteSpace(titleEntry.Text) ||
                string.IsNullOrWhiteSpace(instructorEntry.Text) ||
                string.IsNullOrWhiteSpace(instructoremailEntry.Text) ||
                string.IsNullOrWhiteSpace(OAEntry.Text) ||
                string.IsNullOrWhiteSpace(PAEntry.Text)
                )
            {
                await DisplayAlert("Warning", "All non-optional fields must be filled out.", "OK");
            }
            else
            {
                var course = new Course()
                {
                    TermId = selectedTerm.Id,
                    Name = titleEntry.Text,
                    StartDate = startdateEntry.Date,
                    EndDate = enddateEntry.Date,
                    CurrentStatus = GetStatus(statusEntry),
                    InstructorName = instructorEntry.Text,
                    InstructorPhone = instructorPhone,
                    InstructorEmail = instructoremailEntry.Text,
                    OAName = OAEntry.Text,
                    OAStart = OAstartEntry.Date,
                    OAEnd = OAendEntry.Date,
                    OAStatus = GetStatus(OAstatusEntry),
                    OANotes = OAnotesEntry.Text,
                    PAName = PAEntry.Text,
                    PAStart = PAstartEntry.Date,
                    PAEnd = PAendEntry.Date,
                    PAStatus = GetStatus(PAstatusEntry),
                    PANotes = PAnotesEntry.Text
                };

                var connection = new SQLiteAsyncConnection(TermsMainPage.path);
                await connection.InsertAsync(course);

                await DisplayAlert("Alert", "New course has been added.", "OK");

                await Navigation.PopAsync();
            }                  
        }
    }
}
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
    public partial class EditCourseForm : ContentPage
    {
        Course selectedCourse;
        public EditCourseForm(Course course)
        {
            InitializeComponent();

            selectedCourse = course;

            titleEntry.Text = course.Name;
            statusEntry.SelectedIndex = GetStatusIndex(course.CurrentStatus);
            startEntry.Date = course.StartDate;
            endEntry.Date = course.EndDate;
            instructorEntry.Text = course.InstructorName;
            instructorphoneEntry.Text = course.InstructorPhone;
            instructoremailEntry.Text = course.InstructorEmail;
            OAentry.Text = course.OAName;
            OAstartEntry.Date = Convert.ToDateTime(course.OAStart);
            OAendEntry.Date = course.OAEnd;
            OAstatusEntry.SelectedIndex = GetStatusIndex(course.OAStatus);
            OAnotesEntry.Text = course.OANotes;
            PAentry.Text = course.PAName;
            PAstartEntry.Date = course.PAStart;
            PAendEntry.Date = course.PAEnd;
            PAstatusEntry.SelectedIndex = GetStatusIndex(course.PAStatus);
            PAnotesEntry.Text = course.PANotes;
        }

        //translates the status in the object to the index for the picker
        int GetStatusIndex (Course.Status status)
        {
            if (status == Course.Status.InProgress)
            {
                return 1;
            }
            else if (status == Course.Status.Upcoming)
            {
                return 0;
            }
            else if (status == Course.Status.Completed)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
        
        //translates the index of the status picker to the correct status enum
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

        //Update Course button
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
            if (startEntry.Date > endEntry.Date || OAstartEntry.Date > OAendEntry.Date || PAstartEntry.Date > PAendEntry.Date)
            {
                await DisplayAlert("Warning", "Start date must be before the end date.", "OK");
            }
            else if
                (
                string.IsNullOrWhiteSpace(titleEntry.Text) ||
                string.IsNullOrWhiteSpace(instructorEntry.Text) ||
                string.IsNullOrWhiteSpace(instructoremailEntry.Text) ||
                string.IsNullOrWhiteSpace(OAentry.Text) ||
                string.IsNullOrWhiteSpace(PAentry.Text)
                )
            {
                await DisplayAlert("Warning", "All non-optional fields must be filled out.", "OK");
            }
            else
            {
                //sets the selected course as an object and makes any changes from the form
                var course = selectedCourse;
                course.Name = titleEntry.Text;
                course.StartDate = startEntry.Date;
                course.EndDate = endEntry.Date;
                course.CurrentStatus = GetStatus(statusEntry);
                course.InstructorName = instructorEntry.Text;
                course.InstructorPhone = instructorphoneEntry.Text;
                course.InstructorEmail = instructoremailEntry.Text;
                course.OAName = OAentry.Text;
                course.OAStart = OAstartEntry.Date;
                course.OAEnd = OAendEntry.Date;
                course.OAStatus = GetStatus(OAstatusEntry);
                course.OANotes = OAnotesEntry.Text;
                course.PAName = PAentry.Text;
                course.PAStart = PAstartEntry.Date;
                course.PAEnd = PAendEntry.Date;
                course.PAStatus = GetStatus(PAstatusEntry);
                course.PANotes = PAnotesEntry.Text;

                var connection = new SQLiteAsyncConnection(TermsMainPage.path);

                await connection.UpdateAsync(course);

                await DisplayAlert("Alert", "Course has been updated.", "OK");

                await Navigation.PopAsync();
            }


                
        }
    }
}
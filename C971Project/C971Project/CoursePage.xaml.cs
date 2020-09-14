using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Plugin.LocalNotifications;

namespace C971Project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoursePage : ContentPage
    {
        Course selectedCourse;
        public CoursePage(Course course)
        {
            selectedCourse = course;

            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            //queries the db and uses that object as the binding context so that latest data will display
            var connection = new SQLiteAsyncConnection(TermsMainPage.path);
            var course = await connection.Table<Course>().Where(c => c.Id == selectedCourse.Id).FirstAsync();

            BindingContext = course;

            //color codes the statuses
            if (course.CurrentStatus == Course.Status.Completed)
            {
                CourseStatus.BackgroundColor = Color.Lime;
            }
            if (course.CurrentStatus == Course.Status.InProgress)
            {
                CourseStatus.BackgroundColor = Color.Yellow;
            }
            if (course.CurrentStatus == Course.Status.Upcoming)
            {
                CourseStatus.BackgroundColor = Color.LightGray;
            }
            if (course.OAStatus == Course.Status.Completed)
            {
                OAStatus.BackgroundColor = Color.Lime;
            }
            if (course.OAStatus == Course.Status.InProgress)
            {
                OAStatus.BackgroundColor = Color.Yellow;
            }
            if (course.OAStatus == Course.Status.Upcoming)
            {
                OAStatus.BackgroundColor = Color.LightGray;
            }
            if (course.PAStatus == Course.Status.Completed)
            {
                PAStatus.BackgroundColor = Color.Lime;
            }
            if (course.PAStatus == Course.Status.InProgress)
            {
                PAStatus.BackgroundColor = Color.Yellow;
            }
            if (course.PAStatus == Course.Status.Upcoming)
            {
                PAStatus.BackgroundColor = Color.LightGray;
            }
        }

        //Edit Course button
        async private void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditCourseForm(selectedCourse));
        }

        //Delete Course button
        async private void Button_Clicked_DeleteCourse(object sender, EventArgs e)
        {
            var response = await DisplayAlert("Warning", "Are you sure you want to delete this course?", "Yes", "No");
            if (response)
            {
                var connection = new SQLiteAsyncConnection(TermsMainPage.path);
                await connection.DeleteAsync(selectedCourse); 
                await DisplayAlert("Done", "Course has been deleted.", "OK");
                await Navigation.PopAsync();
            }
        }

        //Delete OA button
        async private void Button_Clicked_DeleteOA (object sender, EventArgs e)
        {
            var response = await DisplayAlert("Warning", "Are you sure you want to delete this Objective Assessment?", "Yes", "No");
            if (response)
            {
                var course = selectedCourse;               
                course.OAName = null;
                course.OAStart = new DateTime(2020, 1, 1);
                course.OAEnd = new DateTime(2020, 1, 1);
                course.OAStatus = Course.Status.None;
                course.OANotes = null;

                //course.PAName = PAentry.Text;
                //course.PAStart = PAstartEntry.Date;
                //course.PAEnd = PAendEntry.Date;
                //course.PAStatus = GetStatus(PAstatusEntry);
                //course.PANotes = PAnotesEntry.Text;


                var connection = new SQLiteAsyncConnection(TermsMainPage.path);
                await connection.UpdateAsync(course);

                //queries db for latest data and binds it so it will display
                var courseData = await connection.Table<Course>().Where(c => c.Id == selectedCourse.Id).FirstAsync();
                BindingContext = courseData;

                if (course.OAStatus == Course.Status.Completed)
                {
                    OAStatus.BackgroundColor = Color.Lime;
                }
                if (course.OAStatus == Course.Status.InProgress)
                {
                    OAStatus.BackgroundColor = Color.Yellow;
                }
                if (course.OAStatus == Course.Status.Upcoming)
                {
                    OAStatus.BackgroundColor = Color.LightGray;
                }
                if (course.OAStatus == Course.Status.None)
                {
                    OAStatus.BackgroundColor = Color.Transparent;
                }

                await DisplayAlert("Done", "Objective Assessment data has been deleted.", "OK");
            }
        }

        //Delete PA button
        async private void Button_Clicked_DeletePA(object sender, EventArgs e)
        {
            var response = await DisplayAlert("Warning", "Are you sure you want to delete this Performance Assessment?", "Yes", "No");
            if (response)
            {
                var course = selectedCourse;
                course.PAName = null;
                course.PAStart = new DateTime(2020, 1, 1);
                course.PAEnd = new DateTime(2020, 1, 1);
                course.PAStatus = Course.Status.None;
                course.PANotes = null;

                var connection = new SQLiteAsyncConnection(TermsMainPage.path);
                await connection.UpdateAsync(course);

                //queries db for latest data and binds it so it will display
                var courseData = await connection.Table<Course>().Where(c => c.Id == selectedCourse.Id).FirstAsync();
                BindingContext = courseData;

                if (course.PAStatus == Course.Status.Completed)
                {
                    PAStatus.BackgroundColor = Color.Lime;
                }
                if (course.PAStatus == Course.Status.InProgress)
                {
                    PAStatus.BackgroundColor = Color.Yellow;
                }
                if (course.PAStatus == Course.Status.Upcoming)
                {
                    PAStatus.BackgroundColor = Color.LightGray;
                }
                if (course.PAStatus == Course.Status.None)
                {
                    PAStatus.BackgroundColor = Color.Transparent;
                }

                await DisplayAlert("Done", "Performance Assessment data has been deleted.", "OK");
            }
        }

        //Share OA Notes button
        async private void Button_Clicked_ShareNotesOA(object sender, EventArgs e)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = "My Objective Assessment Notes",
                    Body = selectedCourse.OANotes
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                await DisplayAlert("Alert", "Email is not supported on this device.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "An error occurred.", "OK");
            }
        }

        //Share PA Notes button
        async private void Button_Clicked_ShareNotesPA(object sender, EventArgs e)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = "My Performance Assessment Notes",
                    Body = selectedCourse.PANotes
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                await DisplayAlert("Alert", "Email is not supported on this device.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "An error occurred.", "OK");
            }
        }

        //OA Set Reminder button
        async private void Button_Clicked_SetReminderOA(object sender, EventArgs e)
        {
            var courseName = selectedCourse.Name;
            CrossLocalNotifications.Current.Show("Reminder: Start Objective Assessment", $"Your Objective Assessment for {courseName} starts today!", selectedCourse.Id, selectedCourse.OAEnd);
            CrossLocalNotifications.Current.Show("Reminder: Finish Objective Assessment", $"Your Objective Assessment for {courseName} ends today!", selectedCourse.Id + 1000, selectedCourse.OAEnd);

            var response = await DisplayAlert("Message","You will receive a notification reminder on the first and last day of your Objective Assessment. To cancel notification, press Cancel Notification.", "OK", "Cancel Notification");
            if (!response)
            {
                CrossLocalNotifications.Current.Cancel(selectedCourse.Id);
                CrossLocalNotifications.Current.Cancel(selectedCourse.Id + 1000);
                await DisplayAlert("Cancelled", "Reminder notification has been cancelled.", "OK");
            }
        }

        //PA Set Reminder button
        async private void Button_Clicked_SetReminderPA(object sender, EventArgs e)
        {
            var courseName = selectedCourse.Name;
            CrossLocalNotifications.Current.Show("Reminder: Start Performance Assessment", $"Your Performance Assessment for {courseName} starts today!", selectedCourse.Id + 2000, selectedCourse.PAEnd);
            CrossLocalNotifications.Current.Show("Reminder: Finish Performance Assessment", $"Your Performance Assessment for {courseName} ends today!", selectedCourse.Id + 3000, selectedCourse.PAEnd);

            var response = await DisplayAlert("Message", "You will receive a notification reminder on the first and last day of your Performance Assessment. To cancel notification, press Cancel Notification.", "OK", "Cancel Notification");
            if (!response)
            {
                CrossLocalNotifications.Current.Cancel(selectedCourse.Id + 2000);
                CrossLocalNotifications.Current.Cancel(selectedCourse.Id + 3000);
                await DisplayAlert("Cancelled", "Reminder notification has been cancelled.", "OK");
            }
        }
    }
}
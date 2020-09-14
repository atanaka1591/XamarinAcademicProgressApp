using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace C971Project
{
    public partial class TermsDetailPage : ContentPage
    {
        Term selectedTerm;
        public TermsDetailPage(Term term)
        {
            InitializeComponent();

            selectedTerm = term;
        }

        async protected override void OnAppearing()
        {
            var connection = new SQLiteAsyncConnection(TermsMainPage.path);

            //only displays courses associated to this term's id
            var courses = await connection.Table<Course>().Where(c => c.TermId == selectedTerm.Id).ToListAsync();
            
            //observable collection allows updated data to display immediately. Needed for change to display without needing to refresh after editing courses.
            //var _courses = new ObservableCollection<Course>(courses);
            listView.ItemsSource = courses;

            //by binding queried data from the db instead of using the term that is passed in, it will display the latest data. for example, after a term is updated.
            var term = await connection.Table<Term>().Where(c => c.Id == selectedTerm.Id).FirstAsync();
            BindingContext = term;
            //not sure why below is not needed.. i thought it would pass old data to term edit form but it doesn't
            //selectedTerm = term;

            base.OnAppearing();
        }

        //select course to open course detail page
        async private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            
            var course = e.SelectedItem as Course;
            await Navigation.PushAsync(new CoursePage(course));
            listView.SelectedItem = null;
        }

        //Edit Term button
        async private void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditTermForm(selectedTerm));
        }

        //Add New Course button - opens add new course form
        async private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            //connects to db and checks if there are already 6 courses associated to this term
            var connection = new SQLiteAsyncConnection(TermsMainPage.path);
            var courses = await connection.Table<Course>().Where(c => c.TermId == selectedTerm.Id).ToListAsync();

            if (courses.Count >=6)
            {
                await DisplayAlert("Warning", "You cannot add more than 6 courses per term.", "OK");
            }
            else
            {
                await Navigation.PushAsync(new NewCourseForm(selectedTerm));
            }
          
        }

        //Delete Term button
        async private void Button2_Clicked(object sender, EventArgs e)
        {
            var response = await DisplayAlert("Warning", "Are you sure you want to delete this term? All courses associated to it will also be deleted.", "Yes", "No");
            if (response)
            {
                var connection = new SQLiteAsyncConnection(TermsMainPage.path);
                await connection.DeleteAsync(selectedTerm);
                await DisplayAlert("Done", "Term has been deleted.", "OK");
                await Navigation.PopAsync();
            }

        }
    }
}

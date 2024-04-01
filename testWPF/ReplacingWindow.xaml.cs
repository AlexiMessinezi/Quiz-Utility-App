using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace testWPF
{
    /// <summary>
    /// Interaction logic for ReplacingWindow.xaml
    /// </summary>
    public partial class ReplacingWindow : Window
    {
        private static Random random = new Random();
        List<String> list = new List<String>();
        private static List<String> Authors;

        public static String getAuthors()
        {
            //Create list of authours
            Authors = new List<String> {"HENRY JAMES","JANE AUSTEN", "CHARLES DICKENS", "VLADIMIR NABOKOV", "MARCEL PROUST","JAMES JOYCE", "GARCIA MARQUEZ, WILLIAM FAULKNER", "VIRGINIA WOOLF", "LEO TOLSTOY",
                                         "GUSTAVE FLAUBERT", "MARK TWAIN", "ANTON CHEKHOV", "GEORGE ELIOT", "HERMAN MELVILLE", "FYODOR DOSTOEVSKY", "FRANZ KAFKA", "SAUL BELLOW","SIDNEY SHELDON","KINGSLEY AMIS"};

            String rndAuthor = Authors.ElementAt(random.Next(Authors.Count));
            String lastName = rndAuthor.Substring(rndAuthor.IndexOf(" ") + 1);
            String firstThree = lastName.Substring(0, 3);

            return firstThree;
        }


        //Bubble sort for Strings
        public static void Sort(List<string> list)
        {
            int size = list.Count();
            String temp;

            for (int j = 0; j < size - 1; j++)
            {
                for (int i = j + 1; i < size; i++)
                {
                    if (list[j].CompareTo(list[i]) > 0)
                    {
                        temp = list[j];
                        list[j] = list[i];
                        list[i] = temp;
                    }
                }
            }
        }



        //Comapre Both Listbox and return a value from 1 - 10
        public int matchSort()
        {
            int score = lstCallNumbersSorted.Items.Cast<string>().Where(t => lstCallNumbers.Items.Cast<string>().ElementAt(lstCallNumbersSorted.Items.Cast<string>().ToList().IndexOf(t)).Equals(t)).ToList().Count;
            return score;
        }
        //Create Random number Method
        public string RandomDigits(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public ReplacingWindow()
        {
            InitializeComponent();


        }
        //On Button click
        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            //Clear Both tables
            lstCallNumbers.Items.Clear();
            lstCallNumbersSorted.Items.Clear();

            // Create Call numbers
            for (int x = 1; x <= 10; x++)
            {
                lstCallNumbers.Items.Add(RandomDigits(3) + "." + RandomDigits(3) + getAuthors() + "\n");
            }


        }
        //On Button click
        private void btnCheckOrder_Click(object sender, RoutedEventArgs e)
        {

            //y equals Highscore
            var y = Convert.ToInt32(this.lblValue.Content);

            //Clear list 
            list.Clear();

            //Populates Listbox
            foreach (String x in lstCallNumbers.Items)
            {
                list.Add(x);
            }
            //Sorts poppulated listbox

            Sort(list);

            //Clears Sorted listbox
            lstCallNumbersSorted.Items.Clear();

            foreach (String x in list)
            {
                lstCallNumbersSorted.Items.Add(x);
            }

            //If Sort method returns 10
            if (Convert.ToInt32(matchSort()) == 10)
            {
                //User Succefully ordered coorectly
                MessageBox.Show("Congratulation Everything was ordered Correctly");
                //if Score is greater than or equal to highscore enter loop
                if (matchSort() >= y)
                {
                    //Highscore equal to new score
                    lblValue.Content = matchSort();
                }
            }
            else
            {
                //If Score does not equal 10
                MessageBox.Show("You got " + Convert.ToString(matchSort()) + "/10 \nBetter Luck Next Time");

                //Highscore label = score
                lblValue.Content = matchSort();


                if (matchSort() >= y)
                {
                    lblValue.Content = matchSort();

                }
                //If Score is not greater than highscore
                else
                {
                    //Assign a temporary value
                    int temp = 0;
                    //Temp value is equal to highscore
                    temp = y;
                    //Label changed to Highscore
                    lblValue.Content = temp;


                }
            }



        }

        //On Button Click
        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            //If no item is selected
            if (lstCallNumbers.SelectedIndex == -1)
            {
                //Print
                MessageBox.Show("Select an item to move", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //If items is selected
            else
            {
                //Create new index
                int newIndex = lstCallNumbers.SelectedIndex - 1;

                //If new index smaller than 0 terminate and return to caller
                if (newIndex < 0)

                    return;


                object selecteditem = lstCallNumbers.SelectedItem;
                //Remove selected item
                lstCallNumbers.Items.Remove(selecteditem);
                //Replace selected item in new index
                lstCallNumbers.Items.Insert(newIndex, selecteditem);
                //Keep Current Item selected
                lstCallNumbers.SelectedIndex = newIndex;
            }
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            if (lstCallNumbers.SelectedIndex == -1)
            {
                MessageBox.Show("Select an item to move", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                int newIndex = lstCallNumbers.SelectedIndex + 1;

                if (newIndex >= lstCallNumbers.Items.Count)

                    return;


                object selecteditem = lstCallNumbers.SelectedItem;

                lstCallNumbers.Items.Remove(selecteditem);

                lstCallNumbers.Items.Insert(newIndex, selecteditem);

                lstCallNumbers.SelectedIndex = newIndex;


            }
        }
        //On button click
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            //Exit application
            this.Close();
        }
        //On Button click
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            //Create object to main window
            MainWindow mwOBJ = new MainWindow();
            //Close current window
            this.Close();
            //Open main Window
            mwOBJ.Show();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            String message = "Step 1:\tThere will be 2 columns.One will provide you with" +
                             " \t\tcall numbers.\nStep 2:\tYou will " +
                             "be able to use the Move up and Move down \tbutton to arrange the values in " +
                             "ascending order.\nStep 3:\tYou can then check your answers by clicking check \t\t" +
                             "order button.\nOnce done you may exit the applciation, go to the main menu screen or retry with a new set of random callnumbers.";
            MessageBox.Show(message, "Help Screen", MessageBoxButton.OK, MessageBoxImage.Information);
            
        }
    }
}

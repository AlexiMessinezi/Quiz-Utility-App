using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace testWPF
{
    /// <summary>
    /// Interaction logic for IdentifyingAreas.xaml
    /// </summary>
    public partial class IdentifyingAreas : Window
    {

        IDictionary<String, String> questionSet = new Dictionary<String, String>(); //Creating Dictionary
        Random rnd = new Random(); //Calling random class

        //Creating variables
        int score = 0;
        static int arraySize = 7;
        string[] questions = new string[arraySize];
        string[] memoAnswers = new string[arraySize];
        string[] userAnswers = new string[arraySize];
        string[] displayAnswers = new string[arraySize];
        public IdentifyingAreas()
        {
            InitializeComponent();
            //Adding keys and values to the dicctionary
            questionSet.Add("000", "book about general knowledge"); // General Knowledge
            questionSet.Add("100", "book about psychology");        // Psychology and Philosophy
            questionSet.Add("200", "book about religion");          // Religions and Mythology
            questionSet.Add("300", "book about social science");    // Social Sciences and Folklore
            questionSet.Add("400", "book about languages");         // Languages and Grammar
            questionSet.Add("500", "book about maths");             // Math and Science
            questionSet.Add("600", "book about medicine");          // Medicine and Technology
            questionSet.Add("700", "book about arts");              // Arts and Recreation
            questionSet.Add("800", "book about literature");        // Literature
            questionSet.Add("900", "book about geography");         // Geography and History

            //Calling method createQuestion
            createQuestion();
        }

        //Method that calls all other methods
        public void createQuestion()
        {
            createQuestionAnswerSet();
            fillQuestions();
            fillAnswers();
        }

        //Method that shuffles the question sets to produce random values in the match the columns tables
        public void shuffleQuestionSet()
        {
            //Shuffling the Dictionary so random question will show. Using the random feature.
            questionSet = questionSet.OrderBy(x => rnd.Next()).ToDictionary(item => item.Key, item => item.Value);
        }


        public void createQuestionAnswerSet()
        {
            shuffleQuestionSet();
            for (int i = 0; i < arraySize; i++)
            {
                if (rnd.Next(2) == 0) //If it returns 0 the key will be used as the hint and the value will be used as a possible answer
                {
                    questions[i] = questionSet.ElementAt(i).Key.ToString();
                    memoAnswers[i] = questionSet.ElementAt(i).Value.ToString();
                    displayAnswers[i] = questionSet.ElementAt(i).Value.ToString();
                    continue;
                }
                //If it gets here it would have returned 1. The value will be used as the hint and the key will be used as a possible answer.
                questions[i] = questionSet.ElementAt(i).Value.ToString();
                memoAnswers[i] = questionSet.ElementAt(i).Key.ToString();
                displayAnswers[i] = questionSet.ElementAt(i).Key.ToString();
            }
        }
        //Method that fills in the questions table
        public void fillQuestions()
        {
            lblHint1.Content = createQuestionString("A", 0, questions);
            lblHint2.Content = createQuestionString("B", 1, questions);
            lblHint3.Content = createQuestionString("C", 2, questions);
            lblHint4.Content = createQuestionString("D", 3, questions);
        }

        //Method that fills in the Answers table
        public void fillAnswers()
        {
            displayAnswers = displayAnswers.OrderBy(x => rnd.Next()).ToArray();
            lblAns1.Content = createAnswerString(0, displayAnswers);
            lblAns2.Content = createAnswerString(1, displayAnswers);
            lblAns3.Content = createAnswerString(2, displayAnswers);
            lblAns4.Content = createAnswerString(3, displayAnswers);
            lblAns5.Content = createAnswerString(4, displayAnswers);
            lblAns6.Content = createAnswerString(5, displayAnswers);
            lblAns7.Content = createAnswerString(6, displayAnswers);

        }

        //Method that creates the string given before the question
        public String createQuestionString(String questionLetter, int questionIndex, string[] whichArray)
        {
            String temp = "";
            temp += questionLetter;
            temp += ": ";
            temp += whichArray[questionIndex];
            return temp;
        }

        //Method that creates the string before the answer
        public String createAnswerString(int questionNum, string[] whichArray)
        {
            String temp = "";
            temp += (questionNum + 1).ToString();
            temp += ": ";
            temp += whichArray[questionNum];
            return temp;
        }
        //Method that will give a whole new question set
        private void btnNextQuestion_Click(object sender, RoutedEventArgs e)
        {
            resetDropdowns();
            createQuestion();
            clearFontColours();
        }

        //Method that resets the Dropdown boxes
        public void resetDropdowns()
        {
            cmbAns1.SelectedIndex = -1;
            cmbAns2.SelectedIndex = -1;
            cmbAns3.SelectedIndex = -1;
            cmbAns4.SelectedIndex = -1;
        }
        //Method that will reset dropdowns, questions, score and clear font colours
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            resetDropdowns();
            createQuestion();
            score = 0;
            lblScore.Content = score;
            clearFontColours();
        }

        //Method that is used to check if the users answer matches the correct answer
        private void btnCheckAns_Click(object sender, RoutedEventArgs e)
        {
            retrieveAnswers();
            // Question 1
            if (userAnswers[0] == memoAnswers[0]) //If the users answer matches the memos answer
            {
                score += 25;//Add 25 to his score 
                lblHint1.Foreground = new SolidColorBrush(Colors.Green);    //Set text to green
            }
            else // If user answers dont match 
            {
                lblHint1.Foreground = new SolidColorBrush(Colors.Red);      //Set text to red
            }
            // Question 2
            if (userAnswers[1] == memoAnswers[1])
            {
                score += 25;
                lblHint2.Foreground = new SolidColorBrush(Colors.Green);    //Set text to green
            }
            else
            {
                lblHint2.Foreground = new SolidColorBrush(Colors.Red);      //Set text to red
            }
            // Question 3
            if (userAnswers[2] == memoAnswers[2])
            {
                score += 25;
                lblHint3.Foreground = new SolidColorBrush(Colors.Green);    //Set text to green
            }
            else
            {
                lblHint3.Foreground = new SolidColorBrush(Colors.Red);      //Set text to red
            }
            // Question 4
            if (userAnswers[3] == memoAnswers[3])
            {
                score += 25;
                lblHint4.Foreground = new SolidColorBrush(Colors.Green);    //Set text to green
            }
            else
            {
                lblHint4.Foreground = new SolidColorBrush(Colors.Red);      //Set text to red
            }
            lblScore.Content = score;                                       //Update overall score

        }

        // Method that retrieves the answer set from the combo box
        public void retrieveAnswers()
        {
            if (cmbAns1.SelectedIndex != -1) userAnswers[0] = displayAnswers[Convert.ToInt32(cmbAns1.Text) - 1];
            if (cmbAns2.SelectedIndex != -1) userAnswers[1] = displayAnswers[Convert.ToInt32(cmbAns2.Text) - 1];
            if (cmbAns3.SelectedIndex != -1) userAnswers[2] = displayAnswers[Convert.ToInt32(cmbAns3.Text) - 1];
            if (cmbAns4.SelectedIndex != -1) userAnswers[3] = displayAnswers[Convert.ToInt32(cmbAns4.Text) - 1];
        }

        //Method that clears Font colours
        public void clearFontColours()
        {
            lblHint1.Foreground = new SolidColorBrush(Colors.Black);
            lblHint2.Foreground = new SolidColorBrush(Colors.Black);
            lblHint3.Foreground = new SolidColorBrush(Colors.Black);
            lblHint4.Foreground = new SolidColorBrush(Colors.Black);
        }

        //Button that will send the user back to the main menu screen
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mwOBJ = new MainWindow();        //Create object to main window
            this.Close();                               //Close current window
            mwOBJ.Show();                               //Open main Window
        }

        //Button that will entirely close the application
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();                               //Exit application
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            String message = "Step 1:\tThere will be 2 columns.One will provide you with" +
                             " \t\thints the other possible answers.\nStep2:\tYou will " +
                             "enter the answers in the dropdown boxes.\nStep3:\tYou can then check your answers.\nOnce done you may exit the applciation, go to the main menu screen or retry with a new set of questions.";
            MessageBox.Show(message, "Help Screen", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

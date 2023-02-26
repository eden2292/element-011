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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace element_011
{
  
    //Initialise class of books to be stored in XML file. 
    public class Book
    {
        public String author;
        public String title;
        public String ISBN;
        public String description;
    }
    public partial class MainWindow : Window
    {
        //Variables that can be used in place of file path within xml accessing methods. This means that if the XML file name changes, it only needs to be replaced here. 
        private String xmlBookFilePath => "BookInventory.xml";
        private String xmlUserFilePath => "UserList.xml";

        //Boolean that will switch to true if the correct Id is entered. If it does not change to true, show an error message in the log in control if no node matches the user ID entered. 
        public bool AccessSuccess;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Log in page button control method to direct users to the correct landing page based on a tag in their relative XML node. 
        private void btnAccess_Click(object sender, RoutedEventArgs e)
        {
            //Access XML file with user information in. 
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlUserFilePath);
            XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/catalog/User");

            //loop through each node in the XML file and check against "User ID"
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                XmlNode userNo = xmlNode.SelectSingleNode("UserID");

                //If the User ID matches a node in the XML file, check the tag node. 
                if (txtUserId.Text == userNo.InnerText)
                {
                    XmlNode tag = xmlNode.SelectSingleNode("Tag");

                    //switch to landing page based on the "tag" node. 
                    if (tag.InnerText == "Staff")
                    {
                        tabControl.SelectedItem = tabStaffHPage;
                        AccessSuccess = true;
                    }
                    else if (tag.InnerText == "Member")
                    {
                        tabControl.SelectedItem = tabUserHPage;
                        AccessSuccess = true;
                    }
                }

            }
            //If previously instantiated bool has not changed value to "true", a message box shows. 
            if (AccessSuccess != true)
            {
                MessageBox.Show("User ID does not match our records");
            }
        }

        #region User_tabcontrols
        //move to checkout tab. 
        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabCheckout;
        }

        //move to return tab
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabReturn;
        }

        //move to renew tab
        private void btnRenew_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabRenew;
        }

        //move to view checkout tab
        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabViewCheckouts;
        }

        //move to search tab
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabSearch;
        }
        #endregion

        #region Staff_tabcontrols
        //move to manage tab
        private void btnMngBks_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabManageBooks;
        }

        //move to reports tab
        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabReports;
        }
        #endregion

        #region End_tabcontrols
        //returns to log in page
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabLogIn;
        }

        //member-specific cancel button - returns to the member home page
        private void btnCancelUser_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabUserHPage;
        }

        //staff specific cancel button - returns to staff home page. 
        private void btnCancelStaff_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabStaffHPage;
        }
        #endregion

        //Add books to the XML file that holds inventory
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //call the book class and create a new instance. 
            Book newBook = new Book();

            //take information entered into textboxes and assign to the variables in the book class. 
            newBook.title = txtTitleAddRem.Text;
            newBook.author = txtAuthorAddRem.Text;
            newBook.ISBN = txtISBNAddRem.Text;
            newBook.description = txtDescAddRem.Text;

            //access xml document with book information
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlBookFilePath);

            //create nodes, adding the stored information into the child nodes for each book. 
            XmlElement book = doc.CreateElement("Book");
            XmlElement title = doc.CreateElement("Title");
            title.InnerText = newBook.title;
            XmlElement author = doc.CreateElement("Author");
            author.InnerText = newBook.author;
            XmlElement ISBN = doc.CreateElement("ISBN");
            ISBN.InnerText = newBook.ISBN;
            XmlElement description = doc.CreateElement("Description");
            description.InnerText = newBook.description;

            book.AppendChild(title);
            book.AppendChild(author);
            book.AppendChild(ISBN);
            book.AppendChild(description);

            doc.DocumentElement.AppendChild(book);
            //save the document with the appended information. 
            doc.Save(xmlBookFilePath);

        }

        //remove books from the xml document that hold information - CURRENT ISSUE - if multiple books with the same information is held, then all will be removed. 
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlBookFilePath);

            XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/catalog/Book");

            //for each to loop through each node and check if it matched the text entered into the text boxes. 
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                //append node information to a variable to be checked against the entered information
                XmlNode node = xmlNode.SelectSingleNode("Title");
                XmlNode node1 = xmlNode.SelectSingleNode("Author");
                XmlNode node2 = xmlNode.SelectSingleNode("ISBN");

                //compare variable to text box entry. Both title and author OR ISBN must be entered. 
                if (node.InnerText == txtTitleAddRem.Text && node1.InnerText == txtAuthorAddRem.Text || node2.InnerText == txtISBNAddRem.Text)
                {
                    //if the nodes are matched, access the parent node (book) and remove all information contained within. 
                    XmlNode parent = node.ParentNode.ParentNode;
                    parent.RemoveChild(xmlNode);
                }

                xmlDocument.Save(xmlBookFilePath);

            }

        }


        //Load a report that shows all information in the XML file in an easily readable format. CHANGE - make searchable based on parameters selected by the user. 
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            //empties the text box on button click to avoid double-printing. 
            tbBookReport.Text = String.Empty;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlBookFilePath);
            XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/catalog/Book");

            //loop through each node in the XML file and print it in a readable way. 
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                tbBookReport.Text += "Title: " + xmlNode.SelectSingleNode("Title").InnerText + "\n";
                tbBookReport.Text += "Author: " + xmlNode.SelectSingleNode("Author").InnerText + "\n";
                tbBookReport.Text += "ISBN: " + xmlNode.SelectSingleNode("ISBN").InnerText + "\n";
                tbBookReport.Text += "Description: " + xmlNode.SelectSingleNode("Description").InnerText + "\n";
            }
        }

    }
}
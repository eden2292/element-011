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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

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
        private String xmlBookFilePath => "BookInventory.xml";
        private String xmlUserFilePath => "UserList.xml";
        public XmlDocument XmlDocument { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
        }


        //Button controls for user homepage.
        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabCheckout;
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabReturn;
        }

        private void btnRenew_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabRenew;
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabViewCheckouts;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabSearch;
        }

        //Button controls for staff homepage
        private void btnMngBks_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabManageBooks;
        }
        //Add books to the XML file that holds inventory
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {    
            
                Book newBook = new Book();

                newBook.title = txtTitleAddRem.Text;
                newBook.author = txtAuthorAddRem.Text;
                newBook.ISBN = txtISBNAddRem.Text;
                newBook.description = txtDescAddRem.Text;

                XmlDocument doc = new XmlDocument();
                doc.Load(xmlBookFilePath);

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
                doc.Save(xmlBookFilePath);
            
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlBookFilePath);

            XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/catalog/Book");

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                XmlNode node = xmlNode.SelectSingleNode("Title");
                XmlNode node1 = xmlNode.SelectSingleNode("Author");
                XmlNode node2 = xmlNode.SelectSingleNode("ISBN");

                
                if (node.InnerText == txtTitleAddRem.Text && node1.InnerText == txtAuthorAddRem.Text || node2.InnerText == txtISBNAddRem.Text)
                {
                    XmlNode parent = node.ParentNode.ParentNode;
                    parent.RemoveChild(xmlNode);
                }

                xmlDocument.Save(xmlBookFilePath);

            }

        }



        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            tbBookReport.Text = String.Empty;
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlBookFilePath);
            XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/catalog/Book");

            foreach(XmlNode xmlNode in xmlNodeList)
            {
                tbBookReport.Text += "Title: " + xmlNode.SelectSingleNode("Title").InnerText + "\n";
                tbBookReport.Text += "Author: " + xmlNode.SelectSingleNode("Author").InnerText + "\n";
                tbBookReport.Text += "ISBN: " + xmlNode.SelectSingleNode("ISBN").InnerText + "\n";
                tbBookReport.Text += "Description: " + xmlNode.SelectSingleNode("Description").InnerText + "\n";
                

            }
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabReports;
        }

        //Button controls to log out and cancel, returning to previous page. 
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabLogIn;
        }
        private void btnCancelUser_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabUserHPage;
        }

        private void btnCancelStaff_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabStaffHPage;
        }

        private void btn(object sender, RoutedEventArgs e)
        {

        }

        private void btnAccess_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlUserFilePath);
            XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/catalog/User");


            foreach (XmlNode xmlNode in xmlNodeList)
            {
                XmlNode userNo = xmlNode.SelectSingleNode("UserID");

                if (txtUserId.Text == userNo.InnerText)
                 {
                     XmlNode tag = xmlNode.SelectSingleNode("Tag");

                    if(tag.InnerText == "Staff")
                    {
                        tabControl.SelectedItem = tabStaffHPage;
                    }
                    else if(tag.InnerText == "Member")
                    {
                        tabControl.SelectedItem = tabUserHPage;
                    }
                }
                else if(txtUserId.Text != userNo.InnerText)
                {
                    MessageBox.Show("ID number does not match our records");
                }
            }
    }
    }


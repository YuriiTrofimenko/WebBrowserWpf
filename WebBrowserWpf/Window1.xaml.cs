using mshtml;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WebBrowserWpf
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private HtmlBridge htmlBridge;
        //MainWindow objMainWindow;
        public Window1()
        {
            InitializeComponent();

            htmlBridge = new HtmlBridge();
        }

        /*private void ProcessElement(IHTMLElement iHTMLElement, ItemCollection itemCollection)
        {
            throw new NotImplementedException();
        }*/

        private void ProcessElement(IHTMLElement parentElement, ItemCollection nodes)
        {
            //set the Microsoft HTML Object Library
            foreach (IHTMLElement element in (IHTMLElementCollection)parentElement.children)
            {
                TreeViewItem node = new TreeViewItem();
                node.Header = "<" + element.tagName + ">";
                nodes.Add(node);
                if ((((IHTMLElementCollection)(element.children)).length == 0) && (element.innerText != null))
                {
                    node.Items.Add(element.innerText);
                }
                else
                {
                    ProcessElement(element, node.Items);
                }
            }
        }

        private void treeView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = Cursors.Wait;

            // Объект DOM из WebBrowser
            HTMLDocument dom = (HTMLDocument)webBrowser.Document;
            ProcessElement(dom.documentElement, treeView.Items);

            this.Cursor = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            goHome();
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            goHome();
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            //webBrowser.ObjectForScripting = htmlBridge;
            webBrowser.Navigate(
                new Uri("file:///"
                    + Path.Combine(Path.GetDirectoryName(Application.ResourceAssembly.Location)
                        , "Html/about.html")));
        }

        private void goHome() {

            webBrowser.ObjectForScripting = htmlBridge;
            webBrowser.Navigate(
                new Uri("file:///"
                    + Path.Combine(Path.GetDirectoryName(Application.ResourceAssembly.Location)
                        , "Html/index.html")));
        }
    }
}

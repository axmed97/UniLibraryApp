using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pcLibrary.Location = new Point((pcLibrary.Parent.ClientSize.Width - pcLibrary.ClientSize.Width) / 2,
                (pcLibrary.Parent.ClientSize.Height - pcLibrary.ClientSize.Height) / 2
                );
        }

        private void addBooksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddBooks addBooks = new AddBooks();
            addBooks.ShowDialog();
        }

        private void ordersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookToReader bookToReader = new BookToReader();
            bookToReader.ShowDialog();
        }
    }
}

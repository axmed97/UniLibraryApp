using LibraryApp.Models;
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
    public partial class AddBooks : Form
    {
        UniLibraryDbEntities _context = new UniLibraryDbEntities();
        Book selected;
        public AddBooks()
        {
            InitializeComponent();
        }

        private void FillBookDataGridView()
        {
            dtgBooks.DataSource = _context.Books.Select(b => new
            {
                b.Id,
                b.Book_Name,
                b.Count,
                b.Publish_Date,
                b.Author.Author_Name
            }).ToList();
            dtgBooks.Columns[0].Visible = false;
        }

        private void FillAuthorsComboBox()
        {
            cmbAuthors.Items.AddRange(_context.Authors.Select(a => a.Author_Name).ToArray());
        }

        private void AddBooks_Load(object sender, EventArgs e)
        {
            FillBookDataGridView();
            FillAuthorsComboBox();
        }

        private void CleareAllFields()
        {
            txtBookName.Text = string.Empty;
            cmbAuthors.Items.Clear();
            nmBookCount.Value = 1;
            dtPublishDate.Value = DateTime.Now;
        }

        private void ButtonVisible(string text)
        {
            if (text == "add")
            {
                btnAddBook.Visible = false;
                btnEditBook.Visible = true;
                btnDeleteBook.Visible = true;
                btnCancel.Visible = true;
            }
            else
            {
                btnAddBook.Visible = true;
                btnEditBook.Visible = false;
                btnDeleteBook.Visible = false;
                btnCancel.Visible = false;
            }
        }

        private void btnAddBook_Click(object sender, EventArgs e)
        {
            string bookName = txtBookName.Text;
            string authors = cmbAuthors.Text;
            int bookCount = (int)nmBookCount.Value;
            DateTime publishDate = dtPublishDate.Value;
            if (!string.IsNullOrWhiteSpace(bookName) && !string.IsNullOrWhiteSpace(authors))
            {
                lblError.Visible = false;
                Book book = new Book();
                book.Book_Name = bookName;
                book.Count = bookCount;
                book.Publish_Date = publishDate;
                book.AuthorId = _context.Authors.First(a => a.Author_Name == authors).Id;
                _context.Books.Add(book);
                _context.SaveChanges();
                MessageBox.Show("Book added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CleareAllFields();
                FillAuthorsComboBox();
                FillBookDataGridView();
            }
            else
            {
                lblError.Visible = true;
            }
            ButtonVisible("addBook");
        }

        private void dtgBooks_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ButtonVisible("add");
            int bookId = (int)dtgBooks.Rows[e.RowIndex].Cells[0].Value;
            selected = _context.Books.First(b => b.Id == bookId);
            txtBookName.Text = selected.Book_Name;
            cmbAuthors.Text = selected.Author.Author_Name;
            nmBookCount.Value = (int)selected.Count;
            dtPublishDate.Value = selected.Publish_Date;
        }

        private void btnEditBook_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtBookName.Text))
            {
                lblError.Visible = false;
                int authorId = _context.Authors.First(a => a.Author_Name == cmbAuthors.Text).Id;
                selected.Book_Name = txtBookName.Text;
                selected.Count = (int)nmBookCount.Value;
                selected.AuthorId = authorId;
                selected.Publish_Date = dtPublishDate.Value;
                _context.SaveChanges();
                MessageBox.Show("Book edited successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CleareAllFields();
                FillAuthorsComboBox();
                FillBookDataGridView();
                ButtonVisible("edit");
            }
            else
            {
                lblError.Visible = true;
            }
            
        }

        private void btnDeleteBook_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete this Book?", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.OK)
            {
                _context.Books.Remove(selected);
                _context.SaveChanges();
                CleareAllFields();
                FillAuthorsComboBox();
                FillBookDataGridView();
                ButtonVisible("delete");
                MessageBox.Show("Book deleted successfully!", "Success", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ButtonVisible("cancel");
            lblError.Visible = false;
            CleareAllFields();
            FillAuthorsComboBox();
            FillBookDataGridView();
        }
    }
}

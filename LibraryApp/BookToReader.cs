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
    public partial class BookToReader : Form
    {
        UniLibraryDbEntities _context = new UniLibraryDbEntities();
        Reader_to_Book selected;
        public BookToReader()
        {
            InitializeComponent();
        }
        private void FillReaderToBookDataGridView()
        {
            dtgReaderToBook.DataSource = _context.Reader_to_Book.Select(btr => new
            {
                btr.Id,
                btr.Book.Book_Name,
                btr.Reader.Fullname,
                btr.Get_Book,
                btr.Deadline
            }).ToList();
            dtgReaderToBook.Columns[0].Visible = false;
        }
        private void FillReadersComboBox()
        {
            cmbReader.Items.AddRange(_context.Readers.Select(r => r.Fullname).ToArray());
        }
        private void FillBooksComboBox()
        {
            cmbBookName.Items.AddRange(_context.Books.Select(r => r.Book_Name).ToArray());
        }
        private void ClearAllField()
        {
            cmbBookName.Items.Clear();
            cmbReader.Items.Clear();
            dtDeadline.Value = DateTime.Now;
            dtGetDate.Value = DateTime.Now;
        }
        private void FillElements()
        {
            ClearAllField();
            FillReaderToBookDataGridView();
            FillReadersComboBox();
            FillBooksComboBox();
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
        private void BookToReader_Load(object sender, EventArgs e)
        {
            FillElements();
        }

        private void btnAddBook_Click(object sender, EventArgs e)
        {
            string bookName = cmbBookName.Text;
            string reader = cmbReader.Text;
            DateTime getDate = dtGetDate.Value;
            DateTime deadeline = dtDeadline.Value;
            if (!string.IsNullOrWhiteSpace(bookName) && !string.IsNullOrWhiteSpace(reader))
            {
                lblError.Visible = false;
                Reader_to_Book reader_To_Book = new Reader_to_Book();
                reader_To_Book.BookId = _context.Books.First(b => b.Book_Name == bookName).Id;
                reader_To_Book.ReaderId = _context.Readers.First(r => r.Fullname == reader).Id;
                reader_To_Book.Get_Book = getDate;
                reader_To_Book.Deadline = deadeline;
                _context.Reader_to_Book.Add(reader_To_Book);
                _context.SaveChanges();
                FillElements();
            }
            else
            {
                lblError.Visible = true;
            }
            ButtonVisible("addBook");
        }

        private void dtgReaderToBook_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ButtonVisible("add");
            int orderId = (int)dtgReaderToBook.Rows[e.RowIndex].Cells[0].Value;
            selected = _context.Reader_to_Book.First(rtb => rtb.Id == orderId);
            cmbBookName.Text = selected.Book.Book_Name;
            cmbReader.Text = selected.Reader.Fullname;
            dtDeadline.Value = selected.Deadline;
            dtGetDate.Value = selected.Get_Book;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            FillElements();
            ButtonVisible("cancel");
        }

        private void btnDeleteBook_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete this Order?", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.OK)
            {
                _context.Reader_to_Book.Remove(selected);
                _context.SaveChanges();
                FillElements();
                MessageBox.Show("Order deleted successfully!", "Success", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
            ButtonVisible("cancel");
        }

        private void btnEditBook_Click(object sender, EventArgs e)
        {
            ButtonVisible("edit");
            
            if (!string.IsNullOrWhiteSpace(cmbBookName.Text) && !string.IsNullOrWhiteSpace(cmbReader.Text))
            {
                lblError.Visible = false;
                selected.Get_Book = dtGetDate.Value;
                selected.Deadline = dtDeadline.Value;
                selected.ReaderId = _context.Readers.First(r => r.Fullname == cmbReader.Text).Id;
                selected.BookId = _context.Books.First(r => r.Book_Name == cmbBookName.Text).Id;
                _context.SaveChanges();
                FillElements();
            }
            else
            {
                lblError.Visible = true;
            }
        }
    }
}

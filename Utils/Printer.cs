using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace OnlineQuee.Utils
{
    public class Printer
    {
        private static PrintDialog _PrintDialog;

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean SetDefaultPrinter(String name);

        public Printer()
        {
            _PrintDialog = new PrintDialog();
            
            SetDefaultPrinter("CUSTOM VKP80 II");
            //if (_PrintDialog.ShowDialog() != true) {
            //    throw new Exception("Printer not connected");
            //}
        }

        public void Print(string data, int size)
        {
            // clean queue of printer
            // _PrintDialog.PrintQueue.Purge();
            DataGrid dataGrid = new DataGrid();
            FlowDocument fd = new FlowDocument();
            Paragraph p = new Paragraph(new Run(data));
            p.FontStyle = dataGrid.FontStyle;
            p.FontFamily = dataGrid.FontFamily;
            p.FontSize = 18;
            fd.Blocks.Add(p);

            Table table = new Table();
            TableRowGroup tableRowGroup = new TableRowGroup();
            TableRow r = new TableRow();
            fd.PageWidth = _PrintDialog.PrintableAreaWidth;
            fd.PageHeight = _PrintDialog.PrintableAreaHeight;
            fd.BringIntoView();

            fd.TextAlignment = TextAlignment.Center;
            fd.ColumnWidth = 500;
            table.CellSpacing = 0;

            var headerList = dataGrid.Columns.Select(e => e.Header.ToString()).ToList();


            for (int j = 0; j < headerList.Count; j++)
            {

                r.Cells.Add(new TableCell(new Paragraph(new Run(headerList[j]))));
                r.Cells[j].ColumnSpan = 4;
                r.Cells[j].Padding = new Thickness(4);

                r.Cells[j].BorderBrush = Brushes.Black;
                r.Cells[j].FontWeight = FontWeights.Bold;
                r.Cells[j].Background = Brushes.DarkGray;
                r.Cells[j].Foreground = Brushes.White;
                r.Cells[j].BorderThickness = new Thickness(1, 1, 1, 1);
            }
            tableRowGroup.Rows.Add(r);
            table.RowGroups.Add(tableRowGroup);
            for (int i = 0; i < dataGrid.Items.Count; i++)
            {

                DataRowView row = (DataRowView)dataGrid.Items.GetItemAt(i);

                table.BorderBrush = Brushes.Gray;
                table.BorderThickness = new Thickness(1, 1, 0, 0);
                table.FontStyle = dataGrid.FontStyle;
                table.FontFamily = dataGrid.FontFamily;
                //table.FontSize = size;
                table.FontSize = 13;
                tableRowGroup = new TableRowGroup();
                r = new TableRow();
                for (int j = 0; j < row.Row.ItemArray.Count(); j++)
                {

                    r.Cells.Add(new TableCell(new Paragraph(new Run(row.Row.ItemArray[j].ToString()))));
                    r.Cells[j].ColumnSpan = 4;
                    r.Cells[j].Padding = new Thickness(4);

                    r.Cells[j].BorderBrush = Brushes.DarkGray;
                    r.Cells[j].BorderThickness = new Thickness(0, 0, 1, 1);
                }

                tableRowGroup.Rows.Add(r);
                table.RowGroups.Add(tableRowGroup);

            }
            fd.Blocks.Add(table);

            _PrintDialog.PrintDocument(((IDocumentPaginatorSource)fd).DocumentPaginator, "");
        }

    }
}

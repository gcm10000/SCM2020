using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SCM2020___Client
{
    //https://social.msdn.microsoft.com/Forums/aspnet/en-US/1e46fe05-7ba2-4d40-809b-17e5cb6606c7/creating-header-and-footer-for-a-documentpaginator?forum=wpf
    public class DocumentPaginatorWrapper : DocumentPaginator
    {
        Size m_PageSize;
        Size m_Margin;
        DocumentPaginator m_Paginator;
        Typeface m_Typeface;
        String m_DocumentTitle;
        String m_DocumentFooter;
        public DocumentPaginatorWrapper(DocumentPaginator paginator, Size pageSize, Size margin, string DocumentTitle, string DocumentFooter)
        {
            m_PageSize = pageSize;
            m_Margin = margin;
            m_Paginator = paginator;
            m_DocumentTitle = DocumentTitle;
            m_DocumentFooter = DocumentFooter;

            m_Paginator.PageSize = new Size(m_PageSize.Width - margin.Width * 2, m_PageSize.Height - margin.Height * 2);
        }
        Rect Move(Rect rect)
        {
            if (rect.IsEmpty)
            {
                return rect;
            }
            else
            {
                return new Rect(rect.Left + m_Margin.Width, rect.Top + m_Margin.Height, rect.Width, rect.Height);
            }
        }
        public void DrawFunction(DrawingVisual content, string drawContent, Point point)
        {
            using (DrawingContext ctx = content.RenderOpen())
            {
                if (m_Typeface == null)
                {
                    m_Typeface = new Typeface("Arial");
                }

                FormattedText text = new FormattedText(drawContent,
                    System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    m_Typeface, 14, Brushes.Black);

                ctx.DrawText(text, point);
            }
        }
        public override DocumentPage GetPage(int pageNumber)
        {
            DocumentPage page = m_Paginator.GetPage(pageNumber);

            // Create a wrapper visual for transformation and add extras
            ContainerVisual newpage = new ContainerVisual();
            //Title
            DrawingVisual pagetitle = new DrawingVisual();
            DrawFunction(pagetitle, m_DocumentTitle, new Point(m_PageSize.Width / 2 - 100, -96 / 4));
            //Page Number
            DrawingVisual pagenumber = new DrawingVisual();
            DrawFunction(pagenumber, "Page " + (pageNumber + 1), new Point(m_PageSize.Width - 200, m_PageSize.Height - 100));
            //Footer
            DrawingVisual pagefooter = new DrawingVisual();
            DrawFunction(pagefooter, m_DocumentFooter, new Point(m_PageSize.Width / 2 - 100, m_PageSize.Height - 100));


            DrawingVisual background = new DrawingVisual();

            using (DrawingContext ctx = background.RenderOpen())
            {
                ctx.DrawRectangle(new SolidColorBrush(Color.FromRgb(240, 240, 240)), null, page.ContentBox);
            }

            newpage.Children.Add(background); // Scale down page and center

            ContainerVisual smallerPage = new ContainerVisual();
            smallerPage.Children.Add(page.Visual);
            //smallerPage.Transform = new MatrixTransform(0.95, 0, 0, 0.95,
            //    0.025 * page.ContentBox.Width, 0.025 * page.ContentBox.Height);

            newpage.Children.Add(smallerPage);
            newpage.Children.Add(pagetitle);
            newpage.Children.Add(pagenumber);
            newpage.Children.Add(pagefooter);


            newpage.Transform = new TranslateTransform(m_Margin.Width, m_Margin.Height);

            return new DocumentPage(newpage, m_PageSize, Move(page.BleedBox), Move(page.ContentBox));
        }
        public override bool IsPageCountValid
        {
            get
            {
                return m_Paginator.IsPageCountValid;
            }
        }
        public override int PageCount
        {
            get
            {
                return m_Paginator.PageCount;
            }
        }
        public override Size PageSize
        {
            get
            {
                return m_Paginator.PageSize;
            }
            set
            {
                m_Paginator.PageSize = value;
            }
        }
        public override IDocumentPaginatorSource Source
        {
            get
            {
                return m_Paginator.Source;
            }
        }
    }
}

using MSHTML;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SCM2020___Client
{
    public class PrintHtmlDocument : DocumentPaginator
    {
        private readonly WebBrowser webBrowser;
        private readonly int pageScroll;
        private Size pageSize;

        public PrintHtmlDocument(WebBrowser webBrowser, int pageScroll, double pageHeight, double pageWidth)
        {
            this.webBrowser = webBrowser;
            this.pageScroll = pageScroll;
            pageSize = new Size(pageWidth, pageHeight);
        }

        public override DocumentPage GetPage(int pageNumber)
        {
            HTMLDocument htmlDoc = webBrowser.Document as HTMLDocument;
            if (htmlDoc != null) htmlDoc.parentWindow.scrollTo(0, pageScroll * pageNumber);
            Rect area = new Rect(pageSize);

            return new DocumentPage(webBrowser, pageSize, area, area);
        }

        public override bool IsPageCountValid
        {
            get { return true; }
        }

        /// <summary>
        /// Returns one less than actual length.
        /// Last page should be whitespace, used for scrolling.
        /// </summary>
        public override int PageCount
        {
            get
            {
                var doc = (IHTMLDocument2)webBrowser.Document;
                
                var height = ((IHTMLElement2)doc.body).scrollHeight;
                int tempVal = height * 10 / pageScroll;
                tempVal = tempVal % 10 == 0
                    ? Math.Max(height / pageScroll, 1)
                    : height / pageScroll + 1;
                return tempVal > 1 ? tempVal - 1 : tempVal;
            }
        }

        public override Size PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = value;
            }
        }

        /// <summary>
        /// Can be null.
        /// </summary>
        public override IDocumentPaginatorSource Source
        {
            get
            {
                return null;
            }
        }
    }
}

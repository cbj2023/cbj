using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;//不需要的
using System.Drawing.Imaging;
using System.Drawing.Printing;

using System.Windows.Forms;
namespace SUTAIMES.Common
{
    class ClsPrintDocument
    {


        /// <summary>
        /// 打印
        /// </summary>
        
        PrintDocument pd = new PrintDocument();

        public ClsPrintDocument(string _PrintName)
        {
            pd.PrintPage += new PrintPageEventHandler(printDocument_PrintMTPage);
            pd.DefaultPageSettings.PrinterSettings.PrinterName = _PrintName;// "ZDesigner ZT610-600dpi ZPL一线";       //打印机名称
            //pd.DefaultPageSettings.Landscape = true;  //设置横向打印，不设置默认是纵向的
            pd.PrintController = new System.Drawing.Printing.StandardPrintController();
         }
        public void MyAutoprinter(string[] _PrintData)//自动
        {
            tclsPrintData.Customer_name = _PrintData[0];
            tclsPrintData.Order_id = _PrintData[1];
            tclsPrintData.Size  = _PrintData[2];
            tclsPrintData.Order_singlename  = _PrintData[3];
            tclsPrintData.LineName  = _PrintData[4];
            tclsPrintData.QCname  = _PrintData[5];
            pd.DocumentName = "自动"+pd.DefaultPageSettings.PrinterSettings.PrinterName;
            pd.Print();
        }
        public void MyManlprinter(string[] _PrintData)//手动
        {
            tclsPrintData.Customer_name = _PrintData[0];
            tclsPrintData.Order_id = _PrintData[1];
            tclsPrintData.Size = _PrintData[2];
            tclsPrintData.Order_singlename = _PrintData[3];
            tclsPrintData.LineName = _PrintData[4];
            tclsPrintData.QCname = _PrintData[5];
            pd.DocumentName = "手动" + pd.DefaultPageSettings.PrinterSettings.PrinterName;
            pd.Print();
        }
        class clsPrintData
        {
            public string Customer_name { get; set; }
            public string Order_id { get; set; }
            public string Size { get; set; }
            public string Order_singlename { get;set;}
            public string LineName { get; set; }
            public string QCname { get; set; }
        }
        clsPrintData tclsPrintData = new clsPrintData();

        int tLeft1 = clsMyPublic.mLeft1.Trim() == "" ? 53 : int.Parse(clsMyPublic.mLeft1), tLeft2 = clsMyPublic.mLeft2.Trim() == "" ? 182 : int.Parse(clsMyPublic.mLeft2);
        int tTop1 = clsMyPublic.mTop1.Trim() == "" ? 73 : int.Parse(clsMyPublic.mTop1), tTopTemp = clsMyPublic.mTopTemp.Trim() == "" ? 32 : int.Parse(clsMyPublic.mTopTemp);
        private void printDocument_PrintMTPage(object sender, PrintPageEventArgs e)
        {
            Font titleFont = new Font("黑体", 11, System.Drawing.FontStyle.Bold);//标题字体           
            Font fntTxt = new Font("黑体", 8, System.Drawing.FontStyle.Regular);//正文文字         
            Font fntTxt1 = new Font("宋体", 6, System.Drawing.FontStyle.Regular);//正文文字           
            System.Drawing.Brush brush = new SolidBrush(System.Drawing.Color.Black);//画刷           
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Black);           //线条颜色         

            

            try
            {
                //e.Graphics.DrawString("标题name", titleFont, brush, new System.Drawing.Point(20, 10));

                //Point[] points111 = { new Point(20, 28), new Point(230, 28) };
                //e.Graphics.DrawLines(pen, points111);

                e.Graphics.DrawString(tclsPrintData.Customer_name, fntTxt, brush, new System.Drawing.Point(tLeft1, tTop1));

                e.Graphics.DrawString(tclsPrintData.Order_id, fntTxt, brush, new System.Drawing.Point(tLeft1, tTop1 + tTopTemp));

                e.Graphics.DrawString(tclsPrintData.Size , fntTxt, brush, new System.Drawing.Point(tLeft2, tTop1 + tTopTemp));

                e.Graphics.DrawString(tclsPrintData.Order_singlename, fntTxt, brush, new System.Drawing.Point(tLeft1, tTop1 + tTopTemp * 2));

                e.Graphics.DrawString(tclsPrintData.LineName , fntTxt, brush, new System.Drawing.Point(tLeft1, tTop1 + tTopTemp*3+1));

                e.Graphics.DrawString(tclsPrintData.QCname , fntTxt, brush, new System.Drawing.Point(tLeft2, tTop1 + tTopTemp*3+1));

              

                //Bitmap bitmap = CreateQRCode("此处为二维码数据");
                //e.Graphics.DrawImage(bitmap, new System.Drawing.Point(20, 10));

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        /// <summary>
        /// 二维码方法
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        //public static Bitmap CreateQRCode(string asset)
        //{
        //    EncodingOptions options = new QrCodeEncodingOptions
        //    {
        //        DisableECI = true,
        //        CharacterSet = "UTF-8", //编码
        //        Width = 80,             //宽度
        //        Height = 80             //高度
        //    };
        //    BarcodeWriter writer = new BarcodeWriter();
        //    writer.Format = BarcodeFormat.QR_CODE;
        //    writer.Options = options;
        //    return writer.Write(asset);
        //}
        /// <summary>
        /// /
        /// </summary>


    }
}

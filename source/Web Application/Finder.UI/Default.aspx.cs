using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Finder.Lib;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using System.Collections;
using System.Data;

namespace Finder.UI
{
    public partial class _Default : System.Web.UI.Page
    {
        #region Fields

        private String path = "result.xml";
        private ShopResult searchResult = null;

        #endregion

        #region Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            addressList.DataSource = CreateDataSource();
            addressList.DataTextField = "AddressTextField";
            addressList.DataValueField = "AddressValueField";
            addressList.DataBind();
        }

        protected ICollection CreateDataSource()
        {
            DataTable table = new DataTable();

            table.Columns.Add(new DataColumn("AddressTextField", typeof(String)));
            table.Columns.Add(new DataColumn("AddressValueField", typeof(String)));

            DataRow row = table.NewRow();
            row[0] = "panasonic.shop.by";
            row[1] = "panasonic.shop.by";
            table.Rows.Add(row);

            DataView dv = new DataView(table);
            return dv;
        }


        protected void OnFindClickHandler(object sender, EventArgs e)
        {
            searchResult = null;
            String inputFilePath = Server.MapPath(String.Format("xml/input/{0}.xml", addressList.Text));
            String outputFilePath = Server.MapPath(String.Format("xml/output/{0}.xml", addressList.Text));
            using (FileStream input = File.Open(Server.MapPath("xml/input/panasonic.shop.by.xml"), FileMode.Open))
            using (FileStream output = File.Create(outputFilePath))
            {
                try
                {
                    SearchProcess process = new SearchProcess(input, output);
                    searchResult = (ShopResult)process.GetSearchResult(new ShopProductFactory(), new List<String>());//"Монитор", "Принтер", "Телевизор" });
                }
                catch
                {
                    input.Close();
                    output.Close();
                }
            }
            
        }


        protected void OnGetResultClick(object sender, EventArgs e)
        {
            if (searchResult == null)
            {
                using (FileStream input = File.Open(path, FileMode.Open))
                {
                    searchResult = (ShopResult)XmlHelper.Deserialize(input, typeof(ShopResult));
                }
            }
            if (sender == getHtml)
                GetHtml();
            else if (sender == getXml)
                GetXml();
        }


        private void GetXml()
        {
            if (searchResult != null)
            {
                Response.Clear();
                Response.ContentType = "text/xml";
                Response.BinaryWrite(File.ReadAllBytes(path));
                Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", path));
                Response.End();
            }
        }


        private void GetHtml()
        {
            if (searchResult != null)
            {
                result.Text = "";
                foreach (ShopProduct product in searchResult.ProductList)
                {
                    result.Text += "<table border='1'><tr>";
                    result.Text += String.Format("<td>{0}</td></tr><tr><td>{1}</td></tr><tr><td>Цена: ", product.Title, product.Image);
                    foreach (String price in product.PriceList)
                    {
                        result.Text += String.Format("{0}", price);
                    }
                    result.Text += "</td></tr>";
                    result.Text += String.Format("<tr><td>{0}</td></tr>", product.Description);
                    result.Text += "</table><br /><br />";
                }
            }
        }


        //protected void BuildResult()
        //{
        //    if (searchResult == null)
        //    {
        //        using (FileStream input = File.Open(path, FileMode.Open))
        //        {
        //            searchResult = (ShopResult)XmlHelper.Deserialize(input, typeof(ShopResult));
        //        }
        //    }
        //    if (searchResult != null)
        //    {
        //        result.Text = "<table border='1'>";
        //        foreach (ShopProduct product in searchResult.ProductList)
        //        {
        //            result.Text += "<tr><td><table><tr>";
        //            result.Text += String.Format("<td>{0}</td></tr><tr><td>{1}</td></tr><tr><td>", product.Title, product.Image);
        //            foreach (String price in product.PriceList)
        //            {
        //                result.Text += String.Format("{0}", price);
        //            }
        //            result.Text += "</td></tr></table></td>";
        //            result.Text += String.Format("<td>{0}</td></tr>", product.Description);
        //        }
        //        result.Text += "</table>";
        //    }
        //}

        #endregion
    }
}
